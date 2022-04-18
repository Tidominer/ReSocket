# [Documents](https://github.com/Tidominer/ReSocket/blob/main/Documents/Documents.md) > [ReSocket Server](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/ReSocket.md) > SocketClient
#### SocketClient is a wrapper for connected clients to [SocketServer](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/SocketServer.md). Don't Create an instance of it, it should only be made by [SocketServer](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/SocketServer.md). You can access the SocketClient of the connected clients in the SocketServer's OnClientConnect action.

<ul>
  <l1> <h2> SocketClient (Socket client, SocketServer server) </h2> </li>
  SocketClient's constructor initializes the class variables and prepare it for holding the client's socket.
  <table>
    <tr>
      <td>Parameter</td>
      <td>Description</td>
    </tr>
    <tr>
      <td>Socket client</td>
      <td>Socket object of the connected client.</td>
    </tr>
    <tr>
      <td>SocketServer server</td>
      <td>Server's SocketServer object.</td>
    </tr>
  </table>
  <l1> <h2> void StartListening () </h2> </li>
  Sets <i>Listenting boolean</i> to true so the class begins listening to the data coming from the client's device. This method gets automatically called from SocketServer when initializes.
  <l1> <h2> void PauseListening () </h2> </li>
  Sets <i>Listenting boolean</i> to false so the class stops listening to the data coming from the client's device.
  <l1> <h2> void On (string rEvent, Action<string> rAction) </h2> </li>
    Adds the given Event and Action to <i>_events</i> dictionary.
    <table>
    <tr>
      <td>Parameter</td>
      <td>Description</td>
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
  <l1> <h2> void Send (string sEvent, string sMessage = "") </h2> </li>
  WIP
</ul>
