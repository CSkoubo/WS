using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class ServerConnection : IDisposable
    {
        private readonly IDTOSerializer serializer;
        private readonly int port;
        public EventHandler<DataDescriptor> OnDataReveiced;
        private TcpListener listener;
        readonly CancellationTokenSource cts = new CancellationTokenSource();
        private ManualResetEventSlim stopped = new ManualResetEventSlim();

        public ServerConnection(IDTOSerializer serializer, int port)
        {
            this.serializer = serializer;
            this.port = port;
        }

        public void Stop()
        {
            cts.Cancel();
            stopped.Wait();

        }

        public static string getCurrentIP()
        {
            var host = Dns.GetHostName();
            IPHostEntry hostEntry = Dns.GetHostEntry(host);
            if (hostEntry.AddressList.Length > 0)
            {
                //logDelegate("Current IP : " + hostEntry.AddressList[0].ToString());
                return hostEntry.AddressList[0].ToString();
            }
            return "";
        }
        public void Start()
        {
            try
            {
                Debug.WriteLine(getCurrentIP());
                this.listener = new TcpListener(IPAddress.Parse(getCurrentIP()), this.port);
                this.listener.Start();
                _ = Task.Run(async () =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        var socket = await listener.AcceptSocketAsync();
                        MemoryStream stream = new MemoryStream();
                        while (socket.Connected && !cts.IsCancellationRequested)
                        {
                            try
                            {
                                var buffer = new byte[5000];
                                int numBytes = socket.Receive(buffer);
                                if (numBytes <= 0)
                                {
                                    break;
                                }
#pragma warning disable CA1835 // Prefer the 'Memory'-based overloads for 'ReadAsync' and 'WriteAsync'
                                await stream.WriteAsync(buffer, 0, numBytes).ConfigureAwait(false);
#pragma warning restore CA1835 // Prefer the 'Memory'-based overloads for 'ReadAsync' and 'WriteAsync'
                                stream.Position = 0;
                                var bytesReceived = stream.ToArray();
                                var objectAsString =
                                    Encoding.UTF8.GetString(bytesReceived, 0, bytesReceived.Length);
                                var descriptor = serializer.DeserializeObject<DataDescriptor>(objectAsString);
                                OnDataReveiced?.Invoke(this, descriptor);
                            }
                            catch (Exception ex)
                            {
                                //TODO better error handling
                                Debug.WriteLine(ex);
                            }
                        }
                    }
                },
                    cts.Token);
            }
            finally
            {
                this.stopped.Set();
            }
        }

        public void Dispose()
        {
            this.cts.Dispose();
            this.stopped.Dispose();
        }
    }

}
