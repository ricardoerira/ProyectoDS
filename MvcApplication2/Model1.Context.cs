﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcApplication2
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Estudiantes> Estudiantes { get; set; }
        public DbSet<HojaVidas> HojaVidas { get; set; }
        public DbSet<Rotacions> Rotacions { get; set; }
    
        [EdmFunction("Entities", "Funcion")]
        public virtual IQueryable<Funcion_Result> Funcion(Nullable<int> parameter1)
        {
            var parameter1Parameter = parameter1.HasValue ?
                new ObjectParameter("Parameter1", parameter1) :
                new ObjectParameter("Parameter1", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Funcion_Result>("[Entities].[Funcion](@Parameter1)", parameter1Parameter);
        }
    }
}
