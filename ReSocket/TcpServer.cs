// ReSocket by Tidominer
// https://github.com/Tidominer/ReSocket/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ReSocket
{
    public class TcpServer
    {
        // ReSharper disable MemberCanBePrivate.Global UnusedMember.Global
        public readonly IPAddress IpAddress;
        public readonly int Port;
        public TcpListener Listener;
        public bool Listening;
        public readonly List<TcpClientConnection> ClientsList;
        public readonly int ReceiveBufferSize = 1024;
        
        private Thread _listenThread;

        public Action<TcpClientConnection> OnClientConnect;

        public TcpServer(string ipAddress,int port)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            IpAddress = IPAddress.Parse(ipAddress);
            Port = port;
            ClientsList = new List<TcpClientConnection>();
        }

        public void Start()
        {
            if (Listening)
                throw new Exception("Server is already running.");

            StartListenThread();
        }

        private void StartListenThread()
        {
            _listenThread = new Thread(ListenAsync);
            _listenThread.Start();
        }
        
        private async void ListenAsync()
        {
            if (!Listening)
            {
                Listener = new TcpListener(IpAddress, Port);
                Listener.Start();
                Listening = true;
            }

            try
            {
                var client = await Listener.AcceptTcpClientAsync();
                var clientObject = new TcpClientConnection(client, this);
                lock (ClientsList)
                    ClientsList.Add(clientObject);
                OnClientConnect?.Invoke(clientObject);
                StartListenThread();
            }
            catch (SocketException)
            {
                StartListenThread();
            }
        }

        internal void ClientDisconnected(TcpClientConnection tcpClientConnection)
        {
            ClientsList.Remove(tcpClientConnection);
        }

        public void Shutdown()
        {
            Listener.Stop();
            Listening = false;
        }
    }
}