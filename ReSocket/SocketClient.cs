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
        public readonly Socket Socket;
        public readonly SocketServer Server;
        public readonly string IpAddress;
        public readonly int Port;
        
        public bool Connected { get; private set; }
        public bool Listening { get; private set; }
        
        public Action OnDisconnect;
        
        private int ReceiveBufferSize => Server.ReceiveBufferSize;
        
        private readonly Dictionary<string, Action<string>> _events;

        private readonly byte[] _receiveBuffer;


        public SocketClient(Socket client, SocketServer server)
        {
            Connected = true;
            Socket = client;
            Server = server;
            _events = new Dictionary<string, Action<string>>();
            _receiveBuffer = new byte[ReceiveBufferSize];
            var split = ((IPEndPoint) client.RemoteEndPoint).ToString().Split(':');
            IpAddress = split[0];
            Port = int.Parse(split[1]);
            Thread thread = new Thread(ReceiveDataLoop);
            thread.Start();
        }

        public void StartListening()
        {
            Listening = true;
        }

        public void PauseListening()
        {
            Listening = false;
        }
        
        public void On(string @event, Action<string> action)
        {
            _events.Add(@event, action);
        }

        public void Send(string @event, string text = "")
        {
            var message = "<:>" + FilterMessage(@event) + "<?>" + FilterMessage(text) + "<;>";
            var bytes = Encoding.UTF8.GetBytes(message);
            try
            {
                Socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, ar => { Socket.EndSend(ar); }, new object());
            }catch(Exception e){ Disconnect(e);}
        }

        private string FilterMessage(string message) => message
                .Replace(@"\",@"\\")
                .Replace("<",@"\<")
                .Replace(">",@"\>");
        
        private string UnFilterMessage(string message) => message
            .Replace(@"\<","<")
            .Replace(@"\>",">")
            .Replace(@"\\",@"\");

        private async void ReceiveDataLoop()
        {
            while (true)
            {
                if (Listening && Connected)
                {
                    try
                    {
                        TaskCompletionSource<bool> receiveTask = new TaskCompletionSource<bool>();
                        ReceiveData(receiveTask);
                        await receiveTask.Task;
                    }
                    catch(Exception e)
                    {
                        Disconnect(e);
                    }
                }
            }
            //ReSharper disable once FunctionNeverReturns
        }

        private string _receivedData;

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
                                    _events[UnFilterMessage(message[0])].Invoke(UnFilterMessage(message[1]));
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
                    {
                        Disconnect();
                    }
                }
                catch(Exception e) {
                    Disconnect(e);
                }
            }, new object());
        }

        public void Disconnect(Exception exception = null)
        {
            if (Connected)
            {
                Listening = false;
                Connected = false;
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
                OnDisconnect?.Invoke();
                Server.ClientDisconnected(this);
                Socket.Dispose();
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