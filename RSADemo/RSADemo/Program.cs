using System;

namespace RSADemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                KeyModel encryptDecrypt = RSAEncrypt.GetKeyPair();

                string rsaEn1 = RSAEncrypt.Encrypt("I hate you", encryptDecrypt.PrivateKey);
                string rsaDe1 = RSAEncrypt.Decrypt(rsaEn1, encryptDecrypt.PrivateKey);
                Console.WriteLine("私钥加密解密：");
                Console.WriteLine("加密后：{0}", rsaEn1);
                Console.WriteLine("解密后: {0}", rsaDe1);
                Console.WriteLine("\n");

                string rsaEn2 = RSAEncrypt.Encrypt("dot net", encryptDecrypt.PublicKey);
                string rsaDe2 = RSAEncrypt.Decrypt(rsaEn2, encryptDecrypt.PrivateKey);
                Console.WriteLine("公钥加密，私钥解密：");
                Console.WriteLine("加密后：{0}", rsaEn2);
                Console.WriteLine("解密后: {0}", rsaDe2);
                Console.WriteLine("\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
