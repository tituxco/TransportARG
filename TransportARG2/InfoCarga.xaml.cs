using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using TransportARG2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace TransportARG2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoCarga : ContentPage
    {
        
        public InfoCarga(int IdCarga)
        {
            Geocoder geoCoder = new Geocoder();
            InitializeComponent();
            string error;
            conectarMysql conectar = new conectarMysql("kiremoto", "mecago");
            if (conectar.IntentarConectar(out error))
            {
                string consultaOrigen = "SELECT ori.id as id, concat(cam.familia, ' - ', cam.sub_familia) as tipoCamion, serv.tipo as tipoServicio, carg.tipo as cargaTipo, " +
                "ori.origen_provincia_id as provinciaOrigen, ori.origen_ciudad_id as ciudadOrigen, ori.origen_direccion as direccionOrigen, " +
                "ori.origen_coordenadas as origenCoordenadas, ori.origen_comentarios as origenComentarios, ori.carga_peso as origenPeso, " +
                "ori.carga_importe as origenImporte, ori.carga_distancia as origenDistancia " +
                "FROM servicio_solicitud_origen as ori, camiones_tipos as cam, carga_tipo as carg, servicio_tipo as serv " +
                "where cam.id = ori.camion_tipo_id and serv.id = ori.servicio_tipo_id and carg.id = ori.carga_tipo_id " +
                "and ori.id = " + IdCarga;

                string consultaDestino = "SELECT id, destino_provincia_id as provinciaDestino, destino_ciudad_id as ciudadDestino, destino_direccion as direccionDestino, " +
                "destino_coordenadas as coordenadasDestino, destino_comentarios as comentariosDestino, descarga_peso as destinoDescarga " +
                "FROM servicio_solicitud_destino where servicio_solicitud_id = " + IdCarga;

                MySqlConnection conexionMysql = new MySqlConnection(conectar.builder.ToString());

                conexionMysql.Open();
                MySqlDataAdapter verSolicitudesOrigen = new MySqlDataAdapter(consultaOrigen, conexionMysql);
                DataTable solicitudes_origen = new DataTable();
                verSolicitudesOrigen.Fill(solicitudes_origen);
                conexionMysql.Close();


                conexionMysql.Open();
                MySqlDataAdapter verSolicitudes_destino = new MySqlDataAdapter(consultaDestino, conexionMysql);
                DataTable solicitudes_destino = new DataTable();
                verSolicitudes_destino.Fill(solicitudes_destino);
                conexionMysql.Close();

                DataRow solicitudOrigen = solicitudes_origen.Rows[0];

                string[] CoordenadasOrigen = solicitudOrigen["origenCoordenadas"].ToString().Split(char.Parse(","));

                Position posicionOrigen = new Position(double.Parse(CoordenadasOrigen[0]), double.Parse( CoordenadasOrigen[1]));
                //Position miPosicion = ShowMyAdress().Result;         
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(posicionOrigen, Distance.FromKilometers(50));
                Map map = new Map(mapSpan);
                Pin origenPin = new Pin
                {
                    Position = posicionOrigen,
                    Label = "Origen de la carga",
                    Type = PinType.Generic
                };
                map.Pins.Add(origenPin);
                infoCarga.Children.Clear();
                Label textoOrigenCarga = new Label();
                textoOrigenCarga.Text += "Origen de la carga: " + solicitudOrigen["direccionOrigen"] + ", " + solicitudOrigen["ciudadOrigen"] + ", " + solicitudOrigen["provinciaOrigen"] + "\n";
                textoOrigenCarga.Text += "Camion solicitado: " + solicitudOrigen["tipoCamion"] + "\n" + "Tipo de carga a transportar: " + solicitudOrigen["cargaTipo"] + "\n";
                textoOrigenCarga.Text += "Servicio requerido: " + solicitudOrigen["tipoServicio"] + "\n";
                textoOrigenCarga.Text += "Peso a transportar: " + solicitudOrigen["origenPeso"] + "\n" + "Distancia declarada: " + solicitudOrigen["origenDistancia"] +"Km" + "\n";
                textoOrigenCarga.Text += "Importe: $" + solicitudOrigen["origenImporte"];
                infoCarga.Children.Add(textoOrigenCarga);

                foreach (DataRow solicitud_destino in solicitudes_destino.Rows)
                {
                    string[] coordenadas = solicitud_destino["coordenadasDestino"].ToString().Split(char.Parse(","));
                    Pin destinoPin = new Pin
                    {
                        Position = new Position(double.Parse(coordenadas[0]), double.Parse(coordenadas[1])),                        
                        Label = solicitud_destino["id"].ToString(),
                        Type = PinType.Place,
                    };
                    
                    map.Pins.Add(destinoPin);
                    
                    
                    Label comentarios = new Label();
                    comentarios.Text = solicitud_destino["comentariosDestino"].ToString();
                    infoCarga.Children.Add(comentarios);
                    
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(posicionOrigen, Distance.BetweenPositions(origenPin.Position, destinoPin.Position)));
                }
                
                infoCarga.Children.Add(map);
                
                conexionMysql.Close();
            }
            else
            {
                DisplayAlert("ERROR", error, "ok");
            }
            Label idCarga = new Label();            
            infoCarga.Children.Add(idCarga);           
        }
    }
}