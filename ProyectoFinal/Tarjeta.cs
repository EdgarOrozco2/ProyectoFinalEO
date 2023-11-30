using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal
{
    public class Tarjeta
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string numero_tarjeta { get; set; }
        public string fecha_caducidad { get; set; }
        public string cvc { get; set; }
    }

    public enum TipoEncriptacion
    {
        SHA256 = 1,
        AES256 = 2
    }
}