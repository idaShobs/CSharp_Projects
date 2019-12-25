using System;
using System.Collections.Generic;
using System.Text;

namespace Stomp
{
    public interface IStomp
    {
        void AbortConnection();
        void SendMessage(string msg);
        void Subscribe(string topic);
        void ListenForMessages();
        void Connect(string url, int timeout_ms);
        bool IsConnected { get; }
        EventHandler<string> OnMessageReceived { get; }
        EventHandler OnOpen { get; }

        EventHandler<string> OnErrorMessageRecieved { get; }
    }
}
