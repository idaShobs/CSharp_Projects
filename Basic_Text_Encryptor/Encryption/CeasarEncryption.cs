using System;
using System.Collections.Generic;
using System.Text;

namespace Basic_Text_Encryptor.Program
{
    class CeasarEncryption : IEncryptor
    {
        private readonly int _key;
        public CeasarEncryption(int key)
        {
            if(key <= 0 || key > 25)
            {
                throw new ArgumentOutOfRangeException("Key", "Key for ceaser encryption must be between 1 and 25");
            }
            this._key = key;
        }
        public string Decrypt(string text)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string text)
        {
            throw new NotImplementedException();
        }
    }
}
