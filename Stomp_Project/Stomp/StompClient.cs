using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace Stomp
{
    public class StompClient : IStompClient
    {
        private ClientWebSocket cs;
        private CancellationTokenSource tokenSource;
        private readonly Stomp_FrameParser stompMessages;
        private readonly AckMode ackMode = AckMode.auto;
        private int disconnectId = 0;
        public StompClient()
        {
            stompMessages = new Stomp_FrameParser();
        }
        public StompClient(AckMode ackMode) : this()
        {
            this.ackMode = ackMode;
        }
       
        private async void ListenForMessages()
        {
            if(cs != null && IsConnected)
            {
                tokenSource = new CancellationTokenSource();
                FrameSerializer frameSerializer = new FrameSerializer();
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
                            Frame frame = frameSerializer.Deserialize(message);
                            switch (frame.Command) {
                                case Commands.Server.ERROR:
                                    OnErrorMessageRecieved?.Invoke(this, frame);
                                    break;
                                case Commands.Server.CONNECTED:
                                    OnConnected?.Invoke(this, frame);
                                    break;
                                case Commands.Server.RECEIPT:
                                    break;
                                case Commands.Server.MESSAGE:
                                    OnMessageReceived?.Invoke(this, frame);
                                    string msg_id = default;
                                    if(ackMode != AckMode.auto && frame.Headers.TryGetValue(Commands.HttpHeaders.MESSAGE_ID, out msg_id))
                                    {
                                        var msg_encoded = Encoding.UTF8.GetBytes(stompMessages.Ack_Frame(msg_id));
                                        cs.SendAsync(new ArraySegment<Byte>(msg_encoded, 0, msg_encoded.Length), WebSocketMessageType.Text, true, CancellationToken.None); 
                                    }
                                    break;
                                default:
                                    break;
                            }
                           

                        }

                    }
                }
            }
            
        }
        
        public async void Connect(Uri uri, int timeout_ms, string authorization = "",
            string version = Commands.HttpHeaders.VERSION,
            string Heartbeat = Commands.HttpHeaders.HEARTBEATVALUE)
        {
            cs = new ClientWebSocket();
            CancellationTokenSource cancellationToken = new CancellationTokenSource(timeout_ms);
            await cs.ConnectAsync(uri, cancellationToken.Token);
            if (cs.State == WebSocketState.Open)
            {
                ListenForMessages();
                var msg_encoded = Encoding.UTF8.GetBytes(stompMessages.Connection_Frame(authorization, version, Heartbeat));
                var buffer = new ArraySegment<Byte>(msg_encoded, 0, msg_encoded.Length);
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(timeout_ms);
                await cs.SendAsync(buffer, WebSocketMessageType.Text, true,
                    cancellationTokenSource.Token);
                OnOpen?.Invoke(this, EventArgs.Empty);
            }
        }

        public async void SendMessage(string msg, int timeout_ms, string authorization = "", string destination = "")
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource(timeout_ms);
            var msg_encoded = Encoding.UTF8.GetBytes(stompMessages.Send_Frame(authorization, destination, msg));
            var buffer = new ArraySegment<Byte>(msg_encoded, 0, msg_encoded.Length);
            await cs.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken.Token);
        }

        public async void Disconnect(string authorization)
        {
            disconnectId++;
            var msg_encoded = Encoding.UTF8.GetBytes(stompMessages.Disconnect_Frame($"{disconnectId}", authorization));
            var buffer = new ArraySegment<Byte>(msg_encoded, 0, msg_encoded.Length);
            await cs.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            tokenSource.Cancel();
            tokenSource.Dispose();
            cs.Abort();
            cs.Dispose();

        }

        public async void Subscribe(string sub_id, string destination, int timeout_ms, string authorization = "")
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource(timeout_ms);
            var msg_encoded = Encoding.UTF8.GetBytes(stompMessages.Subscribe_Frame(sub_id, destination, authorization, ackMode));
            var buffer = new ArraySegment<Byte>(msg_encoded, 0, msg_encoded.Length);
            await cs.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken.Token);
        }

        public async void Unsubscribe(string sub_id, string destination, int timeout_ms, string authorization = "")
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource(timeout_ms);
            var msg_encoded = Encoding.UTF8.GetBytes(stompMessages.Unsubscribe_Frame(sub_id, destination, authorization));
            var buffer = new ArraySegment<Byte>(msg_encoded, 0, msg_encoded.Length);
            await cs.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken.Token);
        }

        public bool IsConnected => cs?.State == WebSocketState.Open;
        public EventHandler<Frame> OnMessageReceived { get; }
        public EventHandler OnOpen { get; }
        public EventHandler<Frame> OnConnected { get; }
        public EventHandler<Frame> OnErrorMessageRecieved { get; }

        
    }
}
