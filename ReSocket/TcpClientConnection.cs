// ReSocket by Tidominer
// https://github.com/Tidominer/ReSocket/

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReSocket
{
    public class TcpClientConnection
    {
        // ReSharper disable MemberCanBePrivate.Global UnusedAutoPropertyAccessor.Global UnusedMember.Global
        public readonly TcpClient TcpClient;
        public readonly TcpServer Server;
        public bool IsFree;
        public ConnectionState State;
        public Action OnDisconnect;
        public readonly Dictionary<string, Action<string>> Events;

        public int ReceiveBufferSize => Server.ReceiveBufferSize;
        private readonly byte[] _receiveBuffer;
        private string _receivedData;
        private readonly Thread _listeningThread;

        public TcpClientConnection(TcpClient client, TcpServer server)
        {
            TcpClient = client;
            Server = server;
            IsFree = true;
            client.ReceiveBufferSize = ReceiveBufferSize;
            Events = new Dictionary<string, Action<string>>();
            _receivedData = "";
            _receiveBuffer = new byte[ReceiveBufferSize];
            State = ConnectionState.Connected;
            _listeningThread = new Thread(ReceiveDataAsync);
            _listeningThread.Start();
        }
        public void On(string @event, Action<string> action) => Events.Add(@event, action);
        public async Task Send(string @event, string data = "")
        {
            while (!IsFree)
                await Task.Delay(25);

            IsFree = false;
            var bytes = Encoding.UTF8.GetBytes(TcpMessage.Serialize(@event, data));
            try
            {
                await TcpClient.GetStream().WriteAsync(bytes, 0, bytes.Length);
            }
            catch { /*Ignored*/ }
            IsFree = true;
        }
        private async void ReceiveDataAsync()
        {
            try
            {
                using var stream = TcpClient.GetStream();
                while (State == ConnectionState.Connected)
                {
                    var length = await stream.ReadAsync(_receiveBuffer, 0, _receiveBuffer.Length);
                    if (length > 0)
                        ProcessData(length);
                }
            }
            catch (Exception e) {
                Disconnect(e);
            }
        }
        private void ProcessData(int length)
        {
            _receivedData += Encoding.UTF8.GetString(_receiveBuffer, 0, length);
            
            if (_receivedData.Length<2)
                return;
            
            var startIndex = _receivedData.IndexOf("{{", StringComparison.Ordinal);
            var endIndex = _receivedData.IndexOf("}}", StringComparison.Ordinal);

            if (startIndex == -1)
            {
                _receivedData = "";
                return;
            }
            if (startIndex > 0)
            {
                _receivedData = _receivedData.Substring(startIndex, _receivedData.Length - startIndex);
                endIndex = _receivedData.IndexOf("}}", StringComparison.Ordinal);
            }

            if (endIndex == -1)
                return;

            var message = TcpMessage.DeSerialize(_receivedData.Substring(0, endIndex + 2));
            _receivedData = _receivedData.Length == endIndex + 2
                ? ""
                : _receivedData.Substring(endIndex + 2, _receivedData.Length - endIndex + 2);

            foreach (var @event in Events)
            {
                if (@event.Key == message.Event){
                    @event.Value.Invoke(message.Data);
                    break;
                }
            }
        }
        public void Disconnect(Exception e)
        {
            if (State == ConnectionState.Connected)
            {
                _listeningThread.Abort();
                State = ConnectionState.Disconnected;
                OnDisconnect?.Invoke();
                try
                {
                    TcpClient.Client.Shutdown(SocketShutdown.Both);
                    TcpClient.Client.Close();
                }
                catch { /* ignored*/ }

                Server.ClientDisconnected(this);
                TcpClient.Client.Dispose();
            }
        }
    }
    public enum ConnectionState
    {
        Initializing,
        Connecting,
        Connected,
        Disconnected
    }
}