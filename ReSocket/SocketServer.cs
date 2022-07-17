// ReSocket by Tidominer
// https://github.com/Tidominer/ReSocket/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ReSocket
{
    public class SocketServer
    {
        // ReSharper disable MemberCanBePrivate.Global UnusedMember.Global
        public readonly IPAddress IpAddress;
        public readonly int Port;
        public readonly Socket Listener;
        public readonly List<SocketClient> Clients;
        public int ReceiveBufferSize = 1024;
        
        private Thread _serverThread;
        private readonly int _listen;
        
        public Action<SocketClient> OnClientConnect;
        public bool Connected;

        public SocketServer(string ipAddress,int port,int listen = 10)
        {
            Connected = false;
            IpAddress = IPAddress.Parse(ipAddress);
            Port = port;
            _listen = listen;
            Clients = new List<SocketClient>();
            var ipEndPoint = new IPEndPoint(IpAddress,Port);
            Listener = new Socket(ipEndPoint.AddressFamily,SocketType.Stream,ProtocolType.Tcp);
            Listener.Bind(ipEndPoint);
        }

        public void Start()
        {
            if (!Connected)
            {
                _serverThread = new Thread(StartServer);
                _serverThread.Start();
                Connected = true;
            }
            else
            {
                Connected = false;
                throw new Exception("Server is already running");
            }
        }
        
        private async void StartServer()
        {
            Listener.Listen(_listen);
            while (Connected)
            {
                TaskCompletionSource<bool> acceptTask = new TaskCompletionSource<bool>();
                Listener.BeginAccept(ar =>
                {
                    Socket clientSocket = Listener.EndAccept(ar);
                    acceptTask.SetResult(true);
                    SocketClient client = new SocketClient(clientSocket, this);
                    Clients.Add(client);
                    OnClientConnect?.Invoke(client);
                    client.StartListening();
                }, new object());
                await acceptTask.Task;
            }
        }

        internal void ClientDisconnected(SocketClient client)
        {
            Clients.Remove(client);
        }

        public void SendToAll(string @event,string text,SocketClient exception = null)
        {
            foreach (var client in Clients)
            {
                if (exception==null||client!=exception)
                    client.Send(@event,text);
            }
        }

        public void Shutdown()
        {
            Listener.Shutdown(SocketShutdown.Both);
            Listener.Disconnect(false);
            Connected = false;
        }
    }
}