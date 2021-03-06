# [Documents](https://github.com/Tidominer/ReSocket/blob/main/Documents/Documents.md) > [ReSocket Client](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket%20Client/ReSocketClient.md) > SocketConnection
#### SocketConnection is a wrapper for TCP Socket Connections. Create an instance of this class to initialize a connection to a socket server.

# Public Variables

<table>
  <tr>
    <th>Variable</th>
    <th>Description</th>
  </tr>
  <tr>
    <td> public readonly Socket Socket </td>
    <td> Socket object of the client. </td>
  </tr>
  <tr>
    <td> public readonly IPAddress IpAddress </td>
    <td> IPAddress of the client. </td>
  </tr>
  <tr>
    <td> public readonly int Port </td>
    <td> Port of the client's connection. </td>
  </tr>
  <tr>
    <td> public readonly Dictionary<string, Action<string>> Events </td>
    <td> Dictionary of defined events. </td>
  </tr>
  <tr>
    <td> public bool Connected </td>
    <td> Statue of connection between client and the server. </td>
  </tr>
  <tr>
    <td> public Action OnDisconnect </td>
    <td> An action which gets invoked when the client disconnects from the server. </td>
  </tr>
  </table>

# Public Methods

<ul>
  <l1> <h2> SocketConnection (string ipAddress, int port) </h2> </li>
  SocketConnection's constructor initializes the socket object, but does not connect it.
  <table>
    <tr>
      <th>Parameter</th>
      <th>Description</th>
    </tr>
    <tr>
      <td>string ipAddress</td>
      <td>IP Address of the target server.</td>
    </tr>
    <tr>
      <td>int port</td>
      <td>Port of the target server.</td>
    </tr>
  </table>
  <l1> <h2> void Connect () </h2> </li>
  Tries to connect the initialized socket to the server. Throws an exception when called while the connection to the server is made.
  <l1> <h2> void Send (string sEvent,string sMessage = "") </h2> </li>
  Sends an event call with a message to the server.
    <table>
    <tr>
      <th>Parameter</th>
      <th>Description</th>
    </tr>
    <tr>
      <td>string sEvent</td>
      <td>Event name.</td>
    </tr>
    <tr>
      <td>string sMessage</td>
      <td>Message or data to send along the event.</td>
    </tr>
  </table>
  <l1> <h2> void On (string rEvent, Action<string> rAction) </h2> </li>
  Adds the given Event and Action to <i>Events</i> dictionary.
  <table>
    <tr>
      <th>Parameter</th>
      <th>Description</th>
    </tr>
    <tr>
      <td>string rEvent</td>
      <td>Event name.</td>
    </tr>
    <tr>
      <td>string rAction</td>
      <td>Action that will be invoked when <i>rEvent</i> is received from the client.</td>
    </tr>
  </table>
  <l1> <h2> public void Disconnect () </h2> </li>
  Closes the connection to ther server and invokes OnDisconnect.
</ul>
