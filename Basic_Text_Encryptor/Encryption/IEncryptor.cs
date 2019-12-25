using System;
using System.Collections.Generic;
using System.Text;

namespace Basic_Text_Encryptor.Program
{
    interface IEncryptor
    {
        string Encrypt(string text);
        string Decrypt(string text);
    }
}
