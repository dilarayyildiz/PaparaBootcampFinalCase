using System.Security.Cryptography;
using System.Text;

namespace ExpenseManager.Base.Encryption;

public static class PasswordGenerator
{

    public static string CreateMD5(string input)
    {
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }
    }
    

    public static string CreateMD5(string input, string salt)
    {
        var provider = MD5.Create();
        byte[] bytes = provider.ComputeHash(Encoding.ASCII.GetBytes(salt + input));
        string computedHash = BitConverter.ToString(bytes);
        return computedHash.Replace("-", "");
    }


    public static string GeneratePassword(int length)
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}