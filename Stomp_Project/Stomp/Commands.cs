using System;
using System.Collections.Generic;
using System.Text;

namespace Stomp
{
    sealed class Commands
    {
        //Client Commands
        public const string CONNECT = "CONNECT";
        public const string DISCONNECT = "DISCONNECT";
        public const string SUBSCRIBE = "SUBSCRIBE";
        public const string UNSUBSCRIBE = "UNSUBSCRIBE";
        public const string SEND = "SEND";
        //Server Response
        public const string CONNECTED = "CONNECTED";
        public const string MESSAGE = "MESSAGE";
        public const string ERROR = "ERROR";

        internal class HttpHeaders
        {
            public const string AUTHORIZATION = "X-Authorization";
            public const string ACCEPTVERSION = "accept-version";
            public const string HEARTBEAT = "heart-beat";
            public const string HOST = "host";
            public const string ID = "id";
            public const string CONTENTTYPE = "content-type";
            public const string DESTINATION = "destination";
            public const string HEARTBEATVALUE = "10000,10000";
            public const string VERSION = "1.2";
        }
    }
}
