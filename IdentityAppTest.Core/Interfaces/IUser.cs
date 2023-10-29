namespace IdentityAppTest.Core.Interfaces;

public interface IUser
{
    public Guid Id { get; set;  }
    public string Name { get; set; }
    public string Email { get; set; }
    public byte[] PasswordSalt {  get; set; }
    public string PasswordHash { get; set; }
}
