using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WS.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await StartServer());
        }

        public ICommand OpenWebCommand { get; }

        private Task ServerTask;
        private async Task StartServer()
        {
            if (ServerTask != null)
                return;

            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            var server = LocalWebserver.CreateWebServer("http://127.0.0.1:8080");
            
            ServerTask = server.RunAsync();
            try
            {

                await ServerTask.ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}