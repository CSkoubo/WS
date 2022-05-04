using System;
using System.Collections.Generic;
using System.ComponentModel;
using WS.Models;
using WS.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WS.Views
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