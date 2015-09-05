using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Induccion
    {
        public int InduccionId { get; set; }
        public string observaciones { get; set; }
        public string nombre { get; set; }
        public int numeroEstudiantes { get; set; }
        public string frecuencia { get; set; }
        public string responsable { get; set; }
        public string correo { get; set; }
        public int valor { get; set; }
        public int IPS_ESEId { get; set; }
        public virtual IPS_ESE IPS_ESE { get; set; }

    } 
       
}