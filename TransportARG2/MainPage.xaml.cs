using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TransportARG2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();
            menuPagina.listview.ItemSelected += Listview_ItemSelected;
        }

        private void Listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as menuItemPagina;
            if (item != null)
            {               
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.PaginaDestino));
                menuPagina.listview.SelectedItem = null;
                IsPresented = false; 
            }

        }
    }
}