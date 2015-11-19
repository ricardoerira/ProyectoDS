using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Curso
    {
        public int CursoId { get; set; }
        public string tipo { get; set; }
        public string nombre { get; set; }
        [Required]
        public int cupos { get; set; }

        [DataType(DataType.Date)]
        
        public DateTime fechaCreacion { get; set; }
        public string observaciones { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        
        public DateTime fechaInicio { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
       
        public DateTime fechaFin { get; set; }

        public int periodoAcademico { get; set; }
        public string dirigido { get; set; }
        public string dirigidoOtro { get; set; }
        [Required]
        public int asignados { get; set; }
        [Required]
        public int valorParticipante { get; set; }
        [Required]
        public int valorUnitarioPersona { get; set; }
        public int valorTotalPersona { get; set; }
        public double porcentajeTotalPersona { get; set; }
        public int valorTotalUniversidad { get; set; }
        [Required]
        public int valorUnitarioUniversidad { get; set; }
        public double porcentajeTotalUniversidad { get; set; }
        public int totalContraprestacion { get; set; }
        public int totalCapacitacion { get; set; }
        public string depto { get; set; }
        public string duracion { get; set; }
        public string lugar { get; set; }



        public int IPS_ESEId { get; set; }
        public virtual IPS_ESE IPS_ESE { get; set; }
        public virtual ICollection<nombreCurso> nombresCursos { get; set; }

    }

}