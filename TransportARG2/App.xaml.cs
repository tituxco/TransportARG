using System;
using TransportARG2.Services;
using TransportARG2.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TransportARG2;

namespace TransportARG2
{
    public partial class App : Application
    {        
        public App()
        {           
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage();
            if (string.IsNullOrEmpty(UsuarioConectado.usuarioNombre))
            {
                MainPage = new loginPage("");//new NavigationPage(new MainPage());
            }
            else
            {
                MainPage = new MainPage("");
            }

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

       
    }
}
