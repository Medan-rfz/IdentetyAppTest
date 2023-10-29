using System;
using System.Text;
using System.Security.Cryptography;

namespace IdentityAppTest.Infrastructure.Helpers;

public class PasswordHasher
{
    private static int saltLengthLimit = 32;

    public static string HashPassword(string password, byte[] salt)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] saltedPassword = new byte[salt.Length + password.Length];
            Array.Copy(salt, saltedPassword, salt.Length);
            Encoding.UTF8.GetBytes(password).CopyTo(saltedPassword, salt.Length);

            byte[] hashBytes = sha256.ComputeHash(saltedPassword);
            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hash;
        }
    }

    public static byte[] GetSalt()
    {
        return GetSalt(saltLengthLimit);
    }

    public static byte[] GetSalt(int maximumSaltLength)
    {
        var salt = new byte[maximumSaltLength];
        using (var random = new RNGCryptoServiceProvider())
        {
            random.GetNonZeroBytes(salt);
        }

        return salt;
    }
}
