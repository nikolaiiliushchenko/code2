using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static byte[] ComputeHashMd5(byte[] dataForHash)
    {
        using (var md5 = MD5.Create())
        {
            return md5.ComputeHash(dataForHash);
        }
    }

    public static byte[] ComputeHashSha1(byte[] toBeHashed)
    {
        using (var sha1 = SHA1.Create())
        {
            return sha1.ComputeHash(toBeHashed);
        }
    }

    public static byte[] ComputeHashSha256(byte[] toBeHashed)
    {
        using (var sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(toBeHashed);
        }
    }

    public static byte[] ComputeHashSha384(byte[] toBeHashed)
    {
        using (var sha384 = SHA384.Create())
        {
            return sha384.ComputeHash(toBeHashed);
        }
    }

    public static byte[] ComputeHashSha512(byte[] toBeHashed)
    {
        using (var sha512 = SHA512.Create())
        {
            return sha512.ComputeHash(toBeHashed);
        }
    }

    static void PrintAllHashes(string strForHash)
    {
        byte[] data = Encoding.UTF8.GetBytes(strForHash);

        byte[] md5Hash = ComputeHashMd5(data);
        byte[] sha1Hash = ComputeHashSha1(data);
        byte[] sha256Hash = ComputeHashSha256(data);
        byte[] sha384Hash = ComputeHashSha384(data);
        byte[] sha512Hash = ComputeHashSha512(data);

        Guid guid = new Guid(md5Hash);

        Console.WriteLine($"Str: {strForHash}");
        Console.WriteLine($"MD5 ({md5Hash.Length} байт): {Convert.ToBase64String(md5Hash)}");
        Console.WriteLine($"GUID: {guid}");
        Console.WriteLine($"SHA1 ({sha1Hash.Length} байт): {Convert.ToBase64String(sha1Hash)}");
        Console.WriteLine($"SHA256 ({sha256Hash.Length} байт): {Convert.ToBase64String(sha256Hash)}");
        Console.WriteLine($"SHA384 ({sha384Hash.Length} байт): {Convert.ToBase64String(sha384Hash)}");
        Console.WriteLine($"SHA512 ({sha512Hash.Length} байт): {Convert.ToBase64String(sha512Hash)}");
        Console.WriteLine();
    }

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        const string strForHash1 = "Hello World!";
        const string strForHash2 = "Hello World!";
        const string strForHash3 = "Hello world!";

        PrintAllHashes(strForHash1);
        PrintAllHashes(strForHash2);
        PrintAllHashes(strForHash3);

        var md5ForStr1 = ComputeHashMd5(Encoding.UTF8.GetBytes(strForHash1));
        var md5ForStr2 = ComputeHashMd5(Encoding.UTF8.GetBytes(strForHash2));
        var md5ForStr3 = ComputeHashMd5(Encoding.UTF8.GetBytes(strForHash3));

        Guid guid1 = new Guid(md5ForStr1);
        Guid guid2 = new Guid(md5ForStr2);
        Guid guid3 = new Guid(md5ForStr3);

        Console.WriteLine($"(GUID1 == GUID2) = {guid1 == guid2}");
        Console.WriteLine($"(GUID1 == GUID3) = {guid1 == guid3}");
    }
}