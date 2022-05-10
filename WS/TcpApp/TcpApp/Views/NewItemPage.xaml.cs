using System;
using System.Collections.Generic;
using System.ComponentModel;
using TcpApp.Models;
using TcpApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TcpApp.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}