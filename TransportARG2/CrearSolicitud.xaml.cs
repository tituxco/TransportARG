using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportARG2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace TransportARG2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrearSolicitud : ContentPage
    {
        Geocoder geoCoder = new Geocoder();
        public CrearSolicitud()
        {
            InitializeComponent();
            cargarTiposCamiones();
            cargarTiposCargas();
            cargarTiposServicios();
            
            


        }

        private void cargarTiposServicios()
        {
            string error;
            conectarMysql conectar = new conectarMysql("kiremoto", "mecago");
            if (conectar.IntentarConectar(out error))
            {
                string consulta = "SELECT id, tipo FROM servicio_tipo";
                MySqlConnection conexionMysql = new MySqlConnection(conectar.builder.ToString());
                conexionMysql.Open();

                MySqlDataAdapter verServicioTipo = new MySqlDataAdapter(consulta, conexionMysql);
                DataTable serviciosTipo = new DataTable();
                verServicioTipo.Fill(serviciosTipo);
                conexionMysql.Close();
                List<tiposServicios> listaServicios = new List<tiposServicios >();                
                foreach (DataRow servicioTipo in serviciosTipo.Rows)
                {
                    tiposServicios servicio = new tiposServicios
                    {
                        id = int.Parse(servicioTipo["id"].ToString()),
                        tipo = servicioTipo["tipo"].ToString()
                    };
                    listaServicios.Add(servicio);
                }
                pckTiposServicios.ItemDisplayBinding = new Binding("tipo");
                pckTiposServicios.Title = "Seleccione el tipo de servicio para su carga";
                pckTiposServicios.ItemsSource = listaServicios;
                //DisplayAlert("items de picker", "items:" + listaCamiones.ItemsSource.Count+ "---_" + tt.Count +"****"+ camionesTipo.Rows.Count, "ok");                
                //formSolicitud.Children.Add(pickServicios);
            }
            else
            {
                DisplayAlert("ERROR", error, "ok");
            }
        }
        private void cargarTiposCargas()
        {
            string error;
            conectarMysql conectar = new conectarMysql("kiremoto", "mecago");
            if (conectar.IntentarConectar(out error))
            {
                string consulta = "SELECT id, tipo FROM carga_tipo";
                MySqlConnection conexionMysql = new MySqlConnection(conectar.builder.ToString());
                conexionMysql.Open();

                MySqlDataAdapter verCargasTipo = new MySqlDataAdapter(consulta, conexionMysql);
                DataTable cargasTipo = new DataTable();
                verCargasTipo.Fill(cargasTipo);
                conexionMysql.Close();
                List<tiposCargas> listaCargas = new List<tiposCargas>();
                //Picker pickCargas = new Picker()
                //{
                //    Title = "Seleccione tipo de carga a transportar",
                //    ItemDisplayBinding = new Binding("tipo"),
                //};
                foreach (DataRow cargaTipo in cargasTipo.Rows)
                {
                    tiposCargas carga = new tiposCargas
                    {
                        id = int.Parse(cargaTipo["id"].ToString()),
                        tipo = cargaTipo["tipo"].ToString()
                    };
                    listaCargas.Add(carga);
                }
                pckTiposCargas.ItemDisplayBinding = new Binding("tipo");
                pckTiposCargas.Title = "Seleccione tipo de carga a transportar";
                pckTiposCargas.ItemsSource = listaCargas;
                //DisplayAlert("items de picker", "items:" + listaCamiones.ItemsSource.Count+ "---_" + tt.Count +"****"+ camionesTipo.Rows.Count, "ok");                
                //formSolicitud.Children.Add(pickCargas);
            }
            else
            {
                DisplayAlert("ERROR", error, "ok");
            }
        }
        private void cargarTiposCamiones()
        {
            string error;
            conectarMysql conectar = new conectarMysql("kiremoto", "mecago");
            if (conectar.IntentarConectar(out error))
            {
                string consulta = "SELECT id, concat(familia,' - ',sub_familia) as tipo FROM camiones_tipos";
                MySqlConnection conexionMysql = new MySqlConnection(conectar.builder.ToString());
                conexionMysql.Open();

                MySqlDataAdapter verCamionesTipo = new MySqlDataAdapter(consulta, conexionMysql);
                DataTable camionesTipo = new DataTable();
                verCamionesTipo.Fill(camionesTipo);
                conexionMysql.Close();
                List<tiposCamiones> listaCamiones = new List<tiposCamiones>();
        
                foreach (DataRow camionTipo in camionesTipo.Rows)
                {
                    tiposCamiones camion = new tiposCamiones
                    {
                        id = int.Parse(camionTipo["id"].ToString()),
                        tipo = camionTipo["tipo"].ToString()
                    };
                    listaCamiones.Add(camion);
                }
                pckTiposCamiones.Title = "Seleccione tipo de camion";
                pckTiposCamiones.ItemDisplayBinding = new Binding("tipo");
                pckTiposCamiones.ItemsSource = listaCamiones;
                            
            }
            else
            {
                DisplayAlert("ERROR", error, "ok");
            }
        }

        //async void btnVisulizarRecorrido_Clicked(object sender, EventArgs e)
        //{
        //    //IEnumerable<Position> posicionOrigen = await geoCoder.GetPositionsForAddressAsync(txtDireccionOrigenCarga.Text);
        //    //IEnumerable<Position> posicionDestino = await geoCoder.GetPositionsForAddressAsync(txtDireccionDestinoCarga.Text);
        //    //Position DestinoCarga = posicionDestino.FirstOrDefault();
        //    //Position OrigenCarga = posicionOrigen.FirstOrDefault();
        //    //lblCoodenadasOrigen.Text = $"{OrigenCarga.Latitude}, {OrigenCarga.Longitude}";
        //    //lblCoodenadasDestino.Text = $"{DestinoCarga.Latitude}, {DestinoCarga.Longitude}";
        //    //Map map = new Map();
        //    //Pin origenPin = new Pin
        //    //{
        //    //    Position = OrigenCarga,
        //    //    Label = "ORIGEN DE LA CARGA,",
        //    //    Type = PinType.Place,
        //    //};
        //    //Pin destinoPin = new Pin
        //    //{
        //    //    Position = DestinoCarga,
        //    //    Label = "DESTINO DE LA CARGA",
        //    //    Type = PinType.Place,
        //    //};
        //    //map.Pins.Add(origenPin);
        //    //map.Pins.Add(destinoPin);
        //    //map.MoveToRegion(MapSpan.FromCenterAndRadius(OrigenCarga, Distance.FromKilometers(1)));
        //    //map.
        // //   mapaSolicitud.Children.Add(map);
        //}

        async  void btnAceptar_Clicked(object sender, EventArgs e)
        {
            try
            {


                IEnumerable<Position> posicionOrigen = await geoCoder.GetPositionsForAddressAsync(txtDireccionOrigenCarga.Text);
                IEnumerable<Position> posicionDestino = await geoCoder.GetPositionsForAddressAsync(txtDireccionDestinoCarga.Text);
                Position DestinoCarga = posicionDestino.FirstOrDefault();
                Position OrigenCarga = posicionOrigen.FirstOrDefault();

                string CoordenadasOrigen = $"{OrigenCarga.Latitude}, {OrigenCarga.Longitude}";
                string CoordenadasDestino= $"{DestinoCarga.Latitude}, {DestinoCarga.Longitude}";
                lblCoodenadasOrigen.Text = CoordenadasOrigen;
                lblCoodenadasDestino.Text = CoordenadasDestino;
                
                tiposCamiones camionSel = (tiposCamiones)pckTiposCamiones.ItemsSource[pckTiposCamiones.SelectedIndex];
                tiposCargas cargaSel = (tiposCargas)pckTiposCargas.ItemsSource[pckTiposCargas.SelectedIndex];
                tiposServicios servicioSel = (tiposServicios)pckTiposServicios.ItemsSource[pckTiposServicios.SelectedIndex];

                IEnumerable<string> direccionOrigenReal = await geoCoder.GetAddressesForPositionAsync(OrigenCarga);
                IEnumerable<string> direccionDestinoReal = await geoCoder.GetAddressesForPositionAsync(DestinoCarga);

                string[] DireccionOrigen = direccionOrigenReal.FirstOrDefault().ToString().Split(char.Parse(","));
                string[] DireccionDestino = direccionDestinoReal.FirstOrDefault().ToString().Split(char.Parse(","));

                string dir_origen = DireccionOrigen[0];
                string ciudad_origen = DireccionOrigen[1];
                string provincia_origen = DireccionOrigen[2];

                string dir_destino = DireccionDestino[0];
                string ciudad_destino = DireccionDestino[1];
                string provincia_destino = DireccionDestino[2];
                string comentarios_origen = txtComentariosOrigen.Text;
                string comentarios_destino = txtComentariosDescarga.Text;

                double peso_carga = double.Parse(txtPesoCarga.Text);
                double distancia_carga = double.Parse(txtCantidadKilometros.Text);
                double importe_carga = double.Parse(txtImporteCarga.Text);
                double peso_descarga = double.Parse(txtPesoDescarga.Text);



                int IDCargaOrigen=0;
                string error;
                conectarMysql conectar = new conectarMysql("kiremoto", "mecago");
                if (conectar.IntentarConectar(out error))
                {

                    string consulta = "INSERT INTO servicio_solicitud_origen " +
                        "(camion_tipo_id,servicio_tipo_id,carga_tipo_id,origen_provincia_id,origen_ciudad_id,origen_direccion," +
                        "origen_coordenadas,origen_comentarios,carga_peso,carga_importe,carga_distancia)" +
                        "values(?tipoCamion,?tipoServicio,?tipoCarga,?Provincia,?Ciudad,?DireccionOrigen," +
                        "?CoordenadasOrigen,?ComentariosOrigen,?PesoCarga,?ImporteCarga,?DistanciaCarga)";
                    MySqlConnection conexionMysql = new MySqlConnection(conectar.builder.ToString());
                    conexionMysql.Open();

                    MySqlCommand guardarDatosOrigen = new MySqlCommand(consulta, conexionMysql);
                    DataTable serviciosTipo = new DataTable();
                    guardarDatosOrigen.Parameters.AddWithValue("?tipoCamion", camionSel.id);
                    guardarDatosOrigen.Parameters.AddWithValue("?tipoServicio", servicioSel.id);
                    guardarDatosOrigen.Parameters.AddWithValue("?tipoCarga", cargaSel.id);
                    guardarDatosOrigen.Parameters.AddWithValue("?Provincia", provincia_origen);
                    guardarDatosOrigen.Parameters.AddWithValue("?Ciudad", ciudad_origen);
                    guardarDatosOrigen.Parameters.AddWithValue("?DireccionOrigen", dir_origen );
                    guardarDatosOrigen.Parameters.AddWithValue("?CoordenadasOrigen", CoordenadasOrigen);
                    guardarDatosOrigen.Parameters.AddWithValue("?ComentariosOrigen",comentarios_origen );
                    guardarDatosOrigen.Parameters.AddWithValue("?PesoCarga", peso_carga);
                    guardarDatosOrigen.Parameters.AddWithValue("?ImporteCarga", importe_carga);
                    guardarDatosOrigen.Parameters.AddWithValue("?DistanciaCarga", distancia_carga);

                    await guardarDatosOrigen.ExecuteNonQueryAsync();
                    IDCargaOrigen = int.Parse(guardarDatosOrigen.LastInsertedId.ToString());
                    conexionMysql.Close();                    
                }

                if (conectar.IntentarConectar(out error))
                {
                    string consulta = "INSERT INTO servicio_solicitud_destino(" +
                        "servicio_solicitud_id, destino_provincia_id, destino_ciudad_id, destino_direccion," +
                        "destino_coordenadas, destino_comentarios, descarga_peso) " +
                        "values(?IDSolicitudOrigen,?Provincia,?Ciudad,?DireccionDestino,?Coordenadas,?Comentarios," +
                        "?DescargaPeso)";
                    MySqlConnection conexionMysql = new MySqlConnection(conectar.builder.ToString());
                    conexionMysql.Open();

                    MySqlCommand guardarDatosDestino = new MySqlCommand(consulta, conexionMysql);
                    DataTable serviciosTipo = new DataTable();
                    guardarDatosDestino.Parameters.AddWithValue("?IDSolicitudOrigen", IDCargaOrigen);
                    guardarDatosDestino.Parameters.AddWithValue("?Provincia",provincia_destino);
                    guardarDatosDestino.Parameters.AddWithValue("?Ciudad", ciudad_destino);
                    guardarDatosDestino.Parameters.AddWithValue("?DireccionDestino", dir_destino);
                    guardarDatosDestino.Parameters.AddWithValue("?Coordenadas", CoordenadasDestino);
                    guardarDatosDestino.Parameters.AddWithValue("?Comentarios", comentarios_destino);
                    guardarDatosDestino.Parameters.AddWithValue("?DescargaPeso", peso_descarga);

                    await guardarDatosDestino.ExecuteNonQueryAsync();                    
                    conexionMysql.Close();
                }
                await DisplayAlert("Mensaje", "La solicitud se ingreso correctamente", "OK");
                foreach(var control in formSolicitud.Children)
                {
                    Type objeto = control.GetType();
                    if (objeto.Equals(typeof(Entry)))
                    {
                        Entry cajaTexto = (Entry)control;
                        cajaTexto.Text = "";
                    }
                    else if (objeto.Equals(typeof(Picker)))
                    {
                        Picker selector = (Picker)control;
                        selector.SelectedIndex = 0;
                    }
                    else if(objeto.Equals(typeof(Editor)))
                    {
                        Editor cajaTexto = (Editor)control;
                        cajaTexto.Text = "";
                    }                                     
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "OK");
            }
        }

        private void pckTiposCamiones_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var picker = (Picker)sender;
            //int selectedIndex = picker.SelectedIndex;
            //tiposCamiones camionSel = (tiposCamiones)picker.ItemsSource[selectedIndex];

            //DisplayAlert("mm",camionSel.id.ToString() ,"ok");
        }
        //async void CalcularCoordenadas(string direccion)
        //{
        //    IEnumerable<Position> posicionOrigen = await geoCoder.GetPositionsForAddressAsync(txtDireccionOrigenCarga.Text);
        //}
    }
    
}