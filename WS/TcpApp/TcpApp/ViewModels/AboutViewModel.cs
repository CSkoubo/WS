using Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace TcpApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "Start server";
            OpenWebCommand = new Command(async () => await Start());
        }

        public ICommand OpenWebCommand { get; }

        private async Task Start()
        {
            DataClass receivedDataClass = null;
            using (var dataReceived = new ManualResetEventSlim())
            {
                var serializer = new JsonSerializerWrapper();
                using (var server = new ServerConnection(serializer, 50001))
                {

                    server.OnDataReveiced += (sender, descriptor) =>
                    {
                        if (descriptor.Type == nameof(DataClass))
                        {
                            receivedDataClass =
                                serializer.DeserializeObject<DataClass>(
                                    Encoding.UTF8.GetString(descriptor.Value, 0, descriptor.Value.Length));
                            dataReceived.Set();
                        }
                    };
                    server.Start();
                }
                dataReceived.Wait();
            }
        }
    }
}