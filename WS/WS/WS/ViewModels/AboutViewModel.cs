using System;
using System.Net.Http;
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
            PostWebCommand = new Command(async () => await Post());
        }

        public ICommand OpenWebCommand { get; }
        public ICommand PostWebCommand { get; }

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

        private async Task Post()
        {
            var client = new HttpClient();
            var response = await client.PostAsync("http://127.0.0.1:8080/api/test", new StringContent("Hej med dig"));
            var code = response.StatusCode;
            var jsonString = await response.Content.ReadAsStringAsync();

        }
    }
}