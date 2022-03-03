using System;

namespace SocketNetworking
{
    public class ShortID
    {
        private static string chars =
            "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static string Generate(int length)
        {
            var id = "";
            Random random = new Random();
            for (int i = 0; i < length; i++)
                id += chars[random.Next(0, chars.Length)];
            return id;
        }
    }
}