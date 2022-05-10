using System;
using System.Collections.Generic;
using TcpApp.ViewModels;
using TcpApp.Views;
using Xamarin.Forms;

namespace TcpApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
