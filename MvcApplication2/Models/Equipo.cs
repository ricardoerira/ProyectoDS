using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Equipo
    {
        public int equipoId { get; set; }
        public string nombre { get; set; }
        public string serial { get; set; }

        public int cantidad { get; set; }
        public string estado { get; set; }

        public int costo { get; set; }
        public string observaciones { get; set; }

        public string recibido { get; set; }

        public int IPS_ESEId { get; set; }
        public virtual IPS_ESE IPS_ESE { get; set; }

   
         
    }
}