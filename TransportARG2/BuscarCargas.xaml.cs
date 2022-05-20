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
//using Xamarin.Essentials;

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
            if (string.IsNullOrEmpty(UsuarioConectado.usuarioNombre))
            {            
                App.Current.MainPage=new loginPage("BuscarCargas");             
                return;
            }           
        }
        private void btnBuscarCargas_Click(object sender, EventArgs e)
        {           
            string error;
            conectarMysql conectar = new conectarMysql("kiremoto", "mecago");
            if (conectar.IntentarConectar(out error))
            {
                string consulta = "select ori.id, " +
                "concat(ori.origen_ciudad_id, ', ', ori.origen_provincia_id) origenCarga, " +
                "concat(desti.destino_ciudad_id, ', ', desti.destino_provincia_id) as destinoCarga, " +
                "ori.carga_importe as montoCarga, 'fechaCarga' as fechaCarga, tipoServ.tipo as tipoCarga, tipoServ.tipo as tipoServicio, " +
                "concat(camion.familia, ' - ', camion.sub_familia) as tipoCamion, " +
                "ori.origen_coordenadas, ori.origen_comentarios " +
                "from servicio_solicitud_origen as ori, " +
                "servicio_solicitud_destino as desti, " +
                "servicio_tipo as tipoServ, " +
                "carga_tipo as tipoCarg, " +
                "camiones_tipos as camion " +
                "where desti.servicio_solicitud_id = ori.id and " +
                "ori.servicio_tipo_id = tipoServ.id and " +
                "ori.carga_tipo_id = tipoCarg.id and " +
                "ori.camion_tipo_id = camion.id";
                MySqlConnection conexionMysql = new MySqlConnection(conectar.builder.ToString());
                conexionMysql.Open();

                MySqlDataAdapter verSolicitudes_origen = new MySqlDataAdapter(consulta, conexionMysql);
                DataTable solicitudes_origen = new DataTable();
                verSolicitudes_origen.Fill(solicitudes_origen);
               
                Position miPosicion = ShowMyAdress().Result;         
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(miPosicion, Distance.FromKilometers(50));
                
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
                    mapaCargas.Pins.Add(origenPin);
                    mapaCargas.MoveToRegion(MapSpan.FromCenterAndRadius(miPosicion, Distance.FromKilometers(1)));
                    
                    //DisplayAlert("MM", allPines.Count.ToString(), "ok");                    
                }
                listaCargas.ItemsSource = solicitudes_origen.Select("id=1").AsEnumerable();

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

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 5;
                
                if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
                {
                    var position = await locator.GetPositionAsync(timeout: TimeSpan.FromSeconds(10));
                    Position pos = new Position(position.Latitude, position.Longitude);
                    //var locationAddress = (await (new Geocoder()).GetAddressesForPositionAsync(pos));
                    return pos;                    
                }
                else
                {
                    return new Position(-29.138486814739153, -59.64165222910585);
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                return new Position(-29.138486814739153, -59.64165222910585);
            }

        }

        
    }
}