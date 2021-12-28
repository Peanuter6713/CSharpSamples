using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSADemo
{
    class RSAEncrypt
    {
        public static KeyModel GetKeyPair()
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            // includePrivateParameters:  true: 包含公钥和私钥 false: 只包含公钥
            string publicKey = RSA.ToXmlString(false);  // 生成的是公开的解密Key
            string privateKey = RSA.ToXmlString(true); // 生成的是私有的既可以加密也可以解密的Key

            return new KeyModel(publicKey, privateKey);
        }

        public static string Encrypt(string content, string encryptedKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(encryptedKey);
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = ByteConverter.GetBytes(content);
            byte[] resultBytes = rsa.Encrypt(dataToEncrypt, false);

            return Convert.ToBase64String(resultBytes);
        }

        public static string Decrypt(string content, string decryptKey)
        {
            byte[] dataToDecrypt = Convert.FromBase64String(content);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(decryptKey);
            var resultBytes = rsa.Decrypt(dataToDecrypt, false);
            UnicodeEncoding ByteConverter = new UnicodeEncoding();

            return ByteConverter.GetString(resultBytes);
        }

    }
}
