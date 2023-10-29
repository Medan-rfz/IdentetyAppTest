using IdentityAppTest.Core.Entities.Users;
using IdentityAppTest.Helpers;
using IdentityAppTest.Infrastructure.Helpers;
using IdentityAppTest.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdentityAppTest.Controllers;

public class LoginController : Controller
{
    public class UserLoginInfo
    {
        [JsonPropertyName("login")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    };

    public class UserRegistrationInfo
    {
        [JsonPropertyName("login")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    };

    private UserRepository _userRepository;
    private ModeratorRepository _moderatorRepository;

    public LoginController()
    {
        _userRepository = new UserRepository();
        _moderatorRepository = new ModeratorRepository();
    }

    [HttpPost("api/v1/[action]")]
    public async Task<IActionResult> SignInAsync([FromBody] UserLoginInfo userLoginInfo)
    {
        var response = new ContentResult();
        response.ContentType = "application/json";
        Console.WriteLine($"Login: {userLoginInfo.Name}\tPassword: {userLoginInfo.Password}");

        var user = await _userRepository.GetByName(userLoginInfo.Name);

        if (user != null)
        {
            if (user.PasswordHash != PasswordHasher.HashPassword(userLoginInfo.Password, user.PasswordSalt))
            {
                response.StatusCode = StatusCodes.Status401Unauthorized; 
                return response;
            }

            Claim roleClaim;
            if (await _moderatorRepository.Get(user.Id) != null)
                roleClaim = new Claim(ClaimTypes.Role, "Moderator");
            else
                roleClaim = new Claim(ClaimTypes.Role, "User");

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, userLoginInfo.Name),
                roleClaim
            };

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            Console.WriteLine($"Ok: User {userLoginInfo.Name} exist");
            response.StatusCode= StatusCodes.Status200OK;
            response.Content = JsonSerializer.Serialize(new { access_token = token });
        }
        else
        {
            Console.WriteLine($"Error: User {userLoginInfo.Name} not exist");
            response.StatusCode = StatusCodes.Status401Unauthorized;
        }

        return response;
    }

    [HttpPost("api/v1/[action]")]
    public async Task<IActionResult> SignUp([FromBody] UserRegistrationInfo userRegistrationInfo)
    {
        var response = new ContentResult();

        var user = await _userRepository.GetByEmail(userRegistrationInfo.Email);
        var namedUser = await _userRepository.GetByName(userRegistrationInfo.Name);

        if (user == null && namedUser == null)
        {
            var newUser = new User();
            newUser.Name = userRegistrationInfo.Name;
            newUser.Email = userRegistrationInfo.Email;
            newUser.PasswordSalt = PasswordHasher.GetSalt();
            newUser.PasswordHash = PasswordHasher.HashPassword(userRegistrationInfo.Password, newUser.PasswordSalt);
            await _userRepository.Create(newUser);
            await _userRepository.Save();

            Console.WriteLine($"Ok: User {userRegistrationInfo.Name} was add");
            response.StatusCode = StatusCodes.Status200OK;
            response.Content = $"New user {userRegistrationInfo.Name} was append";
        }
        else if (user != null)
        {
            Console.WriteLine($"Error: User email {userRegistrationInfo.Email} already used");
            response.StatusCode = StatusCodes.Status409Conflict;
            response.Content = $"User with email ({userRegistrationInfo.Email}) already exist";
        }
        else if (namedUser != null)
        {
            Console.WriteLine($"Error: User {userRegistrationInfo.Name} already exist");
            response.StatusCode = StatusCodes.Status409Conflict;
            response.Content = $"User with name ({userRegistrationInfo.Name}) already exist";
        }

        return response;
    }
}
