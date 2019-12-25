using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stomp
{
    public class Websocket_Stomp : IStomp
    {
        private ClientWebSocket cs;
        private CancellationTokenSource tokenSource;
        private StompClientMessages stompMessages;
        public Websocket_Stomp()
        {
            cs = new ClientWebSocket();
            stompMessages = new StompClientMessages();
        }

        public async void ListenForMessages()
        {
            tokenSource = new CancellationTokenSource();
            while (!tokenSource.IsCancellationRequested)
            {
                ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[8192]);

                WebSocketReceiveResult result = null;

                using (var ms = new MemoryStream())
                {
                    do
                    {
                        result = await cs.ReceiveAsync(buffer, CancellationToken.None);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    }
                    while (!result.EndOfMessage);

                    ms.Seek(0, SeekOrigin.Begin);

                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        string message = reader.ReadToEnd();
                        if (message.Contains(Commands.ERROR))
                        {
                            OnErrorMessageRecieved?.Invoke(this, message);
                        }
                        else
                        {
                            OnMessageReceived?.Invoke(this, message);
                        }
                        
                    }
                        
                }
            }
            
        }
        
        public async void Connect(string url, int timeout_ms)
        {
            Uri uri = new Uri(url);
            CancellationTokenSource cancellationToken = new CancellationTokenSource(timeout_ms);
            await cs.ConnectAsync(uri, cancellationToken.Token);
            if (cs.State == WebSocketState.Open)
            {
                var encoded = Encoding.UTF8.GetBytes(stompMessages.Connection_Message());
                var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);
                await cs.SendAsync(buffer, WebSocketMessageType.Text, true, 
                    new CancellationTokenSource(timeout_ms).Token);
            }
            if (cs.State == WebSocketState.Open)
            {
                OnOpen?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SendMessage(string msg)
        {
            throw new NotImplementedException();
        }

        public void AbortConnection()
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
            cs.Abort();
        }

        public void Subscribe(string topic)
        {
            throw new NotImplementedException();
        }

        public bool IsConnected => cs.State == WebSocketState.Open;
        public EventHandler<string> OnMessageReceived { get; }
        public EventHandler OnOpen { get; }

        public EventHandler<string> OnErrorMessageRecieved { get; }

        
    }
}
