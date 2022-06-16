#  ReSocket
ReSocket is an event-based C# TCP Socket wrapper which makes writing socket servers and clients **Really Easy**.
# How to Use
### ReSocket Server
After downloading and adding the "ReSocket" directory to your project, you can initialize a tcp socket server like this :

    ReSocket.SocketServer server = new ReSocket.SocketServer(IP,Port);
    server.OnClientConnect = (client) => {
        //Use client.On to define an event or client.Send to send an event to client.
    };
    server.Start();
    
### ReSocket Client
After downloading and adding the "ReSocketClient" directory to your project, you can initialize a tcp socket connection like this :

    ReSocketClient.SocketConnection connection = new ReSocketClient.SocketConnection(IP,Port);
    //Use connection.On to define an event
    connection.Connect();
    //Use connection.Send to send an event
    
# Unity Client
There is also a client That works in Unity engine. You can use it for making online games. An example for that will be provided soon.
# [Documents](https://github.com/Tidominer/ReSocket/blob/main/Documents/Documents.md)
Writing documents is in progress.
