#  ReSocket

ReSocket, short for "Really Easy Socket," is a lightweight C# library designed to simplify the creation of TCP socket servers and clients. This library serves as a wrapper, offering users a straightforward interface for implementing event-based communication in their applications.

# Usage Examples
### ReSocket Server
Here's a brief usage example for the ReSocket library to create a TCP socket server. For a more detailed example, please refer to the [documentation](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/ReSocket.md).

```c#
TcpServer server = new TcpServer("127.0.0.1", 143);

server.OnClientConnect = (client) =>
{
    Console.WriteLine("Client Connected!");

    client.On("event", (msg) =>
    {
        Console.WriteLine($"Received message: {msg}");
    });

    client.OnDisconnect = () =>
    {
        Console.WriteLine("Client Disconnected!");
    };
};

server.Start();
```
    
### ReSocket Client
Here's a brief usage example for the ReSocket Client library to establish a TCP connection with a ReSocket server. For a more detailed example, please refer to the [documentation](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocketClient/ReSocketClient.md).

```c#
TcpConnection connection = new TcpConnection("127.0.0.1", 143);

connection.On("event", (msg) =>
{
    Console.WriteLine($"Received message: {msg}");
});

connection.Connect();

connection.Send("event", "message");
```

# [Documentation](https://github.com/Tidominer/ReSocket/blob/main/Documents/Documents.md)
The documentation includes usage examples for both ReSocket and ReSocket Client, along with concise explanations of their respective classes.