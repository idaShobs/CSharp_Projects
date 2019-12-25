using System;
using System.Collections.Generic;
using System.Text;

namespace Stomp
{
    class StompClientMessages
    {
        public string Connection_Message(string authorization = "",
            string version = Commands.HttpHeaders.VERSION,
            string Heartbeat = Commands.HttpHeaders.HEARTBEATVALUE)
        {
            MessageSerializer msgSerializer = new MessageSerializer();
            var connMsg = new Message(Commands.CONNECT);
            connMsg[Commands.HttpHeaders.AUTHORIZATION] = authorization;
            connMsg[Commands.HttpHeaders.ACCEPTVERSION] = version;
            connMsg[Commands.HttpHeaders.HEARTBEAT] = Heartbeat;
            return msgSerializer.Serialize(connMsg); 
        }

        public string Subscribe_Message(int id, string destination, string authorization = "")
        {
            MessageSerializer msgSerializer = new MessageSerializer();
            var subMsg = new Message(Commands.SUBSCRIBE);
            subMsg[Commands.HttpHeaders.AUTHORIZATION] = authorization;
            subMsg[Commands.HttpHeaders.DESTINATION] = destination;
            subMsg[Commands.HttpHeaders.ID] = $"sub-{id}";
            return msgSerializer.Serialize(subMsg);
        }

       
        public string Unsubscribe_Message(int id, string destination, string authorization = "")
        {
            MessageSerializer msgSerializer = new MessageSerializer();
            var unsubMsg = new Message(Commands.UNSUBSCRIBE);
            unsubMsg[Commands.HttpHeaders.AUTHORIZATION] = authorization;
            unsubMsg[Commands.HttpHeaders.DESTINATION] = destination;
            unsubMsg[Commands.HttpHeaders.ID] = $"sub-{id}";
            return msgSerializer.Serialize(unsubMsg);
        }

        public string Send_Message(string destination, string authorization = "") 
        {
            MessageSerializer msgSerializer = new MessageSerializer();
            var msg = new Message(Commands.SEND);
            msg[Commands.HttpHeaders.AUTHORIZATION] = authorization;
            msg[Commands.HttpHeaders.DESTINATION] = destination;
            return msgSerializer.Serialize(msg);
        }
    }
}
