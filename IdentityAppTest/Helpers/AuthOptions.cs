using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityAppTest.Helpers;

public class AuthOptions
{
    public const string ISSUER = "AuthServer";
    public const string AUDIENCE = "AuthClient";
    const string KEY = "mysupersecret_secretkey!123";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}
