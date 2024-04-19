# [Documents](https://github.com/Tidominer/ReSocket/blob/main/Documents/Documents.md) > ReSocket Server

With [`ReSocket`](https://github.com/Tidominer/ReSocket/ReSocket.md), you can effortlessly develop robust socket-based servers without the complexities typically associated with socket programming.

Below is a basic example demonstrating how to create a TCP socket server using [`ReSocket`](https://github.com/Tidominer/ReSocket/ReSocket.md):

```c#
// Create a new server instance
TcpServer server = new TcpServer("127.0.0.1", 143);

// Define event handler for client connections
server.OnClientConnect = (client) =>
{
    // On Client Connect
    
    // Define custom events here
    client.On("event", (msg) =>
    {
        // Handle event
    });

    client.OnDisconnect = () =>
    {
        // On Client Disconnect
    };
};

// Start the server
server.Start();
```

In this example, we instantiate a TcpServer object with the desired IP address (127.0.0.1) and port number (143). We then define an event handler for client connections using the OnClientConnect property of the server instance. Inside this event handler, you can define custom events to handle various client interactions, such as receiving messages ("event") or detecting client disconnections (OnDisconnect). Finally, we start the server using the Start() method.

This example provides a foundation for building TCP socket servers with [`ReSocket`](https://github.com/Tidominer/ReSocket/ReSocket.md), allowing you to easily handle client-server interactions in your applications.
## Classes

  - [**TcpServer**](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpServer.md)
  - [**TcpClientConnection**](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpClientConnection.md)
  - [**TcpMessage**](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpMessage.md)