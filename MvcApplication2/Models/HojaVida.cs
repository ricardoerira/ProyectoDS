﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcApplication2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class HojaVida
    {
        public HojaVida()
        {
            this.Vacunas = new HashSet<Vacuna>();




        }
    
        public int hojaVidaId { get; set; }
        public string primer_nombre { get; set; }
        public string segundo_nombre { get; set; }
        public string primer_apellido { get; set; }
        public string segundo_apellido { get; set; }
        public string facultad { get; set; }
        public string departamento_procedencia { get; set; }
        
        public string municipio_procedencia { get; set; }
        
        public string direccion_manizales { get; set; }
        public System.DateTime fecha_nacimiento { get; set; }
        public string hemoclasificacion { get; set; }
        public string genero { get; set; }
        public string estado_civil { get; set; }
        public short hijos { get; set; }
        public string imagen_DI { get; set; }        
        public string num_telefono { get; set; }
        [Required]
        public long num_celular { get; set; }        
        public string correo { get; set; }
        public string entidad_salud { get; set; }
        public string estado_afiliacion { get; set; }
        public string regimen { get; set; }
        public string tipo_afiliacion { get; set; }
        public bool estado_HV { get; set; }
        public string ARL { get; set; }
        public string certificado_ARL { get; set; }        
        public int familiaId { get; set; }

        public virtual ICollection<Vacuna> Vacunas { get; set; }

        public virtual ICollection<Docente> Docente { get; set; }
        public virtual ICollection<Estudiante> Estudiante { get; set; }
        public virtual Familia Familia { get; set; }
    }



    public class Profesor
    {



        public string CEDULA { get; set; }

        public string NOMBRE { get; set; }

       
        public class ESObject0
        {
            public List<Profesor> profesoresActivos { get; set; }
        }
       
    }
    public class Estudiante2
    {



        public string CODIGO { get; set; }

        public string NOMBRES { get; set; }

        public string NUM_DOC { get; set; }
        public string NOM_CARRERA { get; set; }

        
        public class ESObject0
        {
            public List<Estudiante2> estudiantesMatriculados { get; set; }
        }

    }
}
