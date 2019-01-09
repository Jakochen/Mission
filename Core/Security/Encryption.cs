using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    public static class Encryption
    {
        /// <summary>
        /// SHA256
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetSwcSH1(string value)
        {
            //SHA1 algorithm = SHA1.Create();
            //byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            //string sh1 = "";
            //for (int i = 0; i < data.Length; i++)
            //{
            //    sh1 += data[i].ToString("x2").ToUpperInvariant();
            //}
            //return sh1;
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] source = Encoding.Default.GetBytes(value);
            byte[] crypto = sha256.ComputeHash(source);
            string result = Convert.ToBase64String(crypto);
            return result;
        }
    }
}
