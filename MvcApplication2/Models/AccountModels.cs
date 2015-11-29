using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace MvcApplication2.Models
{
    public class UsersContext2 : DbContext
    {

        public UsersContext2()
            : base("DefaultConnection")
            {}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          

           

        }


        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet <ActividadAcademica > ActividadAcademicas  { get;set  ; }
        public DbSet <DepartamentoSalud > DepartamentoSaluds  { get;set  ; }
        public DbSet <Departamento > Departamentoes  { get;set  ; }
        public DbSet <Docente > Docentes  { get;set  ; }
        public DbSet <Estudiante > Estudiantes  { get;set  ; }
        public DbSet <HojaVida > HojaVidas  { get;set  ; }
       public DbSet <IPS_ESE_Servicio > IPS_ESE_Servicio  { get;set  ; }
        public DbSet <IPS_ESE > IPS_ESE  { get;set  ; }
       public DbSet <Municipio > Municipios  { get;set  ; }
        public DbSet <Postgrado > Postgradoes  { get;set  ; }
        public DbSet <Programa > Programas  {get;set  ; }
        public DbSet <Rotacion > Rotacions  { get;set  ; }
        public DbSet <Servicio > Servicios  { get;set  ; }
        public DbSet <Usuario > Usuarios  { get;set  ; }
        public DbSet <Vacuna > Vacunas  { get;set  ; }

        public DbSet<Familia> Familias { get; set; }

        public DbSet<RotacionEstudiante> RotacionEstudiantes { get; set; }

        public DbSet<Equipo> Equipoes { get; set; }

        public DbSet<Induccion> Induccions { get; set; }

        public DbSet<Curso> Cursoes { get; set; }

        public DbSet<nombreCurso> nombreCursoes { get; set; }


    


    }

    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }

}

