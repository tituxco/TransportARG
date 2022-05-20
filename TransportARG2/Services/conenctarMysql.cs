using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;

namespace TransportARG2.Services
{
    public class conectarMysql
    {
        string usuario;
        string pass;
        public conectarMysql(string user,string password)
        {
            usuario = user;
            pass = password;
        }
        public MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
        public bool IntentarConectar(out string Error)
        {
            builder.Server="66.97.35.86";
            builder.Database = "cloud_transportARG";
            builder.UserID = "kiremoto";
            builder.Password = "mecago";
            builder.AllowUserVariables = true;
            try
            {
                MySqlConnection conexion = new MySqlConnection(builder.ToString());
                conexion.Open();
                Error = "";
                return true;
                
            }catch(Exception ex){
                Error = ex.Message;
                return false;
            }            
        }
        //public bool LoginUsuario(out string error)
        //{
        //    try
        //    {


        //    }catch (Exception ex)
        //    {
        //        error = ex.Message;
        //        return false;
        //    }

        //}
    }
}
