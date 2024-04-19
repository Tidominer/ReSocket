# [Documents](https://github.com/Tidominer/ReSocket/blob/main/Documents/Documents.md) > ReSocket Client


The [`ReSocket Client`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocketClient/ReSocketClient.md) is a companion library designed to seamlessly integrate with [`ReSocket`](https://github.com/Tidominer/ReSocket/ReSocket.md) servers, allowing applications to easily establish connections, send, and receive data. Acting primarily as a wrapper, it simplifies the complexities typically associated with TCP client setup and management, making it highly accessible for developers of all skill levels.

Below is a basic example demonstrating how to establish a connection to a ReSocket server using [`ReSocket Client`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocketClient/ReSocketClient.md):

```c#
// Create a TCP connection to the ReSocket server
TcpConnection connection = new TcpConnection("127.0.0.1", 143);

// Define event handlers for incoming events from the server
connection.On("event", (msg) =>
{
    // Handle incoming event
    Console.WriteLine($"Received message: {msg}");
});

// Connect to the ReSocket server
connection.Connect();

// Send a message to the server with the specified event
var message = "Hello, ReSocket Server!";
connection.Send("event", message);
```
In this example, the [`ReSocket Client`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocketClient/ReSocketClient.md) library is utilized to establish a TCP connection to a ReSocket server running on 127.0.0.1 at port 143. An event handler is defined to handle incoming events with the name "event". After establishing the connection, a message "Hello, ReSocket Server!" is sent to the server with the specified event name.

## Classes
- [**TcpConnection**](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocketClient/TcpConnection.md)
- [**TcpMessage**](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpMessage.md)