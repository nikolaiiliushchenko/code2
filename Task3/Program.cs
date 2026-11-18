using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    public static byte[] GenerateRndBytes(int length)
    {
        var rnd = RandomNumberGenerator.Create();
        byte[] random = new byte[length];
        rnd.GetBytes(random);
        return random;
    }

    public static byte[] ComputeHmacmd5(byte[] toBeHashed, byte[] key)
    {
        using (var hmac = new HMACMD5(key))
        {
            return hmac.ComputeHash(toBeHashed);
        }
    }

    public static byte[] ComputeHmacsha1(byte[] toBeHashed, byte[] key)
    {
        using (var hmac = new HMACSHA1(key))
        {
            return hmac.ComputeHash(toBeHashed);
        }
    }

    public static byte[] ComputeHmacsha256(byte[] toBeHashed, byte[] key)
    {
        using (var hmac = new HMACSHA256(key))
        {
            return hmac.ComputeHash(toBeHashed);
        }
    }

    public static byte[] ComputeHmacsha512(byte[] toBeHashed, byte[] key)
    {
        using (var hmac = new HMACSHA512(key))
        {
            return hmac.ComputeHash(toBeHashed);
        }
    }

    public static bool VerifyMessage(string message, byte[] key, byte[] receivedHmac)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        byte[] computedHmac = ComputeHmacsha256(messageBytes, key);

        string computedHmacStr = Convert.ToBase64String(computedHmac);
        string receivedHmacStr = Convert.ToBase64String(receivedHmac);

        return computedHmacStr == receivedHmacStr;
    }

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string message = "Переказ 5000 грн на рахунок UA1234567890";
        byte[] data = Encoding.UTF8.GetBytes(message);
        byte[] key = GenerateRndBytes(32);

        byte[] hmacMd5 = ComputeHmacmd5(data, key);
        byte[] hmacSha1 = ComputeHmacsha1(data, key);
        byte[] hmacSha256 = ComputeHmacsha256(data, key);
        byte[] hmacSha512 = ComputeHmacsha512(data, key);

        Console.WriteLine("MD5 HMAC");
        Console.WriteLine($"hash = {Convert.ToBase64String(hmacMd5)}\n");

        Console.WriteLine("SHA 1 HMAC");
        Console.WriteLine($"hash = {Convert.ToBase64String(hmacSha1)}\n");

        Console.WriteLine("SHA 256 HMAC");
        Console.WriteLine($"hash = {Convert.ToBase64String(hmacSha256)}\n");

        Console.WriteLine("SHA 512 HMAC");
        Console.WriteLine($"hash = {Convert.ToBase64String(hmacSha512)}\n");

        Console.WriteLine("Перевірка автентичності повідомлення:");

        bool check1 = VerifyMessage(message, key, hmacSha256);
        Console.WriteLine($"1. Оригінальне повідомлення та вірний ключ: {check1}");

        string fakeMessage = "Переказ 99999 грн на рахунок UA1234567890";
        bool check2 = VerifyMessage(fakeMessage, key, hmacSha256);
        Console.WriteLine($"2. Змінене повідомлення: {check2}");

        byte[] wrongKey = GenerateRndBytes(32);
        bool check3 = VerifyMessage(message, wrongKey, hmacSha256);
        Console.WriteLine($"3. Чужий секретний ключ: {check3}");
    }
}