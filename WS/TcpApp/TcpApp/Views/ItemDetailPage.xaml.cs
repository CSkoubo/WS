using System.ComponentModel;
using TcpApp.ViewModels;
using Xamarin.Forms;

namespace TcpApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}