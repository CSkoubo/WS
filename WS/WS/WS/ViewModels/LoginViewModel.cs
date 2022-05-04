using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WS.Views;
using Xamarin.Forms;

namespace WS.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private Task ServerTask;
        private async void OnLoginClicked(object obj)
        {
            if (ServerTask != null)
                return;
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            using(var server = LocalWebserver.CreateWebServer("http://127.0.0.1:8080"))
            {
                ServerTask = server.RunAsync();
            }
        }
    }
}
