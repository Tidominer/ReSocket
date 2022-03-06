# [Documents](https://github.com/Tidominer/ReSocket/blob/main/Documents/Documents.md) > [ReSocket Server](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/ReSocket.md) > SockerServer
#### SocketServer is a wrapper for TCP Socket Servers. Create an instance of this class to initialize a socket server.

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
