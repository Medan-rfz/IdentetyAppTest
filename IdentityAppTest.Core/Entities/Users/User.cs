using IdentityAppTest.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace IdentityAppTest.Core.Entities.Users;

public class User : IUser
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordSalt { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
}
