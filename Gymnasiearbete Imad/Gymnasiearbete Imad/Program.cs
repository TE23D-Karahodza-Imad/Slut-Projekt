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

        bool hasLower   = password.Any(char.IsLower);
        bool hasUpper   = password.Any(char.IsUpper);
        bool hasDigit   = password.Any(char.IsDigit);
        bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

        int charsetSize = 0;
        if (hasLower)   charsetSize += 26;
        if (hasUpper)   charsetSize += 26;
        if (hasDigit)   charsetSize += 10;
        if (hasSpecial) charsetSize += 33;

        if (charsetSize == 0)
        {
            Console.WriteLine("⚠️ Kunde inte analysera teckenuppsättningen.");
            return;
        }

        string     hashHex      = ComputeSha256Hex(password);
        BigInteger combinations = BigInteger.Pow(new BigInteger(charsetSize), length);

        // Hastighet 1: Modellvärde (1 miljard/sek)
        BigInteger gps1B         = new BigInteger(1_000_000_000L);
        BigInteger avgSeconds1B  = combinations / (2 * gps1B);
        string     timeModel     = FormatSeconds(avgSeconds1B);

        // Hastighet 2: RTX 4090 (30 miljarder/sek)
        BigInteger gps30B        = new BigInteger(30_000_000_000L);
        BigInteger avgSeconds30B = combinations / (2 * gps30B);
        string     timeRTX       = FormatSeconds(avgSeconds30B);

        // Styrkebedömning
        int strengthScore = 0;
        if (length >= 8)          strengthScore++;
        if (length >= 12)         strengthScore++;
        if (hasUpper && hasLower) strengthScore++;
        if (hasDigit)             strengthScore++;
        if (hasSpecial)           strengthScore++;

        // Mönstervarning
        string[] commonPatterns = { "password", "123456", "qwerty", "abc", "admin", "welcome", "login" };
        bool hasCommonPattern = commonPatterns.Any(p => password.ToLower().Contains(p));

        string strengthLabel;
        if (strengthScore <= 2)      strengthLabel = "⚠️  Svagt";
        else if (strengthScore <= 3) strengthLabel = "🟡 Medel";
        else                         strengthLabel = "✅ Starkt";

        Console.WriteLine("\n--- Resultat ---");
        Console.WriteLine($"Längd                  : {length}");
        Console.WriteLine($"Teckentyper            : {(hasLower ? "a-z " : "")}{(hasUpper ? "A-Z " : "")}{(hasDigit ? "0-9 " : "")}{(hasSpecial ? "special " : "")}");
        Console.WriteLine($"Uppskattad teckenmängd : {charsetSize}");
        Console.WriteLine($"SHA-256 hash           : {hashHex}");
        Console.WriteLine($"Möjliga kombinationer  : {ToScientific(combinations)}");
        Console.WriteLine($"Tid (1 mdr/sek)        : {timeModel}");
        Console.WriteLine($"Tid (RTX 4090, 30 mdr) : {timeRTX}");
        Console.WriteLine($"Lösenordsstyrka        : {strengthLabel}");

        if (hasCommonPattern)
            Console.WriteLine("\n⚠️  Varning: Lösenordet innehåller ett vanligt mönster och är lättare att gissa.");
        VisaSaltDemo(password);
        VisaHashJamforelse(password);
        Console.WriteLine("\n--- Genererat säkert lösenord (16 tecken) ---");
        Console.WriteLine(GenereraLosenord(16));


        Console.WriteLine("\nTryck valfri tangent för att avsluta...");
        Console.ReadKey();
    }

    static string ComputeSha256Hex(string input)
    {
        using var sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(hash);
    }

    static string FormatSeconds(BigInteger seconds)
    {
        BigInteger minute = 60;
        BigInteger hour   = 60 * minute;
        BigInteger day    = 24 * hour;
        BigInteger year   = 365 * day;

        if (seconds < minute) return $"{seconds} sek";
        if (seconds < hour)   return $"{seconds / minute} min";
        if (seconds < day)    return $"{seconds / hour} h";
        if (seconds < year)   return $"{seconds / day} dagar";
        return $"{seconds / year} år";
    }

    static string ToScientific(BigInteger n)
    {
        string s = n.ToString();
        if (s.Length <= 6) return s;
        return $"{s[0]}.{s.Substring(1, 2)}e+{s.Length - 1}";
    }
    static void VisaSaltDemo(string password)
{
    Console.WriteLine("\n--- Salt-demonstration ---");
    Console.WriteLine("Samma lösenord, tre olika salts → tre helt olika hashar:\n");

    var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
    for (int i = 1; i <= 3; i++)
    {
        byte[] saltBytes = new byte[16];
        rng.GetBytes(saltBytes);
        string salt = Convert.ToHexString(saltBytes);
        string salted = password + salt;
        string hash = ComputeSha256Hex(salted);
        Console.WriteLine($"Salt {i}: {salt}");
        Console.WriteLine($"Hash {i}: {hash}\n");
    }
}
static void VisaHashJamforelse(string password)
{
    Console.WriteLine("\n--- Hashalgoritmer jämförelse ---");

    using var md5    = System.Security.Cryptography.MD5.Create();
    using var sha1   = System.Security.Cryptography.SHA1.Create();
    using var sha256 = System.Security.Cryptography.SHA256.Create();

    byte[] bytes = Encoding.UTF8.GetBytes(password);

    Console.WriteLine($"MD5    (osäker): {Convert.ToHexString(md5.ComputeHash(bytes))}");
    Console.WriteLine($"SHA-1  (svag):   {Convert.ToHexString(sha1.ComputeHash(bytes))}");
    Console.WriteLine($"SHA-256 (bra):   {Convert.ToHexString(sha256.ComputeHash(bytes))}");
    Console.WriteLine("\n⚠️  MD5 och SHA-1 rekommenderas inte för lösenordslagring.");
}
static string GenereraLosenord(int langd = 16)
{
    const string tecken = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%&*?";
    var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
    var sb = new System.Text.StringBuilder();
    byte[] buf = new byte[1];
    while (sb.Length < langd)
    {
        rng.GetBytes(buf);
        if (buf[0] < tecken.Length)
            sb.Append(tecken[buf[0]]);
    }
    return sb.ToString();
}
}