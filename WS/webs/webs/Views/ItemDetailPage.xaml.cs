using System.ComponentModel;
using webs.ViewModels;
using Xamarin.Forms;

namespace webs.Views
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