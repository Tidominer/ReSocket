# [Documents](https://github.com/Tidominer/ReSocket/blob/main/Documents/Documents.md) > [ReSocket Client](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocketClient/ReSocketClient.md) > TcpConnection


The [`TcpConnection`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocketClient/TcpConnection.md) class in the [`ReSocket Client`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocketClient/ReSocketClient.md) library facilitates the establishment of TCP connections with ReSocket servers, enabling efficient communication between client and server applications. This class abstracts away the complexities of socket programming, providing a simple interface for sending and receiving data asynchronously.

### Constructor
- **Parameters**:
  - **`hostName`** (string): The host name or IP address of the server to which the TCP connection will be established.
  - **`port`** (int): The port number on the server to which the TCP connection will be made.
  - **`bufferSize`**  (int, optional, default: 1024): The size of the receive buffer used for incoming data.

### Properties
- **`TcpClient`**: Represents the underlying TCP client used for communication.
- **`HostName`**: The host name or IP address of the server.
- **`Port`**: The port number on which the server is listening.
- **`Events`**: A dictionary storing event names and corresponding event handler actions.
- **`State`**: Represents the current connection state.
- **`IsFree`**: Indicates whether the connection is available for sending data.

### Methods
- **`Connect()`**: Initiates a connection to the server.
- **`Send(string @event, string data = "")`**: Asynchronously sends a message to the server with the specified event name and optional data payload.
- **`On(string @event, Action<string> action)`**: Registers an event handler for the specified event name.
- **`Disconnect(Exception exception = null)`**: Closes the connection to the server, optionally specifying an exception that triggered the disconnection.

### Private Methods
- **`ReceiveDataAsync()`**: Asynchronously receives and processes data from the server.
- **`ProcessData(int length)`**: Processes incoming data and invokes the corresponding event handlers.

### Enums
- **`ConnectionState`**: Represents the various states of the connection (Initializing, Connecting, Connected, Disconnected).