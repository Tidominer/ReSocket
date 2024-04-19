// ReSocket by Tidominer
// https://github.com/Tidominer/ReSocket/

using System;

namespace ReSocket
{
    public class TcpMessage
    {
        //Message Format : '{{event::message}}'
        public string Event, Data;
        public TcpMessage(string @event, string data)
        {
            Event = @event;
            Data = data;
        }

        public static string Serialize(string @event, string data)
        {
            return "{{" + FilterMessage(@event) + "::" + FilterMessage(data) + "}}";
        }

        public static TcpMessage DeSerialize(string message)
        {
            if (!message.StartsWith("{{") || !message.EndsWith("}}"))
                throw new Exception("Invalid Message. Start or End is not specified.");
            
            message = message.Substring(2, message.Length - 4);
            
            var splitIndex = message.IndexOf("::", StringComparison.Ordinal);
            if (splitIndex == -1)
                throw new Exception("Invalid Message. Splitter is not specified.");

            var data = message.Length == splitIndex + 2
                ? ""
                : message.Substring(splitIndex + 2, message.Length - splitIndex + 2);
            var @event = splitIndex == 0
                ? ""
                : message.Substring(0, splitIndex);
            
            return new TcpMessage(@event, data);
        }
        
        public static string FilterMessage(string message) => message
            .Replace(@"\",@"\\")
            .Replace("{",@"\{")
            .Replace("}",@"\}")
            .Replace(":",@"\:");

        public static string UnFilterMessage(string message) => message
            .Replace(@"\{","{")
            .Replace(@"\}","}")
            .Replace(@"\:",":")
            .Replace(@"\\",@"\");
    }
}