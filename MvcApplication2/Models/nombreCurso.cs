using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class nombreCurso
    {
        public int nombreCursoId { get; set; }

        public string nombre { get; set; }//StateName

        public int CursoId { get; set; }

        public virtual Curso Curso { get; set; }
    }
}