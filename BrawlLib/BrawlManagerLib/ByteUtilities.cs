using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BrawlLib.BrawlManagerLib
{
    public static class ByteUtilities
    {
        public static byte[] StringToByteArray(string s)
        {
            char[] numbers = (from c in s
                              where char.IsLetterOrDigit(c)
                              select c).ToArray();
            byte[] b = new byte[numbers.Length / 2];
            for (int i = 0; i < b.Length; i++)
            {
                string num = numbers[2 * i] + "" + numbers[2 * i + 1];
                b[i] = Convert.ToByte(num, 16);
            }

            return b;
        }

        public static bool ByteArrayEquals(byte[] b1, int offset1, byte[] b2, int offset2, int length)
        {
            for (int subindex = 0; subindex < length; subindex++)
            {
                if (b1[subindex + offset1] != b2[subindex + offset2])
                {
                    return false;
                }
            }

            return true;
        }

        private static MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();

        /// <summary>
        /// Calculates the MD5 sum of a file.
        /// </summary>
        public static string MD5Sum(string path)
        {
            if (File.Exists(path))
            {
                return MD5Sum(File.ReadAllBytes(path));
            }

            return "File not found";
        }

        /// <summary>
        /// Calculates the MD5 sum of the data in a byte array.
        /// </summary>
        public static string MD5Sum(byte[] data)
        {
            byte[] hash = md5provider.ComputeHash(data);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2").ToLower());
            }

            return sb.ToString();
        }
    }
}