// ReSocket by Tidominer
// https://github.com/Tidominer/ReSocket/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReSocket
{
    public class SocketClient
    {
        // ReSharper disable MemberCanBePrivate.Global UnusedAutoPropertyAccessor.Global UnusedMember.Global
        public Socket Socket { get; }
        public SocketServer Server { get; }
        public readonly string Ip;
        public readonly int Port;
        private int ReceiveBufferSize => Server.ReceiveBufferSize;
        public bool Connected => Socket.Connected;
        public bool Listening { get; private set; }
        public Dictionary<string, Action<string>> Events { get; }
        private Thread Thread { get; }

        public Action OnDisconnect;

        private readonly byte[] _receiveBuffer;
        
        private string _receivedDataQuery;

        public bool Disconnected { get; private set; }

        public SocketClient(Socket client, SocketServer server)
        {
            Disconnected = false;
            Socket = client;
            Server = server;
            Events = new Dictionary<string, Action<string>>();
            _receiveBuffer = new byte[ReceiveBufferSize];
            _receivedDataQuery = "";
            var split = ((IPEndPoint) client.RemoteEndPoint).ToString().Split(':');
            Ip = split[0];
            Port = int.Parse(split[1]);
            Thread = new Thread(ReceiveDataLoop);
            Thread.Start();
        }

        public void StartListening()
        {
            Listening = true;
        }

        public void StopListening()
        {
            Listening = false;
        }

        public void Send(string sEvent, string sMessage = "")
        {
            var text = sEvent + "<?:>" + sMessage + "<?;>";
            var bytes = Encoding.UTF8.GetBytes(text);
            Socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, (ar) => { Socket.EndSend(ar); }, new object());
        }

        public void On(string rEvent, Action<string> rAction)
        {
            Events.Add(rEvent, rAction);
        }

        private async void ReceiveDataLoop()
        {
            while (!Disconnected && Connected)
            {
                if (Listening)
                {
                    try
                    {
                        TaskCompletionSource<bool> receiveTask = new TaskCompletionSource<bool>();
                        ReceiveData(receiveTask);
                        await receiveTask.Task;
                    }
                    catch
                    {
                        break;
                    }
                }
            }
        }

        private void ReceiveData(TaskCompletionSource<bool> tcs)
        {
            Socket.BeginReceive(_receiveBuffer, 0, ReceiveBufferSize, SocketFlags.None, ar =>
            {
                try
                {
                    int receivedBytes = Socket.EndReceive(ar);
                    tcs.SetResult(true);
                    if (receivedBytes > 0)
                    {
                        var receivedData = Encoding.UTF8.GetString(_receiveBuffer.Take(receivedBytes).ToArray());
                        _receivedDataQuery += receivedData;
                        var index = _receivedDataQuery.IndexOf("<?;>", StringComparison.Ordinal);
                        while (index > -1)
                        {
                            var request = _receivedDataQuery.Substring(0, index)
                                .Split(new[] {"<?:>"}, StringSplitOptions.None);
                            if (request.Length > 1)
                                try
                                {
                                    Events[request[0]].Invoke(request[1]);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }

                            _receivedDataQuery =
                                _receivedDataQuery.Substring(index + 4, _receivedDataQuery.Length - (index + 4));
                            index = _receivedDataQuery.IndexOf("<?;>", StringComparison.Ordinal);
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
                Listening = false;
                Disconnected = true;
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
                OnDisconnect?.Invoke();
                Server.ClientDisconnected(this);
                Socket.Dispose();
            }
        }
    }
}