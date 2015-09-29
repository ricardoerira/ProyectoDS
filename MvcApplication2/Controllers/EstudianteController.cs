using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace MvcApplication2.Controllers
{
    public class EstudianteController : Controller
    {
        private UsersContext2 db = new UsersContext2();


        public ActionResult BuscarEnVacuna(Estudiante estudiante) 
        {
            var estudiantes = from b in db.Estudiantes
                              select b;

            foreach (var b in estudiantes)
            {
                if (b.num_documento.Equals(estudiante.num_documento) || b.codigo.Equals(estudiante.codigo))
                {
                    estudiante = b;
                }

            }
            if (estudiante.estudianteId == 0)
            {
                return View(estudiante);
            }
            else
            {
                return RedirectToAction("../Estudiante/CarnetVacunacionDS/" + estudiante.estudianteId);
            }
        }



public ActionResult BuscarEnDepartamento(Estudiante estudiante) 
        {
            var estudiantes = from b in db.Estudiantes
                              select b;

            foreach (var b in estudiantes)
            {
                if (b.codigo.Equals(estudiante.codigo))
                {
                    estudiante = b;
                }

            }
            if (estudiante.estudianteId == 0)
            {
                return View(estudiante);
            }
            else
            {
                return RedirectToAction("../Estudiante/ReporteEstudianteA/" + estudiante.estudianteId);
            }
        }

public ActionResult EstadoHV(string num_documento, string programaId, string estado_HV)
{
    int did = 0;
    bool b = false;
    if (!String.IsNullOrEmpty(programaId))
    {
        did = Int32.Parse(programaId);
        b = estado_HV.Equals("True") ? true : false;

    }

    var estudiantes = new List<Estudiante>();
    var estudiantesaux = new List<Estudiante>();
    if (!String.IsNullOrEmpty(num_documento))
    {
        estudiantes = db.Estudiantes.Where(s => s.num_documento.Equals(num_documento)).Where(s => s.programaId == did).Where(s => s.HojaVida.estado_HV == b).ToList();

    }
    else
    {
        if (!String.IsNullOrEmpty(programaId))
        {
            estudiantes = db.Estudiantes.Where(s => s.programaId == did).Where(s => s.HojaVida.estado_HV == b).ToList();
            estudiantesaux = db.Estudiantes.Where(s => s.programaId == did).ToList();
        
        }
    }






    ViewBag.busqueda = estudiantes.Count() + " / " + estudiantesaux.Count();
   

    ViewBag.Programas = new SelectList(db.Programas, "programaId", "nombre");

    return View(estudiantes.ToList());
}
        //
        // GET: /Estudiante/
        public ActionResult Buscar(Estudiante estudiante)
        {
            var estudiantes = from b in db.Estudiantes
                              select b;

            foreach (var b in estudiantes)
            {
                if (b.num_documento.Equals(estudiante.num_documento) || b.codigo.Equals(estudiante.codigo))
                {
                    estudiante = b;
                }

            }
            if (estudiante.estudianteId == 0)
            {
                return View(estudiante);
            }
            else
            {
                return RedirectToAction("../Estudiante/PersonalesDS/" + estudiante.estudianteId);
            }
        }

        public ActionResult Soporte(int id = 0)
        {
            string imagen = Request.Params["imagen"];
            imagen = imagen.Replace("%2F", "/");

            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            string[] documentos = { "doc_identidad"};



            ViewBag.imagen1 = imagen;


            return View(estudiante);
        }

        public ActionResult SoporteDS(int id = 0)
        {
            string imagen = Request.Params["imagen"];
            imagen = imagen.Replace("%2F", "/");

            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            string[] documentos = { "doc_identidad" };



            ViewBag.imagen1 = imagen;


            return View(estudiante);
        }
        public Boolean ValidarCampos(Estudiante estudiante)
        {
            HojaVida hv = db.HojaVidas.Find(estudiante.hojaVidaId);
            Estudiante e = db.Estudiantes.Find(estudiante.estudianteId);
            Familia f = db.Familias.Find(hv.familiaId);


            if ((hv.municipio_procedencia != null) && (hv.direccion_manizales != null) &&
                (hv.hemoclasificacion != "Sin Asignar") &&
                (hv.num_celular != 0) &&
                (hv.estado_civil != "Sin Asignar") && (f.primer_nombre_acudiente != null) &&
                (f.primer_apellido_acudiente != null) && (f.direccion_acudiente != null) &&
                (f.telefono_acudiente != 0))
            {
                Estudiante estudianteAux = db.Estudiantes.Find(estudiante.estudianteId);
                estudianteAux.HojaVida.estado_HV = true;

                db.Entry(estudianteAux).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }  

       
        public ActionResult Index()
        {
            var estudiantes = db.Estudiantes.Include(e => e.HojaVida).Include(e => e.Programa).Include(e => e.Rotacion);
            return View(estudiantes.ToList());
        }
        public ActionResult RotacionEstudiante(string searchString, int id = 0)
        {

            string rotacion = Request.Params["rotacion"];
            if (rotacion == null)
            {
                ViewBag.id = id;
            }
            else
            {
                Estudiante estudiante = db.Estudiantes.Find(id);
                estudiante.rotacionId = Convert.ToInt32(rotacion);
                Rotacion rotacionE = db.Rotacions.Find(Convert.ToInt32(rotacion));
                rotacionE.numero_estudiantes = rotacionE.numero_estudiantes + 1;
                estudiante.Rotacion = rotacionE;
                db.Entry(rotacionE).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(estudiante).State = EntityState.Modified;
                db.SaveChanges();

            }
            var estudiantes = from s in db.Estudiantes
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                int ss = Convert.ToInt32(searchString);
                estudiantes = estudiantes.Where(s => s.codigo == ss);
            }


            return View(estudiantes.ToList());
        }
       
        public ActionResult ReporteEstudiante(string searchString, int id = 0)
        {
            var estudiantes = from s in db.Estudiantes
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                int ss = Convert.ToInt32(searchString);
                estudiantes = estudiantes.Where(s => s.codigo == ss);
            }


            return View(estudiantes.ToList());
        }
        public ActionResult ReporteEstudianteA(int id = 0)
        {




            Estudiante estudiante = db.Estudiantes.Find(id);

            ReportDocument rptH = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/reporteEstudiante.rpt");
            rptH.Load(strRptPath);
            DateTime dt= DateTime.Now.Date.AddMonths(-20);
            var vacunas = db.Vacunas.Where(r => r.hojaVidaId == estudiante.hojaVidaId).Where(r => r.fecha_vacunacion >dt);
            List<Vacuna> listav = vacunas.ToList();
     
            rptH.Database.Tables[0].SetDataSource(listav);
     
            rptH.SetParameterValue("programa", estudiante.Programa.nombre);
            rptH.SetParameterValue("modalidad", estudiante.modalidad + "");
            rptH.SetParameterValue("semestre", estudiante.semestre + "");
            rptH.SetParameterValue("estadom", estudiante.estado_academico + "");



            rptH.SetParameterValue("tipodoc", estudiante.tipo_documento);
            rptH.SetParameterValue("numdoc", estudiante.num_documento);
            rptH.SetParameterValue("codigo", estudiante.codigo);
            rptH.SetParameterValue("genero", estudiante.HojaVida.genero + "");
            rptH.SetParameterValue("nombre", estudiante.HojaVida.primer_nombre+" "+estudiante.HojaVida.segundo_nombre);
            rptH.SetParameterValue("apellidos", estudiante.HojaVida.primer_apellido + " " + estudiante.HojaVida.segundo_apellido);
            rptH.SetParameterValue("fecha_nacimiento", estudiante.HojaVida.fecha_nacimiento + "");
            rptH.SetParameterValue("hemoclasificacion", estudiante.HojaVida.hemoclasificacion + "");
            rptH.SetParameterValue("dpto_procedencia", estudiante.HojaVida.departamento_procedencia + "");
            rptH.SetParameterValue("mun_procedencia", estudiante.HojaVida.municipio_procedencia + "");
            rptH.SetParameterValue("barrio_procedencia", estudiante.barrio_procedencia + "");
            rptH.SetParameterValue("dir_procedencia", estudiante.direccion_procedencia + "");
            rptH.SetParameterValue("dir_local", estudiante.HojaVida.direccion_manizales + "");
            rptH.SetParameterValue("image", estudiante.HojaVida.imagen_DI + "");
            rptH.SetParameterValue("edad", estudiante.barrio_procedencia + "");


            
            rptH.SetParameterValue("tel_proc", estudiante.HojaVida.num_telefono + "");
            rptH.SetParameterValue("tel_local", "");
            rptH.SetParameterValue("estado_civil", estudiante.HojaVida.estado_civil+"");
            rptH.SetParameterValue("num_hijos", estudiante.HojaVida.hijos + "");
            rptH.SetParameterValue("mail", estudiante.HojaVida.correo + "");

            rptH.SetParameterValue("num_cel", estudiante.HojaVida.num_celular + "");

            
            rptH.SetParameterValue("nombre_padre", estudiante.HojaVida.Familia.primer_nombre_padre + " " + estudiante.HojaVida.Familia.primer_apellido_padre + " " + estudiante.HojaVida.Familia.segundo_apellido_padre);
            rptH.SetParameterValue("direccion_padre", estudiante.HojaVida.Familia.direccion_padre + "");
            rptH.SetParameterValue("tel_padre", estudiante.HojaVida.Familia.telefono_padre + "");
            rptH.SetParameterValue("nombre_madre", estudiante.HojaVida.Familia.primer_nombre_madre + " " + estudiante.HojaVida.Familia.primer_apellido_madre + " " + estudiante.HojaVida.Familia.segundo_apellido_madre);
            rptH.SetParameterValue("direccion_madre", estudiante.HojaVida.Familia.direccion_madre + "");
            rptH.SetParameterValue("tel_madre", estudiante.HojaVida.Familia.telefono_madre + "");
            rptH.SetParameterValue("nombre_acudiente", estudiante.HojaVida.Familia.primer_nombre_acudiente + " " + estudiante.HojaVida.Familia.primer_apellido_acudiente + " " + estudiante.HojaVida.Familia.segundo_apellido_acudiente);
            rptH.SetParameterValue("direccion_acudiente", estudiante.HojaVida.Familia.direccion_acudiente + "");
            rptH.SetParameterValue("tel_acudiente", estudiante.HojaVida.Familia.telefono_acudiente + "");
          



            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return File(stream, "application/pdf");

        }
        public ActionResult RotacionEstudiante2(string searchString, int id = 0)
        {

            string rotacion = Request.Params["rotacion"];
            if (rotacion == null)
            {
                ViewBag.id = id;
            }
            else
            {
                Estudiante estudiante = db.Estudiantes.Find(id);
                estudiante.rotacionId = Convert.ToInt32(rotacion);
                Rotacion rotacionE = db.Rotacions.Find(Convert.ToInt32(rotacion));
                rotacionE.numero_estudiantes = rotacionE.numero_estudiantes + 1;
                estudiante.Rotacion = rotacionE;
                db.Entry(rotacionE).State = EntityState.Modified;
                db.SaveChanges();
                estudiante.Rotacion = rotacionE;
                db.Entry(estudiante).State = EntityState.Modified;
                db.SaveChanges();

            }


            return RedirectToAction("RotacionEstudiante/" + rotacion);
        }
       
      
        public ActionResult Details(int id = 0)
        {
            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);
        }



        //
        // GET: /Estudiante/Create

        public ActionResult Create()
        {
            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre");
            ViewBag.programaId = new SelectList(db.Programas, "programaId", "nombre");
            ViewBag.rotacionId = new SelectList(db.Rotacions, "rotacionId", "rotacionId");
            return View();
        }

        //
        // POST: /Estudiante/Create

        [HttpPost]


        [ValidateAntiForgeryToken]
        public ActionResult Create(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                
                db.Estudiantes.Add(estudiante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre", estudiante.hojaVidaId);
            ViewBag.programaId = new SelectList(db.Programas, "programaId", "nombre", estudiante.programaId);
            ViewBag.rotacionId = new SelectList(db.Rotacions, "rotacionId", "tipo", estudiante.rotacionId);
            return View(estudiante);
        }

        //
        // GET: /Estudiante/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre", estudiante.hojaVidaId);
            ViewBag.programaId = new SelectList(db.Programas, "programaId", "nombre", estudiante.programaId);
            ViewBag.rotacionId = new SelectList(db.Rotacions, "rotacionId", "tipo", estudiante.rotacionId);
            return View(estudiante);
        }

        //
        // POST: /Estudiante/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estudiante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre", estudiante.hojaVidaId);
            ViewBag.programaId = new SelectList(db.Programas, "programaId", "nombre", estudiante.programaId);
            ViewBag.rotacionId = new SelectList(db.Rotacions, "rotacionId", "tipo", estudiante.rotacionId);
            return View(estudiante);
        }

        //
        // GET: /Estudiante/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);
        }

        //
        // POST: /Estudiante/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estudiante estudiante = db.Estudiantes.Find(id);
            db.Estudiantes.Remove(estudiante);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        //
        //--------------------- METODOS PARA GESTIONAR LAS VISTAS DE LA-------------------
        //---------------------------- HOJA DE VIDA DE ESTUDIANTES-------------------


        //
        //--------------------- Vista para Logeo del estudiante

       
        
       
        public ActionResult Login()
        {

            return View();

        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Estudiante estudiante)
        {
            
            
            var b = db.Estudiantes.Where(s => s.codigo==estudiante.codigo).Where(s => s.clave==estudiante.clave);
            List<Estudiante> estudianteList = b.ToList();
           
          

            if (estudianteList.Count == 0)
            {

               return RedirectToAction("../Estudiante/Login/");
            }
            else 
            {
                Estudiante docente_aux = estudianteList.ElementAt(0);
          
                return RedirectToAction("../Estudiante/Personales/" + docente_aux.estudianteId);
            }
        }

        public ActionResult LoginCC(int id=0)
        {
            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginCC(Estudiante estudiante)
        {
            Estudiante estudianteReal = db.Estudiantes.Find(estudiante.estudianteId);
            if(estudiante.clave.Equals(estudianteReal.clave))
            {
                return RedirectToAction("../Estudiante/CambioContraseña/" + estudianteReal.estudianteId);  
            }
            else
            {

                return RedirectToAction("../Estudiante/LoginCC/" + estudianteReal.estudianteId);
                
            }
        }

        public ActionResult PaginaPrincipal()
        {
                return View();

            }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LoginCC(Estudiante estudiante)
        //{

        //    var b = db.Estudiantes.Where(s => s.num_documento.Equals(estudiante.num_documento)).Where(s => s.clave.Equals(estudiante.clave));
        //    List<Estudiante> estudianteList = b.ToList();
                       
            
        //    if (estudianteList.Count == 0)
        //    {

        //        return RedirectToAction("../Estudiante/LoginCC/");
        //    }
        //    else
        //    {
        //        Estudiante estudiante_aux = estudianteList.ElementAt(0);

        //        return RedirectToAction("../Estudiante/CambioContraseña/" + estudiante_aux.estudianteId);
        //    }
        //}

        public ActionResult CambioContraseña(int id = 0)
        {
            TempData["notice"] = null;

            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambioContraseña(Estudiante estudiante)
        {
                Estudiante est = db.Estudiantes.Find(estudiante.estudianteId);

                if (estudiante.clave.Equals(estudiante.tipo_documento))
                {
                    est.clave = estudiante.clave;


                    db.Entry(est).State = EntityState.Modified;



                    db.SaveChanges();
                    return RedirectToAction("../Estudiante/Personales/" + est.estudianteId);
                    
                }
            

            return RedirectToAction("../Estudiante/CambioContraseña/" + estudiante.estudianteId);
        }

        
        


        //
        //------------------------- Vista para datos personales del estudiante
        public ActionResult cargaImagen(Estudiante estudiante) {
            
            HojaVida oHojaVida = db.HojaVidas.Find(estudiante.hojaVidaId);
            try
            {
                var request = WebRequest.Create(oHojaVida.imagen_DI);
                using (var response = request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        // Process the stream
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError &&
                    ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        oHojaVida.imagen_DI = "http://www.tagetik.com/intouch2015/user.png";
                    }
                    else
                    {
                        // Do something else
                    }
                }
                else
                {
                    // Do something else
                }
            }
            if (estudiante == null)
            {
                return HttpNotFound();
            } return View(estudiante);
        }

        public ActionResult Personales(int id = 0)
        {
            TempData["notice"] = null;

            Estudiante estudiante = db.Estudiantes.Find(id);
            HojaVida oHojaVida = db.HojaVidas.Find(estudiante.hojaVidaId);
            cargaImagen(estudiante);
           
            int edad = DateTime.Today.AddTicks(-estudiante.HojaVida.fecha_nacimiento.Ticks).Year - 1;
            string edadDocente = edad.ToString();
            estudiante.barrio_procedencia = edadDocente;//Reemplaza edad

            cargaDocumentoDos(estudiante);
            return View(estudiante);
            
        }

        //metodo que muestra imagen
        public ActionResult cargaDocumentoDos(Estudiante estudiante) {
            string path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), "cedula_" + estudiante.codigo, ".jpg");
            if (System.IO.File.Exists(path1))
            {

                ViewBag.imagen = "/Uploads/cedula_" + estudiante.codigo + ".jpg";
            }
            else
            {
                ViewBag.imagen = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

            }
            return View(estudiante);
        }

        //metodo que guarda imagen
        public ActionResult cargaDocumento (Estudiante estudiante){ 
           if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["InputFile"];

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    string extension = System.IO.Path.GetExtension(Request.Files["InputFile"].FileName);
                    string path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"),"cedula_"+ estudiante.codigo, ".jpg");
                    if (System.IO.File.Exists(path1))
                        System.IO.File.Delete(path1);
                    ViewBag.imagen = "/Uploads/cedula_" + estudiante.codigo + ".jpg";
                    Request.Files["InputFile"].SaveAs(path1);
                }
            }
           return View(estudiante);
        }
                        
                     
       
                


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Personales(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
             
              HojaVida oHojaVida=  db.HojaVidas.Find(estudiante.hojaVidaId);
              Estudiante est = db.Estudiantes.Find(estudiante.estudianteId);

              oHojaVida.direccion_manizales = estudiante.HojaVida.direccion_manizales;
              oHojaVida.hemoclasificacion = estudiante.HojaVida.hemoclasificacion;
              oHojaVida.municipio_procedencia = estudiante.HojaVida.municipio_procedencia;
              oHojaVida.hijos = estudiante.HojaVida.hijos;
              oHojaVida.correo = estudiante.HojaVida.correo;
              oHojaVida.estado_civil = estudiante.HojaVida.estado_civil;
              oHojaVida.num_celular = estudiante.HojaVida.num_celular;
              oHojaVida.num_telefono = estudiante.HojaVida.num_telefono;
              oHojaVida.Familia = estudiante.HojaVida.Familia;

              int edad = DateTime.Today.AddTicks(-estudiante.HojaVida.fecha_nacimiento.Ticks).Year - 1;
              string edadDocente = edad.ToString();
              est.barrio_procedencia = edadDocente;//Reemplaza edad
              
              estudiante.HojaVida = null;

                db.Entry(est).State = EntityState.Modified;

                cargaDocumento(estudiante);

                db.SaveChanges();
               return RedirectToAction("../Estudiante/Personales/" + est.estudianteId);
               // return View(est);
            }
            else
            {
                cargaDocumentoDos(estudiante);
                Estudiante estudiante2 = db.Estudiantes.Find(estudiante.estudianteId);
                
                
                return View(estudiante2);
            }
           
        }

         

            
            
           
                       
               
            
         
    
        public String ImagePath(Estudiante estudiante)
        {
            
            {
                return "~/Uploads/img/cedula_"+estudiante.codigo+".jpg";
            }

        }


        //
        //------------------------- Vista para datos familia del estudiante

        public ActionResult Familiares(int id = 0)
        {

            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }

        public ActionResult FamiliaresDS(int id = 0)
        {

            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }

        public ActionResult PersonalesDS(int id = 0)
        {
            TempData["notice"] = null;

            Estudiante estudiante = db.Estudiantes.Find(id);
           
            cargaImagen(estudiante);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
           

            Boolean estado = ValidarCampos(estudiante);
            ViewBag.estado = estado;

            int edad = DateTime.Today.AddTicks(-estudiante.HojaVida.fecha_nacimiento.Ticks).Year - 1;
            string edadDocente = edad.ToString();
            estudiante.barrio_procedencia = edadDocente;//Reemplaza edad
            
           

            cargaDocumentoDos(estudiante);
            return View(estudiante);
            
        }




       


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SolicitarActualizacion(Estudiante estudiante)
        {
            estudiante = db.Estudiantes.Find(estudiante.estudianteId);

            var fromAddress = new MailAddress("docenciaservicioucaldas@hotmail.com", "Docencia Servicio Ucaldas");
            var toAddress = new MailAddress("mgliliana1028@gmail.com", "To Name");
            const string fromPassword = "ucaldas2015";
            const string subject = "Solicitud de Actualizacion ";
            const string body = "<h2>Coordial Saludo.</h2><h2 style=\"text-align: justify;\">La Oficina Docencia Servicio hace solicitud formal de la actualización de su hoja de vida así como su esquema de vacunación. Para ello disponemos de la nueva plataforma web la cual podrá acceder a través del siguiente enlace.</h2><h2>&nbsp;<a href=\"http://localhost:34649/Estudiante/Login\">http://localhost:34649/</a></h2><h2>Los datos de ingreso son:&nbsp;</h2><h2><strong>Usuario</strong>: Código de estudiante</h2><h2><strong>Contrase&ntilde;a</strong>: Código de estudiante&nbsp;</h2><p>&nbsp;</p><p>&nbsp;</p><p><img src=\"https://upload.wikimedia.org/wikipedia/commons/thumb/8/89/Universidad_De_Caldas_-_Logo.jpg/180px-Universidad_De_Caldas_-_Logo.jpg\" alt=\"\" width=\"180\" height=\"180\" /></p><p>&nbsp;</p><p>Copyright &copy; <a href=\"http://www.ucaldas.edu.co/portal\"><strong>Universidad de Caldas</strong></a> - Sede Principal Calle 65 No 26 - 10 / Tel +57 6 8781500 Fax 8781501 / Apartado a&eacute;reo 275 / L&iacute;nea gratuita : 01-8000-512120 E-mail ucaldas@ucaldas.edu.co</p> ";


            try
            {

                var smtp = new SmtpClient
                {
                    Host = "smtp.live.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Timeout = 10000,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                var message = new MailMessage(fromAddress, toAddress);

                message.IsBodyHtml = true;
                message.Subject = subject;
                message.Body = body;



                smtp.EnableSsl = true;
                smtp.Send(message);


            }


            catch (Exception e)
            {

                Console.WriteLine("Ouch!" + e.ToString());

            }


            return RedirectToAction("../Estudiante/PersonalesDS/" + estudiante.estudianteId);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonalesDS(Estudiante estudiante)
        {
             estudiante = db.Estudiantes.Find(estudiante.estudianteId);

             cargaDocumento(estudiante);


                Boolean estado = ValidarCampos(estudiante);
                ViewBag.estado = estado;
                //return View(estudiante);
                return RedirectToAction("../Estudiante/PersonalesDS/" + estudiante.estudianteId);
                
        }



       


        //
        //------------------------- Vista para datos academicos del estudiante

        public ActionResult Academicos(int id = 0)
        {

            Estudiante estudiante = db.Estudiantes.Find(id);
            cargaImagen(estudiante);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }

        public ActionResult AcademicosDS(int id = 0)
        {

            Estudiante estudiante = db.Estudiantes.Find(id);
            cargaImagen(estudiante);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }


        //
        //------------------------- Vista para datos de vacunacion del estudiante

        public ActionResult CarnetVacunacion(int id = 0)
        {
           

            Estudiante estudiante = db.Estudiantes.Find(id);
          
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.Details = estudiante.HojaVida.Vacunas;
            return View(estudiante);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CarnetVacunacionDS(Estudiante estudiante, int id = 0)
        {


            return RedirectToAction("../Vacuna/EsquemaVacunacion/" + estudiante.estudianteId);


        }
        




        public ActionResult CarnetVacunacionDS(int id = 0)
        {
            TempData["notice"] = null;

            Estudiante estudiante = db.Estudiantes.Find(id);
            HojaVida oHojaVida = db.HojaVidas.Find(estudiante.hojaVidaId);

            try
            {
                var request = WebRequest.Create(oHojaVida.imagen_DI);
                using (var response = request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        // Process the stream
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError &&
                    ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        oHojaVida.imagen_DI = "http://www.tagetik.com/intouch2015/user.png";
                    }
                    else
                    {
                        // Do something else
                    }
                }
                else
                {
                    // Do something else
                }
            }
            
            return View(estudiante);

        }



        public ActionResult CarnetVacunacionODS(int id = 0)
        {


            Estudiante estudiante = db.Estudiantes.Find(id);

            if (estudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.Details = estudiante.HojaVida.Vacunas;
            return View(estudiante);

        }

        //
        //------------------------- Vista para datos de salud del estudiante
        public ActionResult Salud(int id = 0)
        {
            string vacuna = Request.Params["vacuna"];
            Estudiante estudiante = db.Estudiantes.Find(id);
            cargaImagen(estudiante);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }

       

        public ActionResult SaludDS(int id = 0)
        {
            string vacuna = Request.Params["vacuna"];
            Estudiante estudiante = db.Estudiantes.Find(id);
            cargaImagen(estudiante);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }


        //
        //------------------------- METODO PARA CARGAR ARCHIVOS
        public ActionResult save()
        {
            int idestudiante = Int32.Parse(Request.Params["idEstudiante"]);
            Estudiante estudiante = db.Estudiantes.Find(idestudiante);

            if (estudiante == null)
            {
                return HttpNotFound();
            }  
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["InputFile"];

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    string extension = System.IO.Path.GetExtension(Request.Files["InputFile"].FileName);
                    string path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/"), "cedula_", extension);
                    if (System.IO.File.Exists(path1))
                        System.IO.File.Delete(path1);

                    Request.Files["InputFile"].SaveAs(path1);
                }
            }
            return RedirectToAction("../Estudiante/Personales/" + estudiante.estudianteId);

            // return View("Personales");
        }
    }

}