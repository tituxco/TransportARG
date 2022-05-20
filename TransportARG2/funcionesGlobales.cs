using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TransportARG2.Services;

namespace TransportARG2
{
    public class funcionesGlobales
    {
        public bool AutenticarUsuario(string correo, string contraseña)
        {
            string consulta = "select * from usuarios where correoElectronico like '" + correo + "' and password like '" + contraseña + "' limit 1";

            string error;
            conectarMysql conectar = new conectarMysql("kiremoto", "mecago");
            if (conectar.IntentarConectar(out error))
            {
                MySqlConnection conexionMysql = new MySqlConnection(conectar.builder.ToString());

                conexionMysql.Open();
                MySqlDataAdapter verUsuariosEncontrados = new MySqlDataAdapter(consulta, conexionMysql);
                DataTable usuariosEncontrados = new DataTable();
                verUsuariosEncontrados.Fill(usuariosEncontrados);
                conexionMysql.Close();
                if (usuariosEncontrados.Rows.Count > 0)
                {
                    UsuarioConectado.tipoUsuario = usuariosEncontrados.Rows[0]["tipodeUsuario"].ToString();
                    UsuarioConectado.usuarioNombre = usuariosEncontrados.Rows[0]["nomApellRazon"].ToString();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool GuardarNuevoUsuario(string nomapell, string cuitCuil, string direccion, string mail, string password, string tipoUsuario)
        {
            return false;
        }
    }
}
