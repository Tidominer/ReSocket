using System;

namespace ReSocket.Example
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            int port = 143;
            SocketServer server = new SocketServer(ip,port);

            server.OnClientConnect = (client) =>
            {
                var id = ShortID.Generate(10);
                Console.WriteLine("Client '{0}' connected. Client IP : {1}",id,client.IpAddress);
                client.Send("SetID",id);

                client.On("ping", (msg) =>
                {
                    client.Send("ping");
                    Console.WriteLine("Client '{0}' requested Ping.",id);
                });
                
                client.On("myPing", (msg) =>
                {
                    Console.WriteLine("Client '{0}' Ping is {1}.",id,msg);
                });
                
                client.On("Introduce", (msg) =>
                {
                    server.SendToAll("Introduction",id+"<id:name>"+ msg,client);
                    Console.WriteLine("Client '{0}' introduced himself as '{1}'.",id,msg);
                });

                client.OnDisconnect = () =>
                {
                    Console.WriteLine("Client '{0}' disconnected.",id);
                };
            };
            
            server.Start();

            Console.ReadLine();
        }
    }
}