# [Documents](https://github.com/Tidominer/ReSocket/blob/main/Documents/Documents.md) > [ReSocket Server](https://github.com/Tidominer/ReSocket/ReSocket.md) > TcpMessage

The [`TcpMessage`](https://github.com/Tidominer/ReSocket/blob/main/Documents/ReSocket/TcpMessage.md) class facilitates the serialization and deserialization of messages exchanged between the server and clients in a TCP communication scenario.

### Constructor

- **Parameters**:
  - **`@event`** (string): The event associated with the message.
  - **`data`** (string): The data payload of the message.

### Properties

- **`Event`** (string): The event associated with the message.
- **`Data`** (string): The data payload of the message.

### Methods

- **`Serialize(string @event, string data)`**: Serializes the event and data into a formatted message string.
- **`DeSerialize(string message)`**: Deserializes a formatted message string into its event and data components.

### Static Methods

- **`FilterMessage(string message)`**: Escapes special characters in the message string for serialization.
- **`UnFilterMessage(string message)`**: Restores escaped characters in the message string after deserialization.

### Message Format

Messages are formatted as `{{event::data}}`, where `event` represents the event associated with the message, and `data` is the data payload.