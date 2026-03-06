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
        if (hasSpecial) charsetSize += 33;

        if (charsetSize == 0)
        {
            Console.WriteLine("⚠️ Kunde inte analysera teckenuppsättningen.");
            return;
        }

        string hashHex = ComputeSha256Hex(password);
        BigInteger combinations = BigInteger.Pow(new BigInteger(charsetSize), length);
        BigInteger guessesPerSecond = new BigInteger(1_000_000_000);
        BigInteger avgSeconds = combinations / (2 * guessesPerSecond);
        string timeReadable = FormatSeconds(avgSeconds);

        // NY KOD: Styrkebedömning
        int strengthScore = 0;
        if (length >= 8) strengthScore++;
        if (length >= 12) strengthScore++;
        if (hasUpper && hasLower) strengthScore++;
        if (hasDigit) strengthScore++;
        if (hasSpecial) strengthScore++;

        // NY KOD: Mönstervarning
        string[] commonPatterns = { "password", "123456", "qwerty", "abc", "admin", "welcome", "login" };
        bool hasCommonPattern = commonPatterns.Any(p => password.ToLower().Contains(p));

        string strengthLabel;
        if (strengthScore <= 2)
            strengthLabel = "⚠️  Svagt";
        else if (strengthScore <= 3)
            strengthLabel = "🟡 Medel";
        else
            strengthLabel = "✅ Starkt";

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

        // NY KOD: Skriver ut styrka och mönstervarning
        Console.WriteLine($"Lösenordsstyrka: {strengthLabel}");

        if (hasCommonPattern)
            Console.WriteLine("⚠️  Varning: Lösenordet innehåller ett vanligt mönster och är lättare att gissa.");

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