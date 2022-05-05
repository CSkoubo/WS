using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace webs.Services
{
    public class WebSocketClient
    {
        readonly ClientWebSocket client;
        readonly CancellationTokenSource cts;

        public WebSocketClient()
        {
            client = new ClientWebSocket();
            cts = new CancellationTokenSource();
        }

        public async Task ConnectToServerAsync()
        {
            await client.ConnectAsync(new Uri("ws://192.168.121.103:9500"), cts.Token);
            //UpdateClientState();

            //await Task.Factory.StartNew(async () =>
            //{
            //    while (true)
            //    {
            //       // await ReadMessage();
            //    }
            //}, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public async Task SendMessageAsync(string message)
        {

            var byteMessage = Encoding.UTF8.GetBytes(message);
            var segmnet = new ArraySegment<byte>(byteMessage);

            await client.SendAsync(segmnet, WebSocketMessageType.Text, true, cts.Token);
        }
    }
}
