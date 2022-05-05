using System.ComponentModel;
using TransportARG2.ViewModels;
using Xamarin.Forms;

namespace TransportARG2.Views
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