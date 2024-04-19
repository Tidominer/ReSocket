# [Documents](https://github.com/Tidominer/ReSocket/blob/main/Documents/Documents.md) > [ReSocket Server](https://github.com/Tidominer/ReSocket/ReSocket.md) > TcpClientConnection

The [`TcpClientConnection`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpClientConnection.md) class is responsible for managing communication with clients connected to the server and is instantiated by the [`TcpServer`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpServer.md) class. It handles receiving and sending data to and from the connected clients.

### Constructor

- **Parameters**:
  - **`client`** (TcpClient): The TcpClient representing the connection to a specific client.
  - **`server`** ([`TcpServer`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpServer.md)): The [`TcpServer`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpServer.md) instance managing this client connection.

### Properties

- **`TcpClient`** (TcpClient): The TcpClient object representing the connection to the client.
- **`Server`** ([`TcpServer`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpServer.md)): The [`TcpServer`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpServer.md) instance managing this client connection.
- **`IsFree`** (bool): Indicates whether the connection is currently available for sending data.
- **`State`** (ConnectionState): The current state of the connection.
- **`ReceiveBufferSize`** (int): The size of the receive buffer for incoming data.

### Events

- **`OnDisconnect`** (Action): An event that occurs when the connection is terminated or disconnected.

### Methods

- **`On(string @event, Action<string> action)`**: Associates an event name with an action to be executed when that event is received from the client.
- **`Send(string @event, string data = "")`**: Sends data to the client with the specified event name.
- **`Disconnect(Exception e)`**: Terminates the connection with the client, handling any exceptions that occur during the disconnection process.

### Private Methods

- **`ReceiveDataAsync()`**: Asynchronously receives data from the client and processes it.
- **`ProcessData(int length)`**: Processes the received data, parsing it into events and their corresponding data.

### Enums

- **`ConnectionState`**: An enumeration representing the various states of the client connection, including Initializing, Connecting, Connected, and Disconnected.