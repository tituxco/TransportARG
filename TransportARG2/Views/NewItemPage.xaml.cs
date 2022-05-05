using System;
using System.Collections.Generic;
using System.ComponentModel;
using TransportARG2.Models;
using TransportARG2.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TransportARG2.Views
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