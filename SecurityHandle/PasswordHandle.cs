using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SecurityHandle
{
    public class PasswordHandle
    {
        private static PasswordHandle _instance;
        public static PasswordHandle GetInstance()
        {
            return _instance ?? (_instance = new PasswordHandle());
        }

        public string GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        

        public string EncryptPassword(string originalPassword, string salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: originalPassword,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            //Console.WriteLine($"Hashed: {hashed}");
            return hashed;
        }
    }
}