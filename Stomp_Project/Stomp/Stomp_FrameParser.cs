using System;

namespace Stomp
{
    class Stomp_FrameParser
    {
        public string Connection_Frame(string authorization, string version, string Heartbeat)
        {
            FrameSerializer frameSerializer = new FrameSerializer();
            var connframe = new Frame(Commands.Client.CONNECT);
            connframe[Commands.HttpHeaders.AUTHORIZATION] = authorization;
            connframe[Commands.HttpHeaders.ACCEPTVERSION] = version;
            connframe[Commands.HttpHeaders.HEARTBEAT] = Heartbeat;
            return frameSerializer.Serialize(connframe); 
        }

        public string Subscribe_Frame(string id, string destination, string authorization, AckMode ackMode)
        {
            FrameSerializer frameSerializer = new FrameSerializer();
            var subframe = new Frame(Commands.Client.SUBSCRIBE);
            subframe[Commands.HttpHeaders.AUTHORIZATION] = authorization;
            subframe[Commands.HttpHeaders.DESTINATION] = destination;
            subframe[Commands.HttpHeaders.ID] = id;
            subframe[Commands.Client.ACK] = Enum.GetName(typeof(AckMode), ackMode);
            return frameSerializer.Serialize(subframe);
        }

       
        public string Unsubscribe_Frame(string id, string destination, string authorization)
        {
            FrameSerializer frameSerializer = new FrameSerializer();
            var unsubframe = new Frame(Commands.Client.UNSUBSCRIBE);
            unsubframe[Commands.HttpHeaders.AUTHORIZATION] = authorization;
            unsubframe[Commands.HttpHeaders.DESTINATION] = destination;
            unsubframe[Commands.HttpHeaders.ID] = id;
            return frameSerializer.Serialize(unsubframe);
        }
        public string Disconnect_Frame(string receipt_id, string authorization)
        {
            FrameSerializer frameSerializer = new FrameSerializer();
            var unsubframe = new Frame(Commands.Client.DISCONNECT);
            unsubframe[Commands.HttpHeaders.AUTHORIZATION] = authorization;
            unsubframe[Commands.Server.RECEIPT] = receipt_id;
            return frameSerializer.Serialize(unsubframe);
        }

        public string Send_Frame(string destination, string authorization, string message) 
        {
            FrameSerializer frameSerializer = new FrameSerializer();
            var frame = new Frame(Commands.Client.SEND, message);
            frame[Commands.HttpHeaders.AUTHORIZATION] = authorization;
            frame[Commands.HttpHeaders.DESTINATION] = destination;
            return frameSerializer.Serialize(frame);
        }

        public string Ack_Frame(string id)
        {
            FrameSerializer frameSerializer = new FrameSerializer();
            var frame = new Frame(Commands.Client.ACK);
            frame[Commands.HttpHeaders.ID] = id;
            return frameSerializer.Serialize(frame);
        }
    }
}
