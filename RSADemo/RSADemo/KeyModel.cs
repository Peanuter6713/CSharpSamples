using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSADemo
{
    class KeyModel
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }

        public KeyModel(string publicKey, string privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
    }
}
