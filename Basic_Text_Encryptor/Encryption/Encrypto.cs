using System;
using System.Collections.Generic;
using System.Text;

namespace Basic_Text_Encryptor.Program
{
    class Encrypto : IEncryptor
    {
       
        private List<KeyValuePair<string, char>> _encodingFormat;
        private readonly int _key;
        public Encrypto(int key)
        {
            if(key < 1)
            {
                throw new ArgumentOutOfRangeException("Key", "Key for algorithm must be greater than zero");
            }
            this._key = key;
            Init();
        }
        public string Decrypt(string text)
        {
            string result = "";
            if (!string.IsNullOrEmpty(text))
            {
                text = text.TrimEnd('\r');
                bool odd = text.Length % 2 > 0;
                //Step 1: Swap
                char[] data = text.ToCharArray();
                int divCount = data.Length / 2;
                if (odd)
                {
                    divCount = (data.Length - 1) / 2;
                }
                Swap(data, divCount);

                //Step 2: Reverse
                Array.Reverse(data, 0, divCount);
                if (odd)
                {
                    Array.Reverse(data, divCount + 1, divCount);
                }
                else
                {
                    Array.Reverse(data, divCount, divCount);
                }


                //Step 3: Convert from ASCII to string with the format defined
                string tempResult = From_EncryptedFormat(new string(data));
                //Get length of original data
                if (tempResult.IndexOf('|') >= 0)
                {
                    var sub = tempResult.Substring(0, tempResult.IndexOf('|') + 1);
                    tempResult = tempResult.Substring(sub.Length, (tempResult.Length - sub.Length));
                    if (sub != null)
                    {
                        sub = sub.TrimEnd('|');
                        int length;
                        try
                        {
                            length = Convert.ToInt32(sub);
                            //Check if length is correct
                            if (tempResult.Length == length)
                            {
                                result = tempResult;
                            }
                        }
                        catch { }
                    }
                }
            }

            return result;
        }

        public string Encrypt(string text)
        {
            string result = "";
            if (!string.IsNullOrEmpty(text))
            {
                //Add Format: data length,|, text
                text = text.Length.ToString() + '|' + text;
                try
                {
                    //XOR Each character with key and Convert to ASCII HEX that way text length is always even
                    text = To_EncryptedFormat(text.ToCharArray());
                }
                catch (OutOfMemoryException)
                {
                    throw new Exception("Please reduce the amount of data been sent!");
                }
                char[] data = text.ToCharArray();


                int divCount = (data.Length / 2);
                bool odd = (data.Length % 2 > 0);
                //Step 1: Reverse
                if (odd)
                {
                    divCount = (data.Length - 1) / 2;
                    Array.Reverse(data, divCount + 1, divCount);
                }
                else
                {
                    Array.Reverse(data, divCount, divCount);
                }
                Array.Reverse(data, 0, divCount);


                //Step 2: Swap
                Swap(data, divCount);
                result = new string(data);
            }


            return result;
           
        }

        private string To_EncryptedFormat(char[] charValues)
        {
            string hexOutPut = "";
            foreach (char val in charValues)
            {
                int value = Convert.ToInt32(val);
                value = value ^ _key;
                var enc = _encodingFormat.Find(x => x.Key.Equals(String.Format("{0:X2}", value)));
                hexOutPut += enc.Value;

            }
            return hexOutPut;
        }

        private string From_EncryptedFormat(string text)
        {
            StringBuilder stringBuilder = new StringBuilder();
            byte[] asciitext = Encoding.UTF8.GetBytes(text);
            for (int i = 0; i < text.Length; i++)
            {
                var sub = text.Substring(i, 1);
                var dec = _encodingFormat.Find(x => Convert.ToByte(x.Value).Equals(asciitext[i])).Key;
                int currInt = Int32.Parse(dec, System.Globalization.NumberStyles.HexNumber);
                currInt = _key ^ currInt;
                stringBuilder.Append(Convert.ToString(Convert.ToChar(currInt)));

            }
            return stringBuilder.ToString();
        }
        private void Init()
        {
            _encodingFormat = new List<KeyValuePair<string, char>>();
            for (int i = 0; i < 256; i++)
            {
                string key = String.Format("{0:X2}", i);
                char val = Convert.ToChar(i);
                _encodingFormat.Add(new KeyValuePair<string, char>(key, val));
            }
        }
        private void Swap(char[] data, int divCount)
        {
            string temp = new string(data);
            bool odd = (data.Length % 2 > 0);
            int rightStart = divCount;
            if (odd)
            {
                rightStart++;
            }
            char[] sub1 = temp.Substring(0, divCount).ToCharArray();
            char[] sub2 = temp.Substring(rightStart, divCount).ToCharArray();
            for (int i = 0; i < divCount; i++)
            {
                data[i] = sub2[i];
            }
            for (int i = rightStart; i < data.Length; i++)
            {
                data[i] = sub1[i - rightStart];
            }

        }

    }
}
