using System.Security.Cryptography;

var hash = await GetMD5ChecksumAsync(args[0]);
Console.WriteLine(hash);

static async Task<string> GetMD5ChecksumAsync(string filename)
{
    using var stream = File.OpenRead(filename);
    var hash = await MD5.HashDataAsync(stream);
    return Convert.ToHexString(hash);
}