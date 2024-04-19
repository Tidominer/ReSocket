// ReSocketClient by Tidominer
// https://github.com/Tidominer/ReSocket/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReSocketClient
{
    public class TcpConnection
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Global MemberCanBePrivate.Global
        public TcpClient TcpClient;
        public readonly string HostName;
        public readonly int Port;
        public readonly Dictionary<string, Action<string>> Events;
        public ConnectionState State;
        public bool IsFree;
        
        public readonly int ReceiveBufferSize;
        private readonly byte[] _receiveBuffer;
        private string _receivedData;
        private Thread _listeningThread;

        public TcpConnection(string hostName, int port, int bufferSize = 1024)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HostName = hostName;
            Port = port;
            Events = new Dictionary<string, Action<string>>();
            State = ConnectionState.Initializing;
            ReceiveBufferSize = bufferSize;

            _receiveBuffer = new byte[ReceiveBufferSize];
            _receivedData = "";
        }
        public void Connect()
        {
            if (State==ConnectionState.Connected || State==ConnectionState.Connecting)
                throw new Exception($"TcpClient is currently {State} to server");
            TcpClient = new TcpClient();
            TcpClient.Connect(HostName, Port);
            State = ConnectionState.Connected;
            IsFree = true;
            _listeningThread = new Thread(ReceiveDataAsync);
            _listeningThread.Start();
        }
        public async Task Send(string @event,string data = "")
        {
            while (!IsFree)
                await Task.Delay(25);
 
            IsFree = false;
            var bytes = Encoding.UTF8.GetBytes(TcpMessage.Serialize(@event, data));
            try
            {
                await TcpClient.GetStream().WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception e) { /*Ignored*/ }
            IsFree = true;
        }
        public void On(string @event, Action<string> action) => Events.Add(@event, action);
        private async void ReceiveDataAsync()
        {
            try
            {
                using var stream = TcpClient.GetStream();
                while (State == ConnectionState.Connected)
                {
                    var length = await stream.ReadAsync(_receiveBuffer, 0, _receiveBuffer.Length);
                    if (length >= 0)
                    {
                        ProcessData(length);
                    }
                }
            }
            catch(Exception e){ Disconnect(e);}
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
        public void Disconnect(Exception exception = null)
        {
            if (State == ConnectionState.Connected)
            {
                State = ConnectionState.Disconnected;
                TcpClient.Close();
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