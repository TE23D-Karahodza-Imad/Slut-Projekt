using System;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("=== Lösenordssäkerhet: Hashning + Brute-force-simulering ===\n");
        Console.Write("Skriv in ett lösenord: ");
        string password = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("⚠️ Du skrev inget lösenord.");
            return;
        }


        int length = password.Length;

        bool hasLower = password.Any(char.IsLower);
        bool hasUpper = password.Any(char.IsUpper);
        bool hasDigit = password.Any(char.IsDigit);
        bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

        int charsetSize = 0;
        if (hasLower) charsetSize += 26;
        if (hasUpper) charsetSize += 26;
        if (hasDigit) charsetSize += 10;
        if (hasSpecial) charsetSize += 33; // ungefärlig mängd vanliga specialtecken

        if (charsetSize == 0)
        {
            Console.WriteLine("⚠️ Kunde inte analysera teckenuppsättningen.");
            return;
        }
        string hashHex = ComputeSha256Hex(password);
        BigInteger combinations = BigInteger.Pow(new BigInteger(charsetSize), length);

        // Antag en hastighet (du kan ändra för att jämföra scenarier)
        BigInteger guessesPerSecond = new BigInteger(1_000_000_000); // 1 miljard gissningar/sek

        BigInteger avgSeconds = combinations / (2 * guessesPerSecond);
        string timeReadable = FormatSeconds(avgSeconds);
        Console.WriteLine("\n--- Resultat ---");
        Console.WriteLine($"Längd: {length}");
        Console.WriteLine($"Teckentyper: " +
                          $"{(hasLower ? "a-z " : "")}" +
                          $"{(hasUpper ? "A-Z " : "")}" +
                          $"{(hasDigit ? "0-9 " : "")}" +
                          $"{(hasSpecial ? "special " : "")}");

        Console.WriteLine($"Uppskattad teckenmängd: {charsetSize}");
        Console.WriteLine($"SHA-256 hash: {hashHex}");
        Console.WriteLine($"Möjliga kombinationer: {ToScientific(combinations)}");
        Console.WriteLine($"Antagen hastighet: {guessesPerSecond:n0} gissningar/sek");
        Console.WriteLine($"Uppskattad tid (i snitt): {timeReadable}");

        Console.WriteLine("\nTryck valfri tangent för att avsluta...");
        Console.ReadKey();
    }
    static string ComputeSha256Hex(string input)
    {
        using var sha = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        byte[] hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }

    static string FormatSeconds(BigInteger seconds)
    {
        BigInteger minute = 60;
        BigInteger hour = 60 * minute;
        BigInteger day = 24 * hour;
        BigInteger year = 365 * day;

        if (seconds < minute) return $"{seconds} sek";
        if (seconds < hour) return $"{(seconds / minute)} min";
        if (seconds < day) return $"{(seconds / hour)} h";
        if (seconds < year) return $"{(seconds / day)} dagar";
        return $"{(seconds / year)} år";
    }

    static string ToScientific(BigInteger n)
    {
        string s = n.ToString();
        if (s.Length <= 6) return s;

        int exp = s.Length - 1;
        string mantissa = s.Substring(0, 1) + "." + s.Substring(1, 2);
        return $"{mantissa}e+{exp}";
    }
}
