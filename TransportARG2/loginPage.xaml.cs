using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportARG2.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TransportARG2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class loginPage : ContentPage        
    {
        string redireccionar;
        funcionesGlobales Funcion = new funcionesGlobales();
        public loginPage(string origen)
        {
            InitializeComponent();
            this.BindingContext = this;
            this.IsBusy = false;
            redireccionar = origen;
            var UltimaPagina = Navigation.NavigationStack.LastOrDefault();
            Navigation.RemovePage(UltimaPagina);
        }

       
        private void btnIngresar_Clicked(object sender, EventArgs e)
        {
            this.IsBusy = true;
            if (Funcion.AutenticarUsuario(txtUsuario.Text, txtPassword.Text))
            {
                if (redireccionar == "BuscarCargas")
                {
                    App.Current.MainPage = new NavigationPage(new MainPage("BuscarCargas"));
                    //Navigation.RemovePage(this);
                }
                else if (redireccionar == "CrearSolicitud")
                {
                    App.Current.MainPage = new NavigationPage(new MainPage("CrearSolicitud"));
                    //Navigation.RemovePage(this);
                }
                else
                {
                    App.Current.MainPage = new NavigationPage(new MainPage(""));
                    //NavigationPage.RemovePage(this);
                }

            }
            this.IsBusy = false;
        }

        private void btnRegistrarme_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new registroUsuario());
        }

        private void btnOlvide_Clicked(object sender, EventArgs e)
        {

        }
    }
}