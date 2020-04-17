using System;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var secretKey = "1234";
            var valueToEncrypt = "Hello";
            var hashExpectedResult =
                "8f8a3752ee05d4465f1747918016fe2dd28fc487652efc2d32a8bce90cf8f4e53de87eccd6c0386ec18811ca8cbf37927a47b5266fb16373194be9a9b105f809";
            var expectedSignature =
                "OGY4YTM3NTJlZTA1ZDQ0NjVmMTc0NzkxODAxNmZlMmRkMjhmYzQ4NzY1MmVmYzJkMzJhOGJjZTkwY2Y4ZjRlNTNkZTg3ZWNjZDZjMDM4NmVjMTg4MTFjYThjYmYzNzkyN2E0N2I1MjY2ZmIxNjM3MzE5NGJlOWE5YjEwNWY4MDk=";

            Console.WriteLine($"input: {valueToEncrypt}");


            var hashResult = SHA512_ComputeHash(valueToEncrypt, secretKey);
            Console.WriteLine($"actual hash: {hashResult}");
            Console.WriteLine($"expected hash Result: {hashExpectedResult}");
            Console.WriteLine($"Is Match: {hashResult == hashExpectedResult}");


            Console.WriteLine($"=============================================================================");

            // var actualResult = Base64Encode(hashExpectedResult);
            var actualResult = Base64Encode(hashResult);

            Console.WriteLine($"actual Signature: {actualResult}");
            Console.WriteLine($"expected Signature Result: {expectedSignature}");
            Console.WriteLine($"Is Match: {actualResult == expectedSignature}");
            Console.ReadLine();
        }

        public static string SHA512_ComputeHash(string text, string secretKey)
        {
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] inputBytes = Encoding.UTF8.GetBytes(text);

            using var hmac = new HMACSHA512(secretKeyBytes);
            byte[] hashValue = hmac.ComputeHash(inputBytes);
            return ToHexString(hashValue);
        }

        public static string ToHexString(byte[] bytes)
        {
            var hash = new StringBuilder();
            foreach (var theByte in bytes) hash.Append(theByte.ToString("x2"));
            return hash.ToString();
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

    }
}
