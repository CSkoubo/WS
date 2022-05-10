using Core;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Core
{
    public class ClientConnection
    {
        private readonly IDTOSerializer serializer;
        private readonly string ip;
        private readonly int port;

        public ClientConnection(IDTOSerializer serializer, string ip, int port)
        {
            this.serializer = serializer;
            this.ip = ip;
            this.port = port;
        }

        public async Task SendObjectAsync<T>(T objectToSend)
        {
            using (var client = new TcpClient())
            {
                await client.ConnectAsync(this.ip, this.port);

                using (var stream = client.GetStream())
                {

                    var dataAsString = serializer.SerializeObject(objectToSend);


                    var objectAsString = serializer.SerializeObject(
                        new DataDescriptor
                        {
                            Type = nameof(DataClass),
                            Value = Encoding.UTF8.GetBytes(dataAsString)
                        });
                    var bytesToSend = Encoding.UTF8.GetBytes(objectAsString);
                    await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);
                    stream.Close();
                }
                client.Close();
            }
        }
    }
}
