using System.Security.Cryptography;
using System.Text;
using Domain.Core.Interfaces;

namespace Domain.Core.Security;

public class Security : ISecurity
{
    /// <summary>
    /// Returns the MD5 hash of a string.
    /// </summary>
    /// <param name="value">Value to encrypt.</param>
    /// <param name="salt">Optional Salt to encrypt with.</param>
    /// <returns></returns>
    public string EncryptString(string value, string salt = "pepper")
    {
        var md5 = MD5.Create();
        var valueToBytes = Encoding.ASCII.GetBytes(value + salt);
        var bytesToHash = md5.ComputeHash(valueToBytes);

        return Convert.ToBase64String(bytesToHash);
    }
}