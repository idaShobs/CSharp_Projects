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
            if(key < 1 || key > 25)
            {
                throw new ArgumentOutOfRangeException("Key", "Key for ceasar encryption must be between 1 and 25");
            }
            this._key = key;
        }
        public string Decrypt(string text)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                if (ch >= 'A' && ch <= 'Z')
                {
                    ch = Shift('A', ch, 26 - _key);
                }
                else if (ch >= 'a' && ch <= 'z')
                {
                    ch = Shift('a', ch, 26 - _key);
                }
                result.Append(ch);
            }
            return result.ToString();
        }

        public string Encrypt(string text)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                if (ch >= 'A' && ch <= 'Z')
                {
                    ch = Shift('A', ch, _key);
                }
                else if (ch >= 'a' && ch <= 'z')
                {
                    ch = Shift('a', ch, _key);
                }
                result.Append(ch);
            }
            return result.ToString();
        }
        private char Shift(char offset, char text, int key) => (char)(offset + (text - offset + key) % 26);
    }
}
