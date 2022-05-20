using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TransportARG2;

namespace TransportARG2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class home : ContentPage
    {
        public home()
        {
            InitializeComponent();            
            if (string.IsNullOrEmpty(UsuarioConectado.usuarioNombre))
            {                
                App.Current.MainPage=new loginPage("");
                return;
            }
            else
            {
                lblBienvenido.Text = "Bienvenido " + UsuarioConectado.usuarioNombre;
            }
            if (UsuarioConectado.tipoUsuario == "1")
            {
                
            }
        }
        private void btnBuscarCargas_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BuscarCargas());
        }

        private void btnMisCargas_Clicked(object sender, EventArgs e)
        {
            
        }

        private void btnMisDatos_Clicked(object sender, EventArgs e)
        {

        }

        private void btnMisCamiones_Clicked(object sender, EventArgs e)
        {

        }
    }
}