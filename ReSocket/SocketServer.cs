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
        public IPAddress IpAddress { get; }
        public int Port { get; }
        public IPEndPoint IpEndPoint { get; private set; }
        public Socket Listener { get; private set; }
        public List<SocketClient> Clients { get; private set; }
        public int ReceiveBufferSize = 1024;
        private Thread _serverThread;
        private readonly int _listen;
        public Action<SocketClient> OnClientConnect;

        public SocketServer(string ipAddress,int port,int listen = 10)
        {
            IpAddress = IPAddress.Parse(ipAddress);
            Port = port;
            _listen = listen;
            
        }

        public void Start()
        {
            if (Listener == null || !Listener.Connected)
            {
                Clients = new List<SocketClient>();
                _serverThread = new Thread(StartServer);
                _serverThread.Start();
            }
            else
            {
                throw new Exception("Server is already running!");
            }
        }
        
        private async void StartServer()
        {
            IpEndPoint = new IPEndPoint(IpAddress,Port);
            Listener = new Socket(IpEndPoint.AddressFamily,SocketType.Stream,ProtocolType.Tcp);
            Listener.Bind(IpEndPoint);
            Listener.Listen(_listen);
            while (true)
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
            // ReSharper disable once FunctionNeverReturns
        }

        internal void ClientDisconnected(SocketClient client)
        {
            Clients.Remove(client);
        }

        public void SendToAll(string sEvent,string sMessage,SocketClient exception = null)
        {
            foreach (var client in Clients)
            {
                if (exception==null||client!=exception)
                    client.Send(sEvent,sMessage);
            }
        }

        public void Shutdown()
        {
            Listener.Shutdown(SocketShutdown.Both);
            Listener.Disconnect(false);
        }
    }
}