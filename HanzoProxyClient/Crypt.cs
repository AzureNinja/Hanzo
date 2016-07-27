using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanzoProxyClient
{
    class Crypt
    {
        public static string EncryptString(string sourceString, string password)
        {
            System.Security.Cryptography.RijndaelManaged rijndael =
                new System.Security.Cryptography.RijndaelManaged();
            byte[] key, iv;
            GenerateKeyFromPassword(
                password, rijndael.KeySize, out key, rijndael.BlockSize, out iv);
            rijndael.Key = key;
            rijndael.IV = iv;
            byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(sourceString);
            System.Security.Cryptography.ICryptoTransform encryptor =
                rijndael.CreateEncryptor();
            byte[] encBytes = encryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
            encryptor.Dispose();
            return System.Convert.ToBase64String(encBytes);
        }

        public static string DecryptString(string sourceString, string password)
        {
            System.Security.Cryptography.RijndaelManaged rijndael =
                new System.Security.Cryptography.RijndaelManaged();
            byte[] key, iv;
            GenerateKeyFromPassword(
                password, rijndael.KeySize, out key, rijndael.BlockSize, out iv);
            rijndael.Key = key;
            rijndael.IV = iv;
            byte[] strBytes = System.Convert.FromBase64String(sourceString);
            System.Security.Cryptography.ICryptoTransform decryptor =
                rijndael.CreateDecryptor();
            byte[] decBytes = decryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
            decryptor.Dispose();
            return System.Text.Encoding.UTF8.GetString(decBytes);
        }

        private static void GenerateKeyFromPassword(string password,
            int keySize, out byte[] key, int blockSize, out byte[] iv)
        {
            byte[] salt = System.Text.Encoding.UTF8.GetBytes("salthastobemorethanandequalto8bytes");
            System.Security.Cryptography.Rfc2898DeriveBytes deriveBytes =
                new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt);
            deriveBytes.IterationCount = 1000;
            key = deriveBytes.GetBytes(keySize / 8);
            iv = deriveBytes.GetBytes(blockSize / 8);
        }
    }
}
