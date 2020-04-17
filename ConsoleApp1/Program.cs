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
            var encryptedExpectedResult =
                "8f8a3752ee05d4465f1747918016fe2dd28fc487652efc2d32a8bce90cf8f4e53de87eccd6c0386ec18811ca8cbf37927a47b5266fb16373194be9a9b105f809";
            var finalExpectedResult =
                "OGY4YTM3NTJlZTA1ZDQ0NjVmMTc0NzkxODAxNmZlMmRkMjhmYzQ4NzY1MmVmYzJkMzJhOGJjZTkwY2Y4ZjRlNTNkZTg3ZWNjZDZjMDM4NmVjMTg4MTFjYThjYmYzNzkyN2E0N2I1MjY2ZmIxNjM3MzE5NGJlOWE5YjEwNWY4MDk=";

            Console.WriteLine($"input: {valueToEncrypt}");


            var encryptedResult = SHA512_ComputeHash(valueToEncrypt, secretKey);
            Console.WriteLine($"encrypted string: {encryptedResult}");
            Console.WriteLine($"expected encrypted Result: {encryptedExpectedResult}");
            Console.WriteLine($"Is Match: {encryptedResult == encryptedExpectedResult}");


            Console.WriteLine($"=============================================================================");

            // var actualResult = Base64Encode(encryptedExpectedResult);
            var actualResult = Base64Encode(encryptedResult);

            Console.WriteLine($"encoded string: {actualResult}");
            Console.WriteLine($"expected Result: {finalExpectedResult}");
            Console.WriteLine($"Is Match: {actualResult == finalExpectedResult}");
            Console.ReadLine();
        }

        public static string SHA512_ComputeHash(string text, string secretKey)
        {
            var hash = new StringBuilder();
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] inputBytes = Encoding.UTF8.GetBytes(text);
            using (var hmac = new HMACSHA512(secretKeyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

    }
}
