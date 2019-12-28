namespace Stomp
{
    sealed class Commands
    {
        //Client Commands
        internal class Client
        {
            public const string CONNECT = "CONNECT";
            public const string DISCONNECT = "DISCONNECT";
            public const string SUBSCRIBE = "SUBSCRIBE";
            public const string UNSUBSCRIBE = "UNSUBSCRIBE";
            public const string SEND = "SEND";
            public const string ACK = "ACK";
        }
        //Server Commands
        internal class Server
        {
            public const string CONNECTED = "CONNECTED";
            public const string MESSAGE = "MESSAGE";
            public const string ERROR = "ERROR";
            public const string RECEIPT = "RECEIPT";
        }

        internal class HttpHeaders
        {
            public const string AUTHORIZATION = "Authorization";
            public const string ACCEPTVERSION = "accept-version";
            public const string HEARTBEAT = "heart-beat";
            public const string HOST = "host";
            public const string ID = "id";
            public const string MESSAGE_ID = "message-id";
            public const string CONTENTTYPE = "content-type";
            public const string DESTINATION = "destination";
            public const string HEARTBEATVALUE = "10000,10000";
            public const string VERSION = "1.2";
            public const string ACK = "ack";
            public const string RECEIPT = "receipt";
        }

        
    }
}
