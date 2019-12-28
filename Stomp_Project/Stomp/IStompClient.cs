using System;

namespace Stomp
{
    /*
     * This API defines a client interface that implements the Stomp Protocol 1.2 following the guidelines as defined by the protocol
     * STOMP is a simple interoperable protocol designed for asynchronous message passing between clients via mediating servers. 
     * It defines a text based wire-format for messages passed between these clients and servers.
     * source: https://stomp.github.io/stomp-specification-1.2.html#BEGIN
     */

    public interface IStompClient
    {
        void SendMessage(string msg, int timeout_ms, string authorization = "", string destination = "");

        void Subscribe(string sub_id, string destination, int timeout_ms, string authorization = "");
        void Unsubscribe(string sub_id, string destination, int timeout_ms, string authorization = "");
        void Disconnect(string authorization);
        void Connect(Uri uri, int timeout_ms, string authorization = "",
            string version = Commands.HttpHeaders.VERSION,
            string Heartbeat = Commands.HttpHeaders.HEARTBEATVALUE);
        bool IsConnected { get; }
        EventHandler<Frame> OnMessageReceived { get; }
        EventHandler OnOpen { get; }
        EventHandler<Frame> OnConnected { get; }
        EventHandler<Frame> OnErrorMessageRecieved { get; }
    }
}
