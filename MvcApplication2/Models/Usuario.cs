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
    
    public partial class Usuario
    {
        public Usuario()
        {
         
        }
    
        public int usuarioId { get; set; }
        public string tipo_documento { get; set; }
        public string num_documento { get; set; }
        public string primer_nombre { get; set; }
        public string segundo_nombre { get; set; }
        public string primer_apellido { get; set; }
        public string segundo_apellido { get; set; }
        public System.DateTime fecha_nacimiento { get; set; }
        public string direccion { get; set; }
        public int telefono { get; set; }
        public int celular { get; set; }
        public string genero { get; set; }
        public string correo { get; set; }
        public string clave { get; set; }
        public string username { get; set; }
        public short rol { get; set; }
    
      
    }
}
