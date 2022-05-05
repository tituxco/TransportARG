using System;
using TransportARG2.Services;
using TransportARG2.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TransportARG2
{
    public partial class App : Application
    {

        public App()
        {           
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new MainPage());
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
