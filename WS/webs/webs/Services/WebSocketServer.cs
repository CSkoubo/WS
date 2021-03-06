using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;

namespace webs.Services
{
    public  class WebSocketServer
    {
        WebSocketListener server;
        CancellationTokenSource cancellation;

        bool stopServer = false;
        List<WebSocket> sockets = new List<WebSocket>();

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

        public void stop()
        {
            this.stopServer = true;
            server.Stop();
            //logDelegate("Server stopped.");
        }

        public async Task Start(string ip, int port)
        {
            try
            {
                cancellation = new CancellationTokenSource();

                var endpoint = new IPEndPoint(IPAddress.Parse(ip), port);
                // var endpoint = new IPEndPoint(IPAddress.Any, port);
                server = new WebSocketListener(endpoint, new WebSocketListenerOptions() { SubProtocols = new[] { "text" } });
                server.Standards.RegisterStandard(new WebSocketFactoryRfc6455());
                await server.StartAsync();

                // start listening for client connection
                var task = Task.Run(() => AcceptWebSocketClients(server, cancellation.Token));
            }
            catch (Exception ex)
            {
            }
        }

        private async Task AcceptWebSocketClients(WebSocketListener server, CancellationToken token)
        {
            while (!token.IsCancellationRequested && !this.stopServer)
            {
                try
                {
                    var ws = await server.AcceptWebSocketAsync(token);
                    if (ws == null)
                        continue;

                    //logDelegate("AcceptWebSocketClients " + ws.RemoteEndpoint.ToString());
                    sockets.Add(ws);

                    Task.Run(() => HandleConnectionAsync(ws, token));
                }
                catch (Exception aex)
                {
                    var ex = aex.GetBaseException();
                    //logDelegate("Error Accepting client: " + ex.GetType().Name + ": " + ex.Message);
                }
            }
            //logDelegate("Server Stop accepting clients");
        }

        async Task HandleConnectionAsync(WebSocket ws, CancellationToken cancellation)
        {
            try
            {
                if (ws.IsConnected == false)
                {
                    //logDelegate("client disconnected : " + ws.RemoteEndpoint.ToString());
                    this.sockets.Remove(ws);
                    ws.Dispose();
                    return;
                }
                while (ws.IsConnected && !cancellation.IsCancellationRequested)
                {
                    String msg = await ws.ReadStringAsync(cancellation).ConfigureAwait(false);
                    if (msg == null)
                        continue;

                    ws.WriteString("received");  //msg);
                    //logDelegate("received : " + msg);

                    // sen,d back the message to all clients
                    SendMessageAsync(msg);
                }
               // logDelegate("clienthhhhh disconnected : " + ws.RemoteEndpoint.ToString());
            }
            catch (TaskCanceledException)
            {
                //logDelegate("Cancellation exception ");
            }
            catch (Exception aex)
            {
               // logDelegate("Error Handling connection: " + aex.GetBaseException().Message);
                try { ws.Close(); }
                catch { }
            }
            finally
            {
                //ws.Dispose(); // why this call ???= is it e mistake ?
            }
        }

        public async void SendMessageAsync(string message)
        {
            //logDelegate("Send message to clients: " + message);

            List<WebSocket> socketsToRemove = new List<WebSocket>();
            foreach (WebSocket ws in this.sockets)
            {
                try
                {
                    using (var messageWriterStream = ws.CreateMessageWriter(WebSocketMessageType.Text))
                    {
                        using (var sw = new StreamWriter(messageWriterStream, Encoding.UTF8))
                        {
                            await sw.WriteAsync(message);
                        }
                    }
                }
                catch (Exception ex)
                {
                   // logDelegate("Send message error : " + ex.Message.ToString());
                   // logDelegate("remove client from list : " + ws.RemoteEndpoint.ToString());
                    socketsToRemove.Add(ws);
                }
            }

            foreach (WebSocket ws in socketsToRemove)
            {
                this.sockets.Remove(ws);
            }

        }
    }
}
