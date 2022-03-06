# [Documents](https://github.com/Tidominer/ReSocket/blob/main/Documents/Documents.md) > [ReSocket Server](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/ReSocket.md) > SockerServer
#### SocketServer is a wrapper for TCP Socket Servers. Create an instance of this class to initialize a socket server.

# Public Variables

<table>
  <tr>
    <td>Variable</td>
    <td>Description</td>
  </tr>
  <tr>
    <td> public readonly IPAddress IpAddress </td>
    <td> IPAddress of the server. </td>
  </tr>
  <tr>
    <td> public readonly int Port </td>
    <td> Port of the server. </td>
  </tr>
  <tr>
    <td> public readonly Socket Listener </td>
    <td> Server's socket object. </td>
  </tr>
  <tr>
    <td> public readonly List<SocketClient> Clients </td>
    <td> List of connected client's SocketClient objects. </td>
  </tr>
  <tr>
    <td> public int ReceiveBufferSize = 1024 </td>
    <td> Size of receive buffer (maximum number of bytes that server can receive from one client at a time. should be the same as buffer size of the client (on client side), otherwise there may be data loss. default = 1 KB. </td>
  </tr>
  <tr>
    <td> public Action<SocketClient> OnClientConnect </td>
    <td> An action that gets called when a client connects to the server. has one parameter 'client' that represents the SocketClient object of the connected client. </td>
  </tr>
  </table>

# Public Methods

<ul>
  <l1> <h2> SocketServer (string ipAddress, int port, int listen = 10) </h2> </li>
  SockerServer's constructor initializes a Socket On given IP and Port <b>but <i>DOES NOT</i> start the server!</b>
  <table>
    <tr>
      <td>Parameter</td>
      <td>Description</td>
    </tr>
    <tr>
      <td>string ipAddress</td>
      <td>IP Address to initialize the server on.</td>
    </tr>
    <tr>
      <td>int port</td>
      <td>Port to initialize the server on.</td>
    </tr>
    <tr>
      <td>int listen</td>
      <td>The maximum length of the pending connections queue of the server (default = 10).</td>
    </tr>
  </table>
  <l1> <h2> void Start () </h2> </li>
  Starts the server. If called more than once, returns an exception.
  <l1> <h2> void SendToAll (string sEvent, string sMessage, SocketClient exception = null) </h2> </li>
  Sends an event call to all connected clients.
    <table>
    <tr>
      <td>Parameter</td>
      <td>Description</td>
    </tr>
    <tr>
      <td>string sEvent</td>
      <td>Event name.</td>
    </tr>
    <tr>
      <td>string sMessage</td>
      <td>Message or data to send along the event.</td>
    </tr>
    <tr>
      <td>SocketClient exception</td>
      <td>This client won't get the call (default = null).</td>
    </tr>
  </table>
  <l1> <h2> void Shutdown () </h2> </li>
  Shutdowns the server and disconnect all clients.
</ul>
