// SocketSharp by Tidominer
// https://github.com/Tidominer/SocketSharp/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketNetworkingClient
{
    public class SocketConnection
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Global MemberCanBePrivate.Global
        public Socket Socket { get; }
        public IPAddress IpAddress  { get; }
        public int Port  { get; }
        public IPEndPoint EndPoint  { get; }
        public bool Connected => Socket.Connected;
        public bool Disconnected { get; private set; }
        
        public Dictionary<string, Action<string>> Events;
        public Action OnDisconnect;

        public SocketConnection(string ipAddress, int port)
        {
            Disconnected = false;
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

        public void On(string rEvent, Action<string> rAction)
        {
            Events.Add(rEvent, rAction);
        }
        
        public void Send(string sEvent,string sMessage = "")
        {
            var text = sEvent + "<?:>" + sMessage + "<?;>";
            var bytes = Encoding.UTF8.GetBytes(text);
            try {Socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, ar => { Socket.EndSend(ar); }, new object());}catch{/*ignored*/}
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

        private readonly byte[] _receiveBytes = new byte[1024];
        private void ReceiveData(TaskCompletionSource<bool> tcs)
        {
            Socket.ReceiveTimeout = 1;
            Socket.BeginReceive(_receiveBytes, 0, _receiveBytes.Length, SocketFlags.None, (ar) =>
            {
                try
                {
                    int receivedBytes = Socket.EndReceive(ar);
                    tcs.SetResult(true);
                    if (receivedBytes > 0)
                    {
                        var data = Encoding.UTF8.GetString(_receiveBytes.Take(receivedBytes).ToArray());
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