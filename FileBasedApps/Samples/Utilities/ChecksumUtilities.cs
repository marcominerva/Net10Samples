using System.Security.Cryptography;

namespace Utilities;

public class ChecksumUtilities
{
    public static async Task<string> GetMD5ChecksumAsync(string fileName)
    {
        using var stream = File.OpenRead(fileName);
        var hash = await MD5.HashDataAsync(stream);
        return Convert.ToHexString(hash);
    }
}
