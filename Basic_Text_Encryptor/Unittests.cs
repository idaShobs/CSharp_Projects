using Basic_Text_Encryptor.Program;
using NUnit.Framework;
using System;

namespace Basic_Text_Encryptor
{
    public class Tests
    {
       [Test]
        public void Encyrpto_output_not_empty()
        {
            IEncryptor encryptor = new Encrypto(100);
            string text = "Encrypto";
            Assert.IsNotEmpty(encryptor.Encrypt(text));
        }
        [Test]
        public void Encyrpto_output_not_equal_input()
        {
            IEncryptor encryptor = new Encrypto(20);
            string text = "Output";
            Assert.AreNotEqual(text, encryptor.Encrypt(text));
        }

        [Test]
        public void Encrypto_text_decyrpted()
        {
            IEncryptor encryptor = new Encrypto(40);
            string text = "Hello World";
            string encrypted = encryptor.Encrypt(text);
            Assert.That(encryptor.Decrypt(encrypted), Is.EqualTo(text));

        }
        [Test]
        public void Encrypto_key_arg_out_of_range_Exception()
        {
            int key = 0;
            IEncryptor encryptor;
            Assert.That(() =>encryptor = new Encrypto(key), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        [Test]
        public void Ceaser_output_not_empty()
        {
            IEncryptor encryptor = new CeasarEncryption(22);
            string text = "Encrypto";
            Assert.IsNotEmpty(encryptor.Encrypt(text));
        }
        [Test]
        public void Ceasar_output_not_equal_input()
        {
            IEncryptor encryptor = new CeasarEncryption(20);
            string text = "Output";
            Assert.AreNotEqual(text, encryptor.Encrypt(text));
        }
        [Test]
        public void Ceasar_key_arg_out_of_range_Exception()
        {
            int key = 0;
            IEncryptor encryptor;
            Assert.That(() => encryptor = new CeasarEncryption(key), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        [Test]
        public void Ceasar_key_decrypted()
        {
            IEncryptor encryptor = new CeasarEncryption(10);
            string text = "Hello World";
            string encrypted = encryptor.Encrypt(text);
            Assert.That(encryptor.Decrypt(encrypted), Is.EqualTo(text));
        }

        
    }
}