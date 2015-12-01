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
using System.Net.Mail;
using System.Net;
using WebMatrix.WebData;

namespace MvcApplication2.Controllers
{
    public class IPS_ESEController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //C:\Users\Lili\Desktop\ProyectoDS\ProyectoDS\MvcApplication2\Controllers\IPS_ESEController.cs
        // GET: /IPS_ESE/

        public ActionResult Index()
        {
            
            var ips_ese = db.IPS_ESE.Include(i => i.Municipio);
            return View(ips_ese.ToList());
        }

        [AllowAnonymous]
        public ActionResult RegistroEPS()
        {
            
            var municipios = db.Municipios.Include(h => h.Departamento);
            List<Municipio> lista = municipios.ToList();

            ViewBag.municipioId = new SelectList(lista, "municipioId", "nombre");


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistroEPS(IPS_ESE ips_ese)
        {
            if (ModelState.IsValid)
            {
                if(ips_ese.pass.Equals(ips_ese.passC))
                {
                    db.IPS_ESE.Add(ips_ese);
                    if (!WebSecurity.Initialized)
                    {
 
                        WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
           
                    }
                       
                    WebSecurity.CreateUserAndAccount(ips_ese.user, ips_ese.passC);
                    db.SaveChanges();

                    if (Request != null)
                    {


                        int uploadedCount = 0;
                        string[] documentos = { "resolucion", "cedularl", "actap", "rut", "habilitacion" };
                        int numFiles = Request.Files.Count;
                 
                        for (int i = 0; i < numFiles; i++)
                        {

                            HttpPostedFileBase file = Request.Files[i];
                            if (file.ContentLength > 0)
                            {
                                string fileName = file.FileName;
                                string fileContentType = file.ContentType;
                                byte[] fileBytes = new byte[file.ContentLength];
                                file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                                string path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[i] + ips_ese.IPS_ESEId , ".jpg");
                                if (System.IO.File.Exists(path1))
                                    System.IO.File.Delete(path1);

                                file.SaveAs(path1);
                                uploadedCount++;
                            }
                        }
                    }
                    
                    return RedirectToAction("RegistroEPS");

                }
                else
                {
                    cargaDocumentos(ips_ese);
                    var municipios = db.Municipios.Include(h => h.Departamento);
                    List<Municipio> lista = municipios.ToList();

                    ViewBag.municipioId = new SelectList(lista, "municipioId", "nombre");
                    ViewBag.AlertMessage = "Las contrasenias deben de coincidir";
                    return View();
                }
              
            }

            ViewBag.municipioId = new SelectList(db.Municipios, "municipioId", "nombre", ips_ese.municipioId);
            return View(ips_ese);
        }



        public ActionResult cargaDocumentos(IPS_ESE ips_ese)
        {
            string[] documentos = { "resolucion", "cedularl", "actap", "rut", "habilitacion" };


            string path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[0] + ips_ese.IPS_ESEId, ".jpg");
            if (System.IO.File.Exists(path1))
            {
                path1 = string.Format("{0}/{1}{2}", "http://salud.ucaldas.edu.co/Proyecto/Uploads", documentos[0] + ips_ese.IPS_ESEId, ".jpg");

                ViewBag.imagen1 = path1;

            }
            else
            {
                ViewBag.imagen1 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

            }


            path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[1] + ips_ese.IPS_ESEId, ".jpg");

            if (System.IO.File.Exists(path1))
            {

                path1 = string.Format("{0}/{1}{2}", "http://salud.ucaldas.edu.co/Proyecto/Uploads", documentos[1] + ips_ese.IPS_ESEId, ".jpg");
                ViewBag.imagen2 = path1;
            }
            else
            {
                ViewBag.imagen2 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

            }


            path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[2] + ips_ese.IPS_ESEId, ".jpg");

            if (System.IO.File.Exists(path1))
            {

                path1 = string.Format("{0}/{1}{2}", "http://salud.ucaldas.edu.co/Proyecto/Uploads", documentos[2] + ips_ese.IPS_ESEId, ".jpg");
                ViewBag.imagen3 = path1;

            }
            else
            {
                ViewBag.imagen3 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

            }



            path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[3] + ips_ese.IPS_ESEId, ".jpg");

            if (System.IO.File.Exists(path1))
            {

                path1 = string.Format("{0}/{1}{2}", "http://salud.ucaldas.edu.co/Proyecto/Uploads", documentos[3] + ips_ese.IPS_ESEId, ".jpg");
                ViewBag.imagen4 = path1;

            }
            else
            {
                ViewBag.imagen4 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

            }




            path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[4] + ips_ese.IPS_ESEId, ".jpg");

            if (System.IO.File.Exists(path1))
            {

                path1 = string.Format("{0}/{1}{2}", "http://salud.ucaldas.edu.co/Proyecto/Uploads", documentos[4] + ips_ese.IPS_ESEId, ".jpg");
                ViewBag.imagen5 = path1;

            }
            else
            {
                ViewBag.imagen5 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

            }



            //path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[5] + docente.num_documento, ".jpg");

            //if (System.IO.File.Exists(path1))
            //{

            //    ViewBag.imagen6 = "/Uploads/" + documentos[5] + docente.num_documento + ".jpg";

            //}
            //else
            //{
            //    ViewBag.imagen6 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

            //}



            //path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[6] + docente.num_documento, ".jpg");

            //if (System.IO.File.Exists(path1))
            //{

            //    ViewBag.imagen7 = "/Uploads/" + documentos[6] + docente.num_documento + ".jpg";

            //}
            //else
            //{
            //    ViewBag.imagen7 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

            //}



            //path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[7] + docente.num_documento, ".jpg");

            //if (System.IO.File.Exists(path1))
            //{

            //    ViewBag.imagen8 = "/Uploads/" + documentos[7] + docente.num_documento + ".jpg";

            //}
            //else
            //{
            //    ViewBag.imagen8 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

            //}





            return View(ips_ese);


        }


       
        

        public ActionResult VistaIPS_ESE()
        {
            bool estado = User.IsInRole("IPS");
            if (!estado)
            {
                return RedirectToAction("../Account/Login");
            }
            else
            {
                return View();


            }

        }
        public ActionResult SeleccionRotacionCarta()
        {

            var municipios = db.IPS_ESE.Include(h => h.Municipio);
            List<IPS_ESE> lista = municipios.ToList();
            
            ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");
            
            ViewBag.programaId = new SelectList(db.Programas, "programaId", "nombre");

            ViewBag.DepartamentoSaludId = new SelectList(db.DepartamentoSaluds, "DepartamentoSaludId", "nombre");
            

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeleccionRotacionCarta(IPS_ESE s,FormCollection value)
        {

            IPS_ESE ips = db.IPS_ESE.Find(s.IPS_ESEId);

            int programaId =Int32.Parse( value["programaId"]);
            int departamentoId = Int32.Parse(value["DepartamentoSaludId"]);
            DepartamentoSalud ds = db.DepartamentoSaluds.Find(departamentoId);
            Programa pr= db.Programas.Find(programaId);
            int mesId = Int32.Parse(value["mesId"]);
            int añoId = Int32.Parse(value["añoId"]);
            var date = DateTime.MinValue;
           DateTime.TryParse(añoId + "/" +mesId+ "/01", out date);
         
           DateTime date2 = new DateTime(añoId, mesId,
                                     DateTime.DaysInMonth(añoId, mesId));
            ReportDocument rptH = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/reporte.rpt");
            rptH.Load(strRptPath);
            List<RotacionEstudiante> re = db.RotacionEstudiantes.Include(h => h.Docente).Where(r => r.IPS_ESEId == ips.IPS_ESEId).Where(r => r.Estudiante.programaId == programaId).Where(r => r.Rotacion.fecha_inicio >= date).Where(r => r.Rotacion.fecha_terminacion <= date2).Where(r=>r.Rotacion.ActividadAcademica.DepartamentoSaludId==departamentoId).ToList();
            List<Docente> docentes = new List<Docente>();
            List<Estudiante> estudiantes = new List<Estudiante>();
            List<Rotacion> rotaciones = new List<Rotacion>();
            List<ActividadAcademica> acti = new List<ActividadAcademica>();
            List<HojaVida> hojas = new List<HojaVida>();
            List<HojaVida> hojas2 = new List<HojaVida>();
            foreach (var item in re)
            {
                docentes.Add(item.Docente);
                hojas.Add(item.Docente.HojaVida);
                estudiantes.Add(item.Estudiante);
                hojas2.Add(item.Estudiante.HojaVida);
                rotaciones.Add(item.Rotacion);
                acti.Add(item.Rotacion.ActividadAcademica);
            }

            rptH.Database.Tables[0].SetDataSource(re);
            rptH.Database.Tables[1].SetDataSource(docentes);
            rptH.Database.Tables[2].SetDataSource(estudiantes);
            rptH.Database.Tables[3].SetDataSource(hojas2);
            rptH.Database.Tables[4].SetDataSource(acti);
            rptH.Database.Tables[5].SetDataSource(rotaciones);
           



            rptH.SetParameterValue("presentacion", "A continuación le relaciono las rotaciones de los estudiantes del Programa de "+pr.nombre+" Departamento "+ds.nombre+" que realizaran su rotación en su institución y los profesores con su horario.");
            rptH.SetParameterValue("fecha", "");
            rptH.SetParameterValue("dr", ips.representante);
            rptH.SetParameterValue("cargo",ips.cargo); 
            rptH.SetParameterValue("nombreIPS", ips.nombre);
            




            if(re.Count>0)
            {
                EnviarEstudiantes(estudiantes, docentes);
                Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                SaveStreamToFile(stream,"cartaPresentacion");
                Stream stream2 = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
               
                return File(stream2, "application/pdf");
            }
            else
            {
                var municipios = db.IPS_ESE.Include(h => h.Municipio);
                List<IPS_ESE> lista = municipios.ToList();

                ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");

                ViewBag.programaId = new SelectList(db.Programas, "programaId", "nombre");

                ViewBag.DepartamentoSaludId = new SelectList(db.DepartamentoSaluds, "DepartamentoSaludId", "nombre");
            
                ViewBag.AlertMessage = "No se encontraron resultados";
                return View();
            }

         
        }

        public void EnviarEstudiantes(List<Estudiante> estudiantes, List<Docente> docentes)
        {
            string body = "<h2>Coordial Saludo.</h2><h2 style=\"text-align: justify;\">Se envía carta de presentación con sus respectivas hojas de vida</h2>";
            body += "<h2>Estudiantes</h2>";
            foreach(Estudiante estudiante in estudiantes)
            {
                body += "&nbsp;<a href=\"http://localhost:34649/Estudiante/ReporteEstudianteA/" + estudiante.estudianteId + "\">" + estudiante.num_documento + "</a>";
            }
            body += "<h2>Docentes</h2>";

            foreach (Docente docente in docentes.Distinct())
            {
                body += "&nbsp;<a href=\"http://localhost:34649/Docente/ReporteDocenteA/" + docente.docenteId + "\">" + docente.num_documento + "</a>";
            }
            body += "<p><img src=\"https://upload.wikimedia.org/wikipedia/commons/thumb/8/89/Universidad_De_Caldas_-_Logo.jpg/180px-Universidad_De_Caldas_-_Logo.jpg\" alt=\"\" width=\"180\" height=\"180\" /></p><p>&nbsp;</p><p>Copyright &copy; <a href=\"http://www.ucaldas.edu.co/portal\"><strong>Universidad de Caldas</strong></a> - Sede Principal Calle 65 No 26 - 10 / Tel +57 6 8781500 Fax 8781501 / Apartado a&eacute;reo 275 / L&iacute;nea gratuita : 01-8000-512120 E-mail ucaldas@ucaldas.edu.co</p>";






            var fromAddress = new MailAddress("info@salud.ucaldas.edu.co", "Decanatura – Oficina Docencia Servicio");
            var toAddress = new MailAddress("ricardoerira@gmail.com", "To Name");
            const string fromPassword = "descargar";
            const string subject = "Carta de presentación";
                

                try
                {

                    var smtp = new SmtpClient
                    {
                        Host = "72.29.75.91",
                        Port = 25,
                        EnableSsl = false,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Timeout = 10000,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    var message = new MailMessage(fromAddress, toAddress);

                    message.IsBodyHtml = true;
                    message.Subject = subject;
                    message.Body = body;
                    string file = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), "CartaPresentacion", ".pdf");

                    message.Attachments.Add(new System.Net.Mail.Attachment(file));


                    smtp.EnableSsl = false;
                    smtp.Send(message);


                }


                catch (Exception e)
                {

                    Console.WriteLine("Ouch!" + e.ToString());

                }

        
        
        }
       
        public void SaveStreamToFile(Stream stream, string filename)
        {
            string path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), filename, ".pdf");
            try
            {
                var fileStream = new FileStream(path1, FileMode.CreateNew, FileAccess.Write);
                stream.CopyTo(fileStream);
                fileStream.Dispose();
                fileStream.Flush();
                fileStream.Close();
            }
            catch(Exception e)
            {

            }
          
        }
       

        //Typically I implement this Write method as a Stream extension method. 
        //The framework handles buffering.

        public void Write(Stream from, Stream to)
        {
            for (int a = from.ReadByte(); a != -1; a = from.ReadByte())
                to.WriteByte((byte)a);
            to.Flush();
            to.Close();
            from.Flush();
            to.Flush();


         
        }
        
        public ActionResult SeleccionRotacionContraPrestacionC()
        {

            List<IPS_ESE> lista = null;
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("IPS"))
                {
                    int res = consultaIPS(User.Identity.Name);

                    var municipios = db.IPS_ESE.Include(h => h.Municipio).Where(r => r.IPS_ESEId == res);

                    lista = municipios.ToList();



                }
                else
                {
                    var municipios = db.IPS_ESE.Include(h => h.Municipio);
                    lista = municipios.ToList();


                }


            }
            else
            {

                var municipios = db.IPS_ESE.Include(h => h.Municipio);
                lista = municipios.ToList();

            }
            ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeleccionRotacionContraPrestacionC(IPS_ESE ipss, FormCollection value)
        {

            IPS_ESE ips = db.IPS_ESE.Find(ipss.IPS_ESEId);
            string fecha = ViewBag.fecha;


            List<Curso> cursos = new List<Curso>();
            
            ReportDocument rptH = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/ReporteContraPrestacionC.rpt");
            rptH.Load(strRptPath);

            int mesId = Int32.Parse(value["mesId"]);
            int añoId = Int32.Parse(value["añoId"]);
            var date = DateTime.MinValue;
           
            if(mesId==13)
            {
                DateTime.TryParse(añoId + "/01/01", out date);
                DateTime date2 = new DateTime(añoId, 12,
                                     DateTime.DaysInMonth(añoId, 12));
                cursos = db.Cursoes.Where(r => r.IPS_ESEId == ips.IPS_ESEId).Where(r => r.fechaInicio >= date).Where(r => r.fechaFin <= date2).ToList();
          
            }
            else 
            {
                DateTime.TryParse(añoId + "/" + mesId + "/01", out date);

                DateTime date2 = new DateTime(añoId, mesId,
                                          DateTime.DaysInMonth(añoId, mesId));

                cursos = db.Cursoes.Where(r => r.IPS_ESEId == ips.IPS_ESEId).Where(r => r.fechaInicio >= date)
                    .Where(r => r.fechaFin <= date2 ).ToList();
       
            }
          
           if(cursos.Count>0)
           {
               rptH.Database.Tables[0].SetDataSource(cursos);


               rptH.SetParameterValue("ips", ips.nombre);
               rptH.SetParameterValue("email", ips.correo);
               rptH.SetParameterValue("representante", ips.representante);

               if(mesId==13)
               {
                   rptH.SetParameterValue("fecha",  "Año: " + añoId);
              
               }
               else
               {
                   rptH.SetParameterValue("fecha", "Mes: " + mesId + " Año: " + añoId);
              
               }
               int total = 0;

               if (cursos.Count > 0)
               {
                   total += cursos.Sum(d => d.totalContraprestacion);

               }

               rptH.SetParameterValue("total", total);





               Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);




               return File(stream, "application/pdf");
           }
           else
           {
               ViewBag.AlertMessage = "No se encontraron resultados";

               var municipios = db.IPS_ESE.Include(h => h.Municipio);
               List<IPS_ESE> lista = municipios.ToList();
               ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");

               return View();
           }
            
          
        }


        public ActionResult SeleccionRotacionContraPrestacionI()
        {
            List<IPS_ESE> lista = null;
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("IPS"))
                {
                    int res = consultaIPS(User.Identity.Name);

                    var municipios = db.IPS_ESE.Include(h => h.Municipio).Where(r => r.IPS_ESEId == res);

                    lista = municipios.ToList();



                }
                else
                {
                    var municipios = db.IPS_ESE.Include(h => h.Municipio);
                    lista = municipios.ToList();


                }


            }
            else
            {

                var municipios = db.IPS_ESE.Include(h => h.Municipio);
                lista = municipios.ToList();

            }
            ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeleccionRotacionContraPrestacionI(IPS_ESE ipss, FormCollection value)
        {

            IPS_ESE ips = db.IPS_ESE.Find(ipss.IPS_ESEId);
            string fecha = ViewBag.fecha;


            List<Induccion> inducciones = new List<Induccion>();
          
            ReportDocument rptH = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/ReporteContraPrestacionI.rpt");
            rptH.Load(strRptPath);

            int periodoId = Int32.Parse(value["periodoId"]);
            int añoId = Int32.Parse(value["añoId"]);
            var date = DateTime.MinValue;
           

           inducciones=  db.Induccions.Where(r => r.IPS_ESEId == ips.IPS_ESEId).Where(r => r.periodo == periodoId)
                    .Where(r => r.año == añoId ).ToList();

            rptH.Database.Tables[0].SetDataSource(inducciones);

           

            rptH.SetParameterValue("ips", ips.nombre);
            rptH.SetParameterValue("fecha", "");
            rptH.SetParameterValue("representante", ips.representante);
            rptH.SetParameterValue("correo", ips.correo);

            


                rptH.SetParameterValue("fecha", "Periodo: " + periodoId + " Año: " + añoId);

            
            //rptH.SetParameterValue("correo", ips.correo);

            int total = 0;
           
            if (inducciones.ToList().Count > 0)
            {
                total += inducciones.Sum(d => d.valor);
                rptH.SetParameterValue("total", total);

                 Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                
                return File(stream, "application/pdf");
            }
            else
            {
                ViewBag.AlertMessage = "No se encontraron resultados";

             
                var municipios = db.IPS_ESE.Include(h => h.Municipio);
                List<IPS_ESE> lista = municipios.ToList();
                ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");

                return View();
            }


          

        }



        //--------------SeleccionRotacionContraPrestacionPeriodoCurso  1

        public ActionResult SeleccionRotacionContraPrestacionPeriodoCurso()
        {
            List<IPS_ESE> lista = null;
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("IPS"))
                {
                    int res = consultaIPS(User.Identity.Name);

                    var municipios = db.IPS_ESE.Include(h => h.Municipio).Where(r => r.IPS_ESEId == res);

                    lista = municipios.ToList();



                }
                else
                {
                    var municipios = db.IPS_ESE.Include(h => h.Municipio);
                    lista = municipios.ToList();


                }


            }
            else
            {

                var municipios = db.IPS_ESE.Include(h => h.Municipio);
                lista = municipios.ToList();

            }
            ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");

            return View();
        }


        //---------- //--------------SeleccionRotacionContraPrestacionPeriodoCurso 2

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeleccionRotacionContraPrestacionPeriodoCurso(IPS_ESE ipss, FormCollection value)
        {

            IPS_ESE ips = db.IPS_ESE.Find(ipss.IPS_ESEId);
            string fecha = ViewBag.fecha;


            List<Curso> cursos = new List<Curso>();
            ReportDocument rptH = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/ReporteContraPrestacionPeriodoCurso.rpt");
            rptH.Load(strRptPath);

            int periodoId = Int32.Parse(value["periodoId"]);
            var date = DateTime.MinValue;

            cursos = db.Cursoes.Where(r => r.IPS_ESEId == ips.IPS_ESEId).Where(r => r.periodoAcademico == periodoId).ToList();
            

            rptH.Database.Tables[0].SetDataSource(cursos);


            rptH.SetParameterValue("ips", ips.nombre);
            rptH.SetParameterValue("email", ips.correo);
            rptH.SetParameterValue("representante", ips.representante);




            rptH.SetParameterValue("fecha", "Periodo: " + periodoId + " Año: " );


            int total = 0;

            if (cursos.ToList().Count > 0)
            {
                total += cursos.Sum(d => d.totalContraprestacion);
                rptH.SetParameterValue("total", total);

                Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                return File(stream, "application/pdf");
            }
            else
            {
                ViewBag.AlertMessage = "No se encontraron resultados";


                var municipios = db.IPS_ESE.Include(h => h.Municipio);
                List<IPS_ESE> lista = municipios.ToList();
                ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");

                return View();
            }




        }
       public int consultaIPS(string nombre)
       
       {
      
           if(User.Identity.Name.Equals("hdecaldas"))
      
           {

               return 1;
           }
           if (User.Identity.Name.Equals("cversalles"))
           {

               return 2;
           }
           if (User.Identity.Name.Equals("hsantasofia"))
           {

               return 15;
           }
           return -1;

       }
        public ActionResult SeleccionRotacionContraPrestacionE()
        {

            List<IPS_ESE> lista =null;
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("IPS"))
                {
                    int res=consultaIPS(User.Identity.Name);

                        var municipios = db.IPS_ESE.Include(h => h.Municipio).Where(r => r.IPS_ESEId == res);
                        
                        lista = municipios.ToList();
                    
                    
           
                }
                else
                {
                    var municipios = db.IPS_ESE.Include(h => h.Municipio);
                    lista = municipios.ToList();


                }
               
              
            }
            else
            {

               var municipios = db.IPS_ESE.Include(h => h.Municipio);
               lista = municipios.ToList();
           
            }
            ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeleccionRotacionContraPrestacionE(IPS_ESE ipss, FormCollection value)
        {

            IPS_ESE ips = db.IPS_ESE.Find(ipss.IPS_ESEId);
            string fecha = ViewBag.fecha;
            List<Equipo> equipos = new List<Equipo>();

            int mesId = Int32.Parse(value["mesId"]);
            int añoId = Int32.Parse(value["añoId"]);
            var date = DateTime.MinValue;

            if (mesId == 13)
            {
                DateTime.TryParse(añoId + "/01/01", out date);
                DateTime date2 = new DateTime(añoId, 12,
                                     DateTime.DaysInMonth(añoId, 12));
                equipos = db.Equipoes.Where(r => r.IPS_ESEId == ips.IPS_ESEId).Where(r => r.fechaPrestamo >= date).Where(r => r.fechaPrestamo <= date2).ToList();

            }
            else
            {
                DateTime.TryParse(añoId + "/" + mesId + "/01", out date);
                DateTime date2 = new DateTime(añoId, mesId,
                                          DateTime.DaysInMonth(añoId, mesId));
                                equipos = db.Equipoes.Where(r => r.IPS_ESEId == ips.IPS_ESEId).Where(r => r.fechaPrestamo >= date).Where(r => r.fechaPrestamo <= date2).ToList();
            }

          
                      
            if (equipos.ToList().Count > 0)
            {

            ReportDocument rptH = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/ReporteEquipos.rpt");
            rptH.Load(strRptPath);



            int total = equipos.Sum((c => c.costo));
            rptH.Database.Tables[0].SetDataSource(equipos.ToList());
            if (mesId == 13)
            {
                rptH.SetParameterValue("fecha", "Año: " + añoId);

            }
            else
            {
                rptH.SetParameterValue("fecha", "Mes: " + mesId + " Año: " + añoId);

            }
          
            rptH.SetParameterValue("ips", ips.nombre);
            rptH.SetParameterValue("total", total+"");
            rptH.SetParameterValue("correo",ips.correo);
            rptH.SetParameterValue("recibido",ips.representante);






            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);




            return File(stream, "application/pdf");
                
            }
                else
            {
                ViewBag.AlertMessage = "No se encontraron resultados";

                var municipios = db.IPS_ESE.Include(h => h.Municipio);
                List<IPS_ESE> lista = municipios.ToList();
                ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");

                return View();
            }

        }
        public ActionResult GeneraReporte(int id = 0)
        {

            IPS_ESE ips = db.IPS_ESE.Find(id);

          
            
            ReportDocument rptH = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/reporte.rpt");
            rptH.Load(strRptPath);


            rptH.Database.Tables[0].SetDataSource(db.Estudiantes.ToList());
            rptH.Database.Tables[1].SetDataSource(db.HojaVidas.ToList());
            rptH.Database.Tables[2].SetDataSource(db.Rotacions.ToList());

            

           
            rptH.SetParameterValue("presentacion", "A continuación le relaciono las rotaciones de los estudiantes que realizaran su rotación en su institución y los profesores con su horario.");
            rptH.SetParameterValue("fecha", "");






            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return File(stream, "application/pdf");

        }
     
        //
        // GET: /IPS_ESE/Details/5

        public ActionResult Details(int id = 0)
        {
            IPS_ESE ips_ese = db.IPS_ESE.Find(id);
            if (ips_ese == null)
            {
                return HttpNotFound();
            }
            return View(ips_ese);
        }

        //
        // GET: /IPS_ESE/Create

        public ActionResult Create()
        {
            var municipios = db.Municipios.Include(h => h.Departamento);
            List<Municipio> lista = municipios.ToList();

            ViewBag.municipioId = new SelectList(lista, "municipioId", "nombre");
            
            return View();
        }

       

        //
        // POST: /IPS_ESE/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IPS_ESE ips_ese)
        {
            if (ModelState.IsValid)
            {
                db.IPS_ESE.Add(ips_ese);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.municipioId = new SelectList(db.Municipios, "municipioId", "nombre", ips_ese.municipioId);
            return View(ips_ese);
        }

        //
        // GET: /IPS_ESE/Edit/5

        public ActionResult Edit(int id = 0)
        {
            IPS_ESE ips_ese = db.IPS_ESE.Find(id);
            if (ips_ese == null)
            {
                return HttpNotFound();
            }
            ViewBag.municipioId = new SelectList(db.Municipios, "municipioId", "nombre", ips_ese.municipioId);
            return View(ips_ese);
        }

        //
        // POST: /IPS_ESE/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IPS_ESE ips_ese)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ips_ese).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.municipioId = new SelectList(db.Municipios, "municipioId", "nombre", ips_ese.municipioId);
            
            return View(ips_ese);

            
        }

        
         //
        // GET: /IPS_ESE/Edit/5---------------------

        public ActionResult EditEPS(int id = 0)
        {
            IPS_ESE ips_ese = db.IPS_ESE.Find(id);
            if (ips_ese == null)
            {
                return HttpNotFound();
            }
            cargaDocumentos(ips_ese);
            ViewBag.municipioId = new SelectList(db.Municipios, "municipioId", "nombre", ips_ese.municipioId);
            return View(ips_ese);
        }

        //
        // POST: /IPS_ESE/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEPS(IPS_ESE ips_ese)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ips_ese).State = EntityState.Modified;


                int numFiles = Request.Files.Count;
                if (Request != null)
                {


                    int uploadedCount = 0;
                    string[] documentos = { "resolucion", "cedularl", "actap", "rut", "habilitacion" };
                    for (int i = 0; i < numFiles; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];
                        if (file.ContentLength > 0)
                        {
                            string fileName = file.FileName;
                            string fileContentType = file.ContentType;
                            byte[] fileBytes = new byte[file.ContentLength];
                            file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                            string path1 = string.Format("{0}/{1}{2}", Server.MapPath("../../Uploads/" ), documentos[i] + ips_ese.IPS_ESEId, ".jpg");
                            if (System.IO.File.Exists(path1))
                                System.IO.File.Delete(path1);

                            file.SaveAs(path1);
                            uploadedCount++;
                        }
                    }
                }


                db.SaveChanges();
                //return RedirectToAction("../Rotacion/VistaODS");
                return RedirectToAction("EditEPS/" + ips_ese.IPS_ESEId);
                //return RedirectToAction("RegistroEPS");
            }
            ViewBag.municipioId = new SelectList(db.Municipios, "municipioId", "nombre", ips_ese.municipioId);
            return RedirectToAction("EditEPS/" + ips_ese.IPS_ESEId);
            //return RedirectToAction("./Rotacion/VistaODS");
        }

        //
        // GET: /IPS_ESE/Delete/5

        public ActionResult Delete(int id = 0)
        {
            IPS_ESE ips_ese = db.IPS_ESE.Find(id);
            if (ips_ese == null)
            {
                return HttpNotFound();
            }
            return View(ips_ese);
        }

        //
        // POST: /IPS_ESE/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IPS_ESE ips_ese = db.IPS_ESE.Find(id);
            db.IPS_ESE.Remove(ips_ese);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}