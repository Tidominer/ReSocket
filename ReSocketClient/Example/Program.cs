using System;

namespace ReSocketClient.Example
{
    internal static class Program
    {
        public static void Main()
        {
            SocketConnection socket = new SocketConnection("127.0.0.1",143);
            
            socket.On("SetID", (msg) =>
            {
                Console.WriteLine("My Id is : " + msg);
            });
            socket.On("Introduction", (msg) =>
            {
                var split = msg.Split(new[] {"<id:name>"}, StringSplitOptions.None);
                Console.WriteLine("{0}({1}) Connected to the Server.",split[1],split[0]);
            });
            
            var pingRequestTime = DateTime.Now;
            socket.On("ping", (msg) =>
            {
                var t = (DateTime.Now-pingRequestTime);
                var ping = t.Seconds * 1000 + t.Milliseconds;
                Console.WriteLine(ping);
                socket.Send("myPing",ping.ToString());
            });
            socket.Connect();
            Console.WriteLine("Connected!");
            socket.Send("ping");
            string name = Console.ReadLine();
            socket.Send("Introduce",name);
            while (!socket.Disconnected)
            {
                var command = Console.ReadLine();
                if (command != "" && !socket.Disconnected)
                {
                    var split = command.Split('\\');
                    if (split.Length > 1)
                    {
                        string sEvent = split[0], sMsg = split[1];
                        socket.Send(sEvent, sMsg);
                    }
                    else
                    {
                        if (command == "ping")
                        {
                            pingRequestTime = DateTime.Now;
                            socket.Send(command);
                        }
                        else if (command == "disconnect")
                        {
                            Console.WriteLine(socket.Connected);
                            socket.Disconnect();
                            break;
                        }else
                            socket.Send(command);
                    }
                }
            }
            Console.WriteLine("Press Enter to close ...");
            Console.ReadLine();
        }
    }
}