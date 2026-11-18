using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string targetStr = "db93c299-351e-79f3-0bfc-a6424576a5a8";

        // Вариант 1: стандартный порядок байтов структуры Guid в .NET
        Guid targetGuid = new Guid(targetStr);

        // Вариант 2: прямой шестнадцатеричный порядок байтов MD5-хеша
        byte[] rawHexBytes = Convert.FromHexString(targetStr.Replace("-", ""));
        Guid targetHexGuid = new Guid(rawHexBytes);

        Console.WriteLine($"Цільовий хеш: {targetStr}");
        Console.WriteLine("Швидкий багатопотоковий перебір у різних кодуваннях...\n");

        Encoding[] encodings = new Encoding[]
        {
            Encoding.Unicode,          // UTF-16 (стандартне кодування char/string у C#)
            Encoding.UTF8,             // UTF-8 / ASCII
            Encoding.BigEndianUnicode, // UTF-16 Big Endian
            Encoding.UTF32             // UTF-32
        };

        bool found = false;

        foreach (var enc in encodings)
        {
            Console.WriteLine($"Перевірка кодування: {enc.EncodingName}...");

            Parallel.For(0, 100_000_000, (i, state) =>
            {
                string candidate = i.ToString("D8");
                byte[] bytes = enc.GetBytes(candidate);
                byte[] hash = MD5.HashData(bytes);
                Guid g = new Guid(hash);

                if (g == targetGuid || g == targetHexGuid)
                {
                    string mode = (g == targetGuid) ? "new Guid(byte[])" : "Hex-рядок";
                    Console.WriteLine($"\n[УСПІХ] Пароль відновлено: {candidate}");
                    Console.WriteLine($"Кодування: {enc.EncodingName} | Формат: {mode}");
                    found = true;
                    state.Stop();
                }
            });

            if (found) break;
        }

        if (!found)
        {
            Console.WriteLine("\nСеред 8-значних чисел у стандартних кодуваннях збігів не знайдено.");
        }
    }
}