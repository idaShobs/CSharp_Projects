﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Stomp
{
    public sealed class Frame
    {
        public Frame(string command) : this(command, string.Empty) { }

        public Frame(string command, string body) : this(command, body, new Dictionary<string, string>()) { }

        internal Frame(string command, string body, Dictionary<string, string> headers)
        {
            Command = command;
            Body = body;
            Headers = headers;
            this["content-length"] = body.Length.ToString();
        }
        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();
        public string Body { get; private set; }
        public string Command { get; private set; }
        public string this[string header]
        {
            get { return Headers.ContainsKey(header) ? Headers[header] : string.Empty; }
            set { Headers[header] = value; }
        }

       
    }
}
