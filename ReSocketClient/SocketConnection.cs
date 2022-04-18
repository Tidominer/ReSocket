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
        public readonly IPEndPoint EndPoint;
        public readonly Dictionary<string, Action<string>> Events;
        public bool Connected => Socket.Connected;
        public bool Disconnected { get; private set; }

        public int ReceiveBufferSize = 1024;

        public Action OnDisconnect;
        
        private readonly byte[] _receiveBuffer;

        public SocketConnection(string ipAddress, int port)
        {
            Disconnected = false;
            _receiveBuffer = new byte[ReceiveBufferSize];
            IpAddress = IPAddress.Parse(ipAddress);
            Port = port;
            EndPoint = new IPEndPoint(IpAddress, Port);
            Events = new Dictionary<string, Action<string>>();
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            if (!Connected)
            {
                Socket.Connect(EndPoint);
                ConnectionLoop();
            }
        }
        
        public void Send(string sEvent,string sMessage = "")
        {
            var text = sEvent + "<?:>" + sMessage + "<?;>";
            var bytes = Encoding.UTF8.GetBytes(text);
            try {Socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, ar => { Socket.EndSend(ar); }, new object());}catch{/*ignored*/}
        }

        public void On(string rEvent, Action<string> rAction)
        {
            Events.Add(rEvent, rAction);
        }

        private async void ConnectionLoop()
        {
            while (Connected && !Disconnected)
            {
                try
                {
                    var tcs = new TaskCompletionSource<bool>(false);
                    ReceiveData(tcs);
                    await tcs.Task;
                }
                catch{ Disconnect();}
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
                        var data = Encoding.UTF8.GetString(_receiveBuffer.Take(receivedBytes).ToArray());
                        var requests = data.Split(new[] {"<?;>"}, StringSplitOptions.None);
                        foreach (var request in requests)
                        {
                            var split = request.Split(new[] {"<?:>"}, StringSplitOptions.None);
                            if (split.Length > 1)
                                Events[split[0]].Invoke(split[1]);
                        }
                    }
                    else
                    {
                        Disconnect();
                    }
                }
                catch
                {
                    Disconnect();
                }
            }, new object());
        }

        public void Disconnect()
        {
            if (!Disconnected)
            {
                Disconnected = true;
                Socket.Close();
                Console.WriteLine("Disconnected From Server");
                OnDisconnect?.Invoke();
            }
        }
    }
}