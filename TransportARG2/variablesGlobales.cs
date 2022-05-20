using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace TransportARG2
{
    public class variablesGlobales
    {
        public int UsuarioAutorizado = 1;
    }
    public class UsuarioConectado
    {
        public static string tipoUsuario { get; set; }
        public static string usuarioNombre { get; set; }
        public static string usuarioPermisos { get; set; }


    }

}
