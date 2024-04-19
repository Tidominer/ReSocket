# [Documents](https://github.com/Tidominer/ReSocket/blob/main/Documents/Documents.md) > [ReSocket Server](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/ReSocket.md) > TcpServer

The [`TcpServer`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpServer.md) class facilitates the handling of new TCP connections within your application. Upon instantiation, this class takes an IP address and a port as parameters, initiating the server's listening process for incoming connections. When a new client connects, it creates an instance of the [`TcpClientConnection`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpClientConnection.md) class and passes the new connection to the created instance, while continuing to listen for additional connections.

### Constructor

- **Parameters**:
  - **`ipAddress`** (string): The IP address on which the server will listen for incoming connections.
  - **`port`** (int): The port number on which the server will listen for incoming connections.

### Properties

- **`IpAddress`** (IPAddress): The IP address on which the server is li**stening.
- **`Port`** (int): The port number on which the server is listening.
- **`Listening`** (bool): Indicates whether the server is currently listening for incoming connections.
- **`ClientsList`** (List<[`TcpClientConnection`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpClientConnection.md)>): A list containing instances of connected [`TcpClientConnection`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpClientConnection.md) objects.
- **`ReceiveBufferSize`** (int): The size of the receive buffer for incoming data.

### Events

- **`OnClientConnect`** (Action<[`TcpClientConnection`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpClientConnection.md)>): An event that occurs when a new client successfully connects to the server. It provides access to the connected [`TcpClientConnection`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpClientConnection.md) object.

### Methods

- **`Start()`**: Initiates the server's listening process for incoming connections. Throws an exception if the server is already running.

- **`Shutdown()`**: Stops the server from listening for incoming connections and shuts down the server.

### Private Methods

- **`ListenAsync()`**: An internal method that asynchronously listens for incoming connections. It accepts new connections, creates corresponding [`TcpClientConnection`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpClientConnection.md) instances, and invokes the `OnClientConnect` event handler for each new connection.

- **`ClientDisconnected(`[`TcpClientConnection`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpClientConnection.md)` tcpClientConnection)`**: An internal method used to handle client disconnection events. It removes the disconnected client from the `ClientsList`.