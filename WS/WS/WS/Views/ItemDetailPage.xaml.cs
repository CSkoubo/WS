using System.ComponentModel;
using WS.ViewModels;
using Xamarin.Forms;

namespace WS.Views
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