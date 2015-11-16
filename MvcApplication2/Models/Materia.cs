using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Materia
    {


       
        public string COD_MATERIA { get; set; }
     
        public string NOM_MATERIA { get; set; }
       
        public string NOM_DEPTO { get; set; }
      
        public string PMO_NOMBRE { get; set; }
      
        public string GRUPOS_MAXIMO { get; set; }
         public string CUPOS_MINIMO { get; set; }
        public class ESObject0
        {
            public List<Materia> materias { get; set; }
        }
       
    }
}