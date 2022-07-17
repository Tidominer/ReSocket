// ReSocket by Tidominer
// https://github.com/Tidominer/ReSocket/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ReSocketClient
{
    public class SocketConnection
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Global MemberCanBePrivate.Global
        public readonly Socket Socket;
        public readonly IPAddress IpAddress;
        public readonly int Port;
        public readonly Dictionary<string, Action<string>> Events;
        public bool Connected { get; private set; }
        public Action OnDisconnect;
        private int ReceiveBufferSize = 1024;
        private readonly IPEndPoint _endPoint;
        private readonly byte[] _receiveBuffer;
        private string _receivedData = "";

        public SocketConnection(string ipAddress, int port)
        {
            Connected = false;
            _receiveBuffer = new byte[ReceiveBufferSize];
            IpAddress = IPAddress.Parse(ipAddress);
            Port = port;
            _endPoint = new IPEndPoint(IpAddress, Port);
            Events = new Dictionary<string, Action<string>>();
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            if (!Connected)
            {
                Socket.Connect(_endPoint);
                Connected = true;
                ConnectionLoop();
            }
            else
            {
                throw new Exception("Socket is currently connected to server");
            }
        }
        
        public void Send(string @event,string text = "")
        {
            var message = "<:>" + FilterMessage(@event) + "<?>" + FilterMessage(text) + "<;>";
            var bytes = Encoding.UTF8.GetBytes(message);
            try {Socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, ar => { Socket.EndSend(ar); }, new object());}catch(Exception e){ Disconnect(e);}
        }
        
        private string FilterMessage(string message) => message
            .Replace(@"\",@"\\")
            .Replace("<",@"\<")
            .Replace(">",@"\>");
        
        private string UnFilterMessage(string message) => message
            .Replace(@"\<","<")
            .Replace(@"\>",">")
            .Replace(@"\\",@"\");

        public void On(string @event, Action<string> action)
        {
            Events.Add(@event, action);
        }

        private async void ConnectionLoop()
        {
            while (Connected)
            {
                try
                {
                    var tcs = new TaskCompletionSource<bool>(false);
                    ReceiveData(tcs);
                    await tcs.Task;
                }
                catch(Exception e){ Disconnect(e);}
            }
        }

        private void ReceiveData(TaskCompletionSource<bool> tcs)
        {
            Socket.ReceiveTimeout = 1;
            Socket.BeginReceive(_receiveBuffer, 0, ReceiveBufferSize, SocketFlags.None, (ar) =>
            {
                try
                {
                    int receivedBytes = Socket.EndReceive(ar);
                    tcs.SetResult(true);
                    if (receivedBytes > 0)
                    {
                        var receivedText = Encoding.UTF8.GetString(_receiveBuffer.Take(receivedBytes).ToArray());
                        _receivedData += receivedText;
                        var endIndex = _receivedData.IndexOf("<;>", StringComparison.Ordinal);
                        while (endIndex > -1)
                        {
                            var startIndex = _receivedData.IndexOf("<:>", StringComparison.Ordinal);
                            if (startIndex < endIndex)
                            {
                                var message = _receivedData.Substring(startIndex + 3, endIndex - startIndex - 3)
                                    .Split(new []{"<?>"},StringSplitOptions.None);
                                _receivedData = _receivedData.Remove(startIndex, endIndex - startIndex + 3);
                                try
                                {
                                    Events[UnFilterMessage(message[0])].Invoke(UnFilterMessage(message[1]));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                            endIndex = _receivedData.IndexOf("<;>", StringComparison.Ordinal);
                        }
                    }
                    else
                        Disconnect();
                }
                catch (Exception e)
                {
                    Disconnect(e);
                }
            }, new object());
        }

        public void Disconnect(Exception exception = null) //Exception for debug purposes
        {
            if (Connected)
            {
                Connected = false;
                Socket.Close();
                OnDisconnect?.Invoke();
            }
            //Console.WriteLine(exception?.Message);
            //Console.WriteLine(exception?.StackTrace);
        }
    }

    public struct Message
    {
        public Message(string @event, string text)
        {
            Event = @event;
            Text = text;
        }
        public string Event;
        public string Text;
    }
}