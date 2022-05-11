using Core;
using System;
using System.Threading.Tasks;

namespace TcpClientnet48
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating client");
            DataClass sendDataClass = new DataClass { Number = 2, Text = "TextAAAAA" };
            var serializer = new JsonSerializerWrapper();
            var client = new ClientConnection(serializer, "192.168.1.83", 50001);
            Console.WriteLine("client created");
            Console.WriteLine("sending....");
            Task.Run(async () => await client.SendObjectAsync(sendDataClass));
            Console.WriteLine("sent.");
        }
    }
}



