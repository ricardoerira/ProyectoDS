using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Equipo
    {
        public int equipoId { get; set; }

        [Required]
        public string nombre { get; set; }
        [Required]
        public string serial { get; set; }

        [Required]
        public int cantidad { get; set; }

        
        public string estado { get; set; }

        [Required]
        public int costo { get; set; }
        
        public string observaciones { get; set; }

        public string responsable { get; set; }



        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime fechaPrestamo { get; set; }
        public int IPS_ESEId { get; set; }
        public virtual IPS_ESE IPS_ESE { get; set; }

   
         
    }
}