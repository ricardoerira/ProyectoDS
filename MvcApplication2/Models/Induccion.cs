using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication2.Models
{
    public class Induccion
    {
        public int InduccionId { get; set; }
        public string observaciones { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public int numeroEstudiantes { get; set; }
        public string frecuencia { get; set; }
        public string responsable { get; set; }
        public string correo { get; set; }
        [Required]
        public int valor { get; set; }
        public int año { get; set; }
        public int periodo { get; set; }
        public int IPS_ESEId { get; set; }
        public virtual IPS_ESE IPS_ESE { get; set; }

    } 
       
}