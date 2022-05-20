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
        string redireccionar;
        public MainPage(string origen)
        {
            InitializeComponent();
            redireccionar = origen;
            if (redireccionar == "BuscarCargas")
            {
                Navigation.PushAsync(new BuscarCargas());
            }else if (redireccionar == "CrarSolicitud")
            {
                Navigation.PushAsync(new CrearSolicitud());
            }          
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