using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SecurityHandle
{
    public class TokenHandle
    {
        private static TokenHandle _instance;
        public static TokenHandle GetInstance()
        {
            return _instance ?? (_instance = new TokenHandle());
        }

        // generate token for Credential
        public string GenerateToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            token = token.Replace("=", "");
            token = token.Replace("+", "");

            return token;
        }
    }
}
