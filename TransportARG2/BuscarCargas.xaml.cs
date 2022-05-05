using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TransportARG2.Services;
using Xamarin.Forms.Maps;
using System.Data;
using MySqlConnector;
using Plugin.Geolocator;
using System.Collections.ObjectModel;

namespace TransportARG2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuscarCargas : ContentPage
    {
        private ObservableCollection<Pin> allPines = new ObservableCollection<Pin>();
        public ObservableCollection<Pin> AllPines
        {
            get
            {
                return allPines;
            }
            set
            {
                allPines = value;
                OnPropertyChanged("AllPines");
            }
        }

        private ObservableCollection<Position> ruta = new ObservableCollection<Position>();
        public ObservableCollection<Position> Ruta
        {
            get
            {
                return ruta;
            }
            set
            {
                ruta = value;
                OnPropertyChanged("Ruta");
            }
        }
        public BuscarCargas()
        {
            InitializeComponent();
            Button btnBuscarCargas = new Button();
            btnBuscarCargas.Text = "Buscar";
            btnBuscarCargas.Clicked += btnBuscarCargas_Click;
            mapaInicial.Children.Add(btnBuscarCargas);
        }
        private void btnBuscarCargas_Click(object sender, EventArgs e)
        {
            //var location = await Geolocation.GetLastKnownLocationAsync();
            string error;
            conectarMysql conectar = new conectarMysql("kiremoto", "mecago");
            if (conectar.IntentarConectar(out error))
            {
                string consulta = "select id, origen_coordenadas,origen_comentarios from servicio_solicitud_origen";
                MySqlConnection conexionMysql = new MySqlConnection(conectar.builder.ToString());
                conexionMysql.Open();

                MySqlDataAdapter verSolicitudes_origen = new MySqlDataAdapter(consulta, conexionMysql);
                DataTable solicitudes_origen = new DataTable();
                verSolicitudes_origen.Fill(solicitudes_origen);
                Position miPosicion = new Position(-29.138486814739153, -59.64165222910585);//ShowMyAdress().Result;//new Position(double.Parse(miPosicionSTR[0]), double.Parse(miPosicionSTR[1]));
                //Position miPosicion = ShowMyAdress().Result;         
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(miPosicion, Distance.FromKilometers(50));
                Map map = new Map(mapSpan);

                //CustomMap map2 = new CustomMap();
                foreach (DataRow solicitud_origen in solicitudes_origen.Rows)
                {
                    string[] coordenadas = solicitud_origen["origen_coordenadas"].ToString().Split(char.Parse(","));
                    //AllPines.Add(new Pin() { 
                    Pin origenPin = new Pin
                    {
                        Position = new Position(double.Parse(coordenadas[0]), double.Parse(coordenadas[1])),
                        Label = solicitud_origen["id"].ToString(),
                        Type = PinType.Place,
                    };
                    //});
                    origenPin.MarkerClicked += OrigenPin_MarkerClicked;


                    map.Pins.Add(origenPin);
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(miPosicion, Distance.FromKilometers(50)));
                    //DisplayAlert("MM", allPines.Count.ToString(), "ok");                    
                }
                //map2.MapPins = AllPines;
                Button btnBuscarCargas = new Button();
                btnBuscarCargas.Text = "Buscar";
                btnBuscarCargas.Clicked += btnBuscarCargas_Click;

                mapaInicial.Children.Clear();                
                mapaInicial.Children.Add(map);
                mapaInicial.Children.Add(btnBuscarCargas);
                conexionMysql.Close();
            }
            else
            {
                DisplayAlert("ERROR", error, "ok");
            }
        }

        private async void OrigenPin_MarkerClicked(object sender, PinClickedEventArgs e)
        {
            var pin = (Pin)sender;
            await Navigation.PushAsync(new InfoCarga(int.Parse(pin.Label)));
        }

        public async Task<Position> ShowMyAdress()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;
            var position = await locator.GetPositionAsync(timeout: TimeSpan.FromSeconds(10));
            Position pos = new Position(position.Latitude, position.Longitude);
            //var locationAddress = (await (new Geocoder()).GetAddressesForPositionAsync(pos));
            return pos;
        }

        
    }
}