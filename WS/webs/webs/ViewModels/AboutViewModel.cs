using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using webs.Services;
using Xamarin.Forms;

namespace webs.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await StartServer());
        }

        public ICommand OpenWebCommand { get; }
        public ICommand PostWebCommand { get; }

        public string MyIp => WebSocketServer.getCurrentIP();

        private ClientWebSocket websocketClient;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private CancellationToken cancellationToken;
        private Uri websocketServerUIR;

        private Task ServerTask;

        private WebSocketServer server;
        private async Task StartServer()
        {
            if (ServerTask != null)
                return;

            try
            {
                server = new WebSocketServer();
                ServerTask = server.Start(WebSocketServer.getCurrentIP(), 5000);
            }
            catch (Exception ex)
            {
                //logDelegate("Connnection to websocket server failed : " + ex.Message.ToString());
            }
        }
    }
}