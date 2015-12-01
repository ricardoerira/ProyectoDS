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

namespace MvcApplication2.Controllers
{
    public class DocenteController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /Docente/

        public ActionResult Index()
        {
            var docentes = db.Docentes.Include(d => d.DepartamentoSalud).Include(d => d.HojaVida).Include(d => d.Rotacion);
            return View(docentes.ToList());
        }

        public ActionResult ReporteDocente(string searchString, int id = 0)
        {

            var docentes = from s in db.Docentes
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {


                docentes = docentes.Where(s => s.num_documento.Equals(searchString));
            }

            ViewBag.DepartamentoSaludId = new SelectList(db.DepartamentoSaluds, "DepartamentoSaludId", "nombre");
           
            return View(docentes.ToList());
        }

        

        public ActionResult ReporteDocenteA(int id = 0)
        {
            Docente docente = db.Docentes.Find(id);

            ReportDocument rptH2 = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/reporteDocente.rpt");
            rptH2.Load(strRptPath);


            DateTime dt = DateTime.Now.Date.AddMonths(-20);
            var vacunas = db.Vacunas.Where(r => r.hojaVidaId == docente.hojaVidaId).Where(r => r.fecha_vacunacion > dt);
            List<Vacuna> listav = vacunas.ToList();
     
            rptH2.Database.Tables[0].SetDataSource(listav.ToList());

            
            rptH2.SetParameterValue("departamento", docente.DepartamentoSalud.nombre);
            rptH2.SetParameterValue("titlePre", docente.titulo_pregrado + "");
            rptH2.SetParameterValue("dedicacion", docente.dedicacion + "");
            rptH2.SetParameterValue("maxNF", docente.maximo_nivel_formacion + "");
            rptH2.SetParameterValue("vinculacion", docente.tipo_vinculacion + "");
            rptH2.SetParameterValue("categoria", docente.categoria_escalafon_docente + "");


            
            rptH2.SetParameterValue("numdoc", docente.num_documento);
            rptH2.SetParameterValue("numLB", docente.num_libreta_militar + "");
            rptH2.SetParameterValue("genero", docente.HojaVida.genero + "");
            rptH2.SetParameterValue("nombre", docente.HojaVida.primer_nombre + " " + docente.HojaVida.segundo_nombre);
            rptH2.SetParameterValue("fecha_nacimiento", docente.HojaVida.fecha_nacimiento + "");
            rptH2.SetParameterValue("rh", docente.HojaVida.hemoclasificacion + "");
            rptH2.SetParameterValue("mun_procedencia", docente.HojaVida.municipio_procedencia + "");


            rptH2.SetParameterValue("dir_local", docente.HojaVida.direccion_manizales + "");
            rptH2.SetParameterValue("image", docente.HojaVida.imagen_DI + "");
            rptH2.SetParameterValue("tel_local", docente.HojaVida.num_telefono + "");
            rptH2.SetParameterValue("num_cel", docente.HojaVida.num_celular + "");
            rptH2.SetParameterValue("estado_civil", docente.HojaVida.estado_civil + "");
            rptH2.SetParameterValue("num_hijos", docente.HojaVida.hijos + "");
            rptH2.SetParameterValue("mail", docente.HojaVida.correo + "");





            rptH2.SetParameterValue("nameAcudiente", docente.HojaVida.Familia.primer_nombre_acudiente + " " + docente.HojaVida.Familia.primer_apellido_acudiente + " " + docente.HojaVida.Familia.segundo_apellido_acudiente);
            rptH2.SetParameterValue("DirAcudiente", docente.HojaVida.Familia.direccion_acudiente + "");
            rptH2.SetParameterValue("TelAcuediente", docente.HojaVida.Familia.telefono_acudiente + "");
            rptH2.SetParameterValue("celAcudiente", docente.HojaVida.Familia.celular_acudiente + "");

            rptH2.SetParameterValue("tituloPostgrado", docente.certificado_DPE + "");
            rptH2.SetParameterValue("TPD", docente.certificado_TPDTS+ "");
            rptH2.SetParameterValue("TPN", docente.certificado_TPN + "");
            rptH2.SetParameterValue("edad", docente.diploma_profesional+ "");
            rptH2.SetParameterValue("EPS", docente.HojaVida.entidad_salud+ "");
            rptH2.SetParameterValue("ARL", docente.HojaVida.ARL+ "");




            Stream stream = rptH2.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return File(stream, "application/pdf");

        }

        public ActionResult CarnetVacunacionDocente(int id = 0)
        {


            Docente docente = db.Docentes.Find(id);

            if (docente == null)
            {
                return HttpNotFound();
            }
            ViewBag.Details = docente.HojaVida.Vacunas;
            return View(docente);

        }

        public ActionResult CarnetVacunacionDocenteODS(int id = 0)
        {


            Docente docente = db.Docentes.Find(id);

            if (docente == null)
            {
                return HttpNotFound();
            }
            ViewBag.Details = docente.HojaVida.Vacunas;
            return View(docente);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CarnetVacunacionDocenteDS(Docente docente, int id = 0)
        {


            return RedirectToAction("../Vacuna/EsquemaVacunacionDocente/" + docente.docenteId);


        }
        public ActionResult CarnetVacunacionDocenteDS(int id = 0)
        {
            TempData["notice"] = null;

            
            Docente docente = db.Docentes.Find(id);
            HojaVida oHojaVida = db.HojaVidas.Find(docente.hojaVidaId);

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

            return View(docente);

        }

        public ActionResult BuscarEnVacuna(Docente docente)
        {
            var docentes = from b in db.Docentes
                           select b;

            foreach (var b in docentes)
            {
                if (b.num_documento.Equals(docente.num_documento))
                {
                    docente = b;
                }

            }
            if (docente.docenteId == 0)
            {
                return View(docente);
            }
            else
            {
                return RedirectToAction("../Docente/CarnetVacunacionDocenteDS/" + docente.docenteId);
            }
        }


        public ActionResult BuscarEnDepartamento(Docente docente)
        {
            var docentes = from b in db.Docentes
                           select b;

            foreach (var b in docentes)
            {
                if (b.num_documento.Equals(docente.num_documento))
                {
                    docente = b;
                }

            }
            if (docente.docenteId == 0)
            {
                return View(docente);
            }
            else
            {
                return RedirectToAction("../Docente/ReporteDocenteA/" + docente.docenteId);
            }
        }
        //
        // GET: /Docente/Details/5

        public ActionResult Details(int id = 0)
        {
            Docente docente = db.Docentes.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            return View(docente);
        }

        //
        // GET: /Docente/Create

        public ActionResult Create()
        {
            ViewBag.DepartamentoSaludId = new SelectList(db.DepartamentoSaluds, "DepartamentoSaludId", "nombre");
            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre");
            ViewBag.rotacionId = new SelectList(db.Rotacions, "rotacionId", "rotacionId");
            return View();
        }

        //
        // POST: /Docente/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Docente docente)
        {
            if (ModelState.IsValid)
            {
                db.Docentes.Add(docente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartamentoSaludId = new SelectList(db.DepartamentoSaluds, "DepartamentoSaludId", "nombre", docente.DepartamentoSaludId);
            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre", docente.hojaVidaId);
            ViewBag.rotacionId = new SelectList(db.Rotacions, "rotacionId", "tipo", docente.rotacionId);
            return View(docente);
        }

        //
        // GET: /Docente/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Docente docente = db.Docentes.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartamentoSaludId = new SelectList(db.DepartamentoSaluds, "DepartamentoSaludId", "nombre", docente.DepartamentoSaludId);
            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre", docente.hojaVidaId);
            ViewBag.rotacionId = new SelectList(db.Rotacions, "rotacionId", "tipo", docente.rotacionId);
            return View(docente);
        }

        //
        // POST: /Docente/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Docente docente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(docente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartamentoSaludId = new SelectList(db.DepartamentoSaluds, "DepartamentoSaludId", "nombre", docente.DepartamentoSaludId);
            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre", docente.hojaVidaId);
            ViewBag.rotacionId = new SelectList(db.Rotacions, "rotacionId", "tipo", docente.rotacionId);
            return View(docente);
        }

        //
        // GET: /Docente/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Docente docente = db.Docentes.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            return View(docente);
        }

        //
        // POST: /Docente/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Docente docente = db.Docentes.Find(id);
            db.Docentes.Remove(docente);
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
        //---------------------------- HOJA DE VIDA DE DOCENTES---------------------


        //
        //--------------------- Vista para Logeo del DOCENTE
        public ActionResult Login()
        {

            return View();

        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Docente docente)
        {
            
         var  b =   db.Docentes.Where(s => s.num_documento.Equals(docente.num_documento)).Where(s => s.clave.Equals(docente.clave));
          List<Docente> docenteList=b.ToList(); 
         if (docenteList.Count == 0)
            {

                ViewBag.AlertMessage = "El usuario y la contraseña que has introducido no coinciden.";
                return View(docente);
            }
            else
            {
                ViewBag.AlertMessage = null;
                Docente docente_aux = docenteList.ElementAt(0);
          
                return RedirectToAction("../Docente/Personales/" + docente_aux.docenteId);
            }
        }


        public ActionResult CambioContraseña(int id = 0)
        {
            TempData["notice"] = null;

            Docente docente = db.Docentes.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            return View(docente);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambioContraseña(Docente docente)
        {
           
            Docente doc = db.Docentes.Find(docente.docenteId);
            if (docente.clave != null)
            {
            if (docente.clave.Equals(docente.tipo_documento))
            {
                doc.clave = docente.clave;


                db.Entry(doc).State = EntityState.Modified;



                db.SaveChanges();
                return RedirectToAction("../Docente/Personales/" + doc.docenteId);

            }
                 }

            ViewBag.AlertMessage = "Las contrasenias deben coincidir";
            return View(docente);
        
        }

        
        



      



        public ActionResult LoginCC(int id = 0)
        {
            Docente docente = db.Docentes.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            return View(docente);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginCC(Docente docente)
        {
            Docente docenteReal = db.Docentes.Find(docente.docenteId);
            if (docente.clave.Equals(docenteReal.clave))
            {
                ViewBag.AlertMessage = null;
                return RedirectToAction("../Docente/CambioContraseña/" + docenteReal.docenteId);
            }
            else
            {

                ViewBag.AlertMessage = "La contraseña que has introducido no coincide.";
                return View(docente);
                //return RedirectToAction("../Docente/LoginCC/" + docenteReal.docenteId);

            }
        }



        //
        //------------------------- Vista para datos personales del DOCENTE

        //public ActionResult InformacionDocente(int id = 0)
        //{
        //    TempData["notice"] = null;

        //    Docente docente = db.Docentes.Find(id);
        //    if (docente == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    int edad = DateTime.Today.AddTicks(-docente.HojaVida.fecha_nacimiento.Ticks).Year - 1;
        //    string edadDocente = edad.ToString();
        //    docente.diploma_profesional = edadDocente;//Reemplaza edad
        //    return View(docente);
        //}

        public ActionResult cargaImagen(Docente docente)
        {
            
            HojaVida oHojaVida = db.HojaVidas.Find(docente.hojaVidaId);
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
             if (docente == null)// Do something else NO ESTOY SEGURA DONDE VAAAAAAAAAAAAAA 
             {
                 return HttpNotFound();
             }
             return View(docente);
        }
        public ActionResult Personales(int id = 0) {

            TempData["notice"] = null;           

            Docente docente = db.Docentes.Find(id);
            HojaVida oHojaVida = db.HojaVidas.Find(docente.hojaVidaId);
            cargaImagen(docente);
            
          
            int edad = DateTime.Today.AddTicks(-docente.HojaVida.fecha_nacimiento.Ticks).Year - 1;
            string edadDocente = edad.ToString();
            docente.diploma_profesional = edadDocente;//Reemplaza edad
            if (docente.HojaVida.genero.Equals("F"))
            {
                docente.num_libreta_militar = "NO APLICA";
            }

            cargaDocumentos(docente);
             return View(docente);

            
        }


        public ActionResult cargaDocumentos(Docente docente){
            string[] documentos = {"doc_identidad", "acta_grado", "dip_prof", "acta_grado_post", "dip_espe", "tpd", "tpn", "cv1", "cv2", "ant_varicela", "ant_hp"};
        

                  string path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[0] + docente.num_documento, ".jpg");

                  if (System.IO.File.Exists(path1))
                  {

                      ViewBag.imagen1 = "/Uploads/" + documentos[0] + docente.num_documento + ".jpg";

                  }
                  else
                  {
                      ViewBag.imagen1 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

                  }

                  path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[1] + docente.num_documento, ".jpg");

                  if (System.IO.File.Exists(path1))
                  {

                      ViewBag.imagen2 = "/Uploads/" + documentos[1] + docente.num_documento + ".jpg";

                  }
                  else
                  {
                      ViewBag.imagen2 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

                  }
                 
            
            path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[2] + docente.num_documento, ".jpg");

                  if (System.IO.File.Exists(path1))
                  {

                      ViewBag.imagen3 = "/Uploads/" + documentos[2] + docente.num_documento + ".jpg";

                  }
                  else
                  {
                      ViewBag.imagen3 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

                  }



                  path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[3] + docente.num_documento, ".jpg");

                  if (System.IO.File.Exists(path1))
                  {

                      ViewBag.imagen4 = "/Uploads/" + documentos[3] + docente.num_documento + ".jpg";

                  }
                  else
                  {
                      ViewBag.imagen4 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

                  }




                  path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[4] + docente.num_documento, ".jpg");

                  if (System.IO.File.Exists(path1))
                  {

                      ViewBag.imagen5 = "/Uploads/" + documentos[4] + docente.num_documento + ".jpg";

                  }
                  else
                  {
                      ViewBag.imagen5 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

                  }



                  path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[5] + docente.num_documento, ".jpg");

                  if (System.IO.File.Exists(path1))
                  {

                      ViewBag.imagen6 = "/Uploads/" + documentos[5] + docente.num_documento + ".jpg";

                  }
                  else
                  {
                      ViewBag.imagen6 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

                  }



                  path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[6] + docente.num_documento, ".jpg");

                  if (System.IO.File.Exists(path1))
                  {

                      ViewBag.imagen7 = "/Uploads/" + documentos[6] + docente.num_documento + ".jpg";

                  }
                  else
                  {
                      ViewBag.imagen7 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

                  }



                  path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[7] + docente.num_documento, ".jpg");

                  if (System.IO.File.Exists(path1))
                  {

                      ViewBag.imagen8 = "/Uploads/" + documentos[7] + docente.num_documento + ".jpg";

                  }
                  else
                  {
                      ViewBag.imagen8 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

                  }

                  path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[8] + docente.num_documento, ".jpg");

                  if (System.IO.File.Exists(path1))
                  {

                      ViewBag.imagen9 = "/Uploads/" + documentos[8] + docente.num_documento + ".jpg";

                  }
                  else
                  {
                      ViewBag.imagen9 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

                  }

                  path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[9] + docente.num_documento, ".jpg");

                  if (System.IO.File.Exists(path1))
                  {

                      ViewBag.imagen10 = "/Uploads/" + documentos[9] + docente.num_documento + ".jpg";

                  }
                  else
                  {
                      ViewBag.imagen10 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

                  }

                  path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[10] + docente.num_documento, ".jpg");

                  if (System.IO.File.Exists(path1))
                  {

                      ViewBag.imagen11 = "/Uploads/" + documentos[10] + docente.num_documento + ".jpg";

                  }
                  else
                  {
                      ViewBag.imagen11 = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

                  }
              
         
            
            
           
            return View(docente);

            
        }


        public ActionResult SoportesCompletosDS(int id = 0)
        {
            //TempData["notice"] = null;

            Docente docente = db.Docentes.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            cargaDocumentos(docente);
            return View(docente);
        }

        public ActionResult SoportesCompletos(int id = 0)
        {
            //TempData["notice"] = null;

            Docente docente = db.Docentes.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            cargaDocumentos(docente);
            return View(docente);
        }

        public ActionResult Soportes(int id = 0)
        {
            //TempData["notice"] = null;
            string imagen = Request.Params["imagen"];
            imagen = imagen.Replace("%2F", "/");


            Docente docente = db.Docentes.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            string[] documentos = { "doc_identidad", "acta_grado", "dip_prof", "dip_espe", "tp_terr", "tp_dept", "tp_nal", "otro" };



            ViewBag.imagen1 = imagen;


            return View(docente);
        }
     
        public ActionResult SoportesDS(int id = 0)
        {
            //TempData["notice"] = null;
            string imagen = Request.Params["imagen"];
            imagen = imagen.Replace("%2F", "/");
       
          
            Docente docente = db.Docentes.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            string[] documentos = { "doc_identidad", "acta_grado", "dip_prof", "dip_espe", "tp_terr", "tp_dept", "tp_nal", "otro" };


          
                ViewBag.imagen1 = imagen;

           
            return View(docente);
        }


        public ActionResult Buscar(Docente docente)
        {
            var docentes = from b in db.Docentes
                              select b;

            foreach (var b in docentes)
            {
                if (b.num_documento.Equals(docente.num_documento))
                {
                    docente = b;
                }

            }
            if (docente.docenteId == 0)
            {
                return View(docente);
            }
            else
            {
                return RedirectToAction("../Docente/PersonalesDS/" + docente.docenteId);
            }
        }
        public ActionResult PersonalesDS(int id = 0)
        {
            TempData["notice"] = null;



            Docente docente = db.Docentes.Find(id);
            HojaVida oHojaVida = db.HojaVidas.Find(docente.hojaVidaId);
            cargaImagen(docente);
            int edad = DateTime.Today.AddTicks(-docente.HojaVida.fecha_nacimiento.Ticks).Year - 1;
            string edadDocente = edad.ToString();
            docente.diploma_profesional = edadDocente;//Reemplaza edad

            if (docente.HojaVida.genero.Equals("F"))
            {
                docente.num_libreta_militar = "NO APLICA";
            }
            Boolean estado = ValidarCamposDocente(docente);
            ViewBag.estado = estado;

            cargaDocumentos(docente);
            return View(docente);

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SolicitarActualizacion(Docente docente)
        {
            docente = db.Docentes.Find(docente.docenteId);
            var fromAddress = new MailAddress("info@salud.ucaldas.edu.co", "Decanatura – Oficina Docencia Servicio");
            var toAddress = new MailAddress("ricardoerira@gmail.com", "To Name");
            const string fromPassword = "descargar";
            const string subject = "Solicitud actualizacion hoja de vida";
            const string body = "<h3>Cordial saludo</h3><h3 style=\"text-align: justify;\">La Facultad de Ciencias para la Salud a través de su Oficina Docencia Servicio le solicita actualizar su hoja de vida; para ello disponemos de la nueva plataforma web la cual podrá acceder a través del siguiente enlace.</h3><h3>&nbsp;<a href=\"http://salud.ucaldas.edu.co\">http://salud.ucaldas.edu.co/</a></h3><h3>Los datos de ingreso son:&nbsp;</h3><h3><strong>Usuario</strong>: Cédula Docente </h3><h3><strong>Contrase&ntilde;a</strong>: Cédula docente&nbsp;</h3><p>&nbsp;</p><p>&nbsp;</p><p><img src=\"https://upload.wikimedia.org/wikipedia/commons/thumb/8/89/Universidad_De_Caldas_-_Logo.jpg/180px-Universidad_De_Caldas_-_Logo.jpg\" alt=\"\" width=\"160\" height=\"160\" /></p><p>&nbsp;</p><p>Copyright &copy; <a href=\"http://www.ucaldas.edu.co/portal\"><strong>Facultad de Ciencias para la Salud </strong></a> - Sede Versalles Carrera 25  48-57 / Tel +57 878 30 60 Ext. 31255 / E-mail docencia.servicio@ucaldas.edu.co</p> ";


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



                smtp.EnableSsl = false;
                smtp.Send(message);


            }


            catch (Exception e)
            {

                Console.WriteLine("Ouch!" + e.ToString());

            }

            return RedirectToAction("../Docente/PersonalesDS/" + docente.docenteId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonalesDS(Docente docente) {
            ModelState.Remove("certificado_TPDTS");

             docente = db.Docentes.Find(docente.docenteId);
            int numFiles = Request.Files.Count;
            if (Request != null)
            {


                int uploadedCount = 0;
                string[] documentos = {"doc_identidad", "acta_grado", "dip_prof", "acta_grado_post", "dip_espe", "tpd", "tpn", "cv1", "cv2", "ant_varicela", "ant_hp"};
                for (int i = 0; i < numFiles; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    if (file.ContentLength > 0)
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        string path1 = string.Format("{0}/{1}{2}", Server.MapPath("../../Uploads/"), documentos[i] + docente.num_documento, ".jpg");
                        if (System.IO.File.Exists(path1))
                            System.IO.File.Delete(path1);

                        file.SaveAs(path1);
                        uploadedCount++;
                    }
                }
            }
            Boolean estado = ValidarCamposDocente(docente);
            ViewBag.estado = estado;

            cargaImagen(docente);
            cargaDocumentos(docente); 
            return View(docente);
           


            }
        




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Personales(Docente docente)
        {
            if (ModelState.IsValid)
            {

                HojaVida oHojaVida = db.HojaVidas.Find(docente.hojaVidaId);
                Docente docente1 = db.Docentes.Find(docente.docenteId);
                

                oHojaVida.direccion_manizales = docente.HojaVida.direccion_manizales;
                oHojaVida.correo = docente.HojaVida.correo;
                oHojaVida.estado_civil = docente.HojaVida.estado_civil;
                oHojaVida.hijos = docente.HojaVida.hijos;
                oHojaVida.municipio_procedencia = docente.HojaVida.municipio_procedencia;
                oHojaVida.num_celular = docente.HojaVida.num_celular;
                oHojaVida.num_telefono = docente.HojaVida.num_telefono;
                oHojaVida.hemoclasificacion = docente.HojaVida.hemoclasificacion;
                
                
                docente1.tipo_vinculacion = docente.tipo_vinculacion;
                docente1.categoria_escalafon_docente = docente.categoria_escalafon_docente;
                docente1.certificado_TPDTS = docente.certificado_TPDTS;
                docente1.certificado_TPN = docente.certificado_TPN;
                docente1.certificado_DPE = docente.certificado_DPE; //Reemplaza titulo postgrado
                docente1.otro_titulo = docente.otro_titulo;

               

                int edad = DateTime.Today.AddTicks(-docente.HojaVida.fecha_nacimiento.Ticks).Year - 1;
                string edadDocente = edad.ToString();
                docente1.diploma_profesional = edadDocente;//Reemplaza edad
                
                
                
                               
                oHojaVida.ARL = docente.HojaVida.ARL;
                oHojaVida.Familia = docente.HojaVida.Familia;

                if (docente.HojaVida.genero.Equals("F"))
                {
                    docente1.num_libreta_militar = "NO APLICA";
                }
                
                

                docente.HojaVida = null;
               
                db.Entry(docente1).State = EntityState.Modified;

                int numFiles = Request.Files.Count;
                if (Request != null)
                {
                   
                   
                    int uploadedCount = 0;
                    string[] documentos = { "doc_identidad", "acta_grado", "dip_prof", "acta_grado_post", "dip_espe", "tpd", "tpn", "cv1", "cv2", "ant_varicela", "ant_hp"};
                    for (int i = 0; i < numFiles; i++)
                    {
                         HttpPostedFileBase  file = Request.Files[i];
                        if (file.ContentLength > 0)
                        {
                            string fileName = file.FileName;
                            string fileContentType = file.ContentType;
                            byte[] fileBytes = new byte[file.ContentLength];
                            file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                            string path1 = string.Format("{0}/{1}{2}", Server.MapPath("../../Uploads/"), documentos[i] + docente.num_documento, ".jpg");
                            if (System.IO.File.Exists(path1))
                                System.IO.File.Delete(path1);

                            file.SaveAs(path1);
                            uploadedCount++;
                        }
                    }
                }
               
                db.SaveChanges();
                return RedirectToAction("../Docente/Personales/" + docente1.docenteId);
                //return View(docente1);
            }
            else
            {
                cargaImagen(docente);
                cargaDocumentos(docente);
                Docente docente2 = db.Docentes.Find(docente.docenteId);
               
                
                return View(docente2);
            }
            
        }


        public Boolean ValidarCamposDocente(Docente docente)
        {
            HojaVida hv = db.HojaVidas.Find(docente.hojaVidaId);
            Docente d = db.Docentes.Find(docente.docenteId);
            Familia f = db.Familias.Find(hv.familiaId);


            if ((d.certificado_TPDTS !=null) && (hv.municipio_procedencia != null) && (hv.direccion_manizales != null) &&
                (hv.ARL != "Sin Asignar") && (hv.hemoclasificacion != "Sin Asignar") &&
                (hv.num_celular != 0) &&
                (hv.estado_civil != "Sin Asignar") && (f.primer_nombre_acudiente != null) &&
                (f.primer_apellido_acudiente != null) && (f.direccion_acudiente != null) &&
                (f.celular_acudiente != 0) && (d.tipo_vinculacion !="Sin Asignar") &&
                (d.dedicacion !=null) && (d.categoria_escalafon_docente!="Sin Asignar"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //EstadoHVdepto


        public ActionResult EstadoHVdepto(string num_documento, string departamentoSaludId, string estado_HV)
        {
            int did = 0;
            bool b = false;
            if (!String.IsNullOrEmpty(departamentoSaludId))
            {
                did = Int32.Parse(departamentoSaludId);
                b = estado_HV.Equals("True") ? true : false;

            }

            var docentes = new List<Docente>();
            var docentesaux = new List<Docente>();

            if (!String.IsNullOrEmpty(num_documento))
            {
                docentes = db.Docentes.Where(s => s.num_documento.Equals(num_documento)).Where(s => s.DepartamentoSaludId == did).Where(s => s.HojaVida.estado_HV == b).ToList();

            }
            else
            {
                if (!String.IsNullOrEmpty(departamentoSaludId))
                {
                    docentes = db.Docentes.Where(s => s.DepartamentoSaludId == did).Where(s => s.HojaVida.estado_HV == b).ToList();
                    docentesaux = db.Docentes.Where(s => s.DepartamentoSaludId == did).ToList();

                }
            }




            ViewBag.busqueda = docentes.Count() + " / " + docentesaux.Count();




            ViewBag.DepartamentoSaludId = new SelectList(db.DepartamentoSaluds, "DepartamentoSaludId", "nombre");

            return View(docentes.ToList());
        }





       

        public ActionResult EstadoHV(string num_documento,string departamentoSaludId,string estado_HV )
        {
            int did = 0;
            bool b = false;
            if (!String.IsNullOrEmpty(departamentoSaludId))
            {
                 did = Int32.Parse(departamentoSaludId);
                 b = estado_HV.Equals("True") ? true : false;
            
            }
               
            var docentes= new List<Docente>();
            var docentesaux = new List<Docente>();
           
            if (!String.IsNullOrEmpty(num_documento))   
            {
                docentes = db.Docentes.Where(s => s.num_documento.Equals(num_documento)).Where(s => s.DepartamentoSaludId == did).Where(s => s.HojaVida.estado_HV == b).ToList();
              
            }
            else
            {
                if (!String.IsNullOrEmpty(departamentoSaludId))
                {
                    docentes = db.Docentes.Where(s => s.DepartamentoSaludId == did).Where(s => s.HojaVida.estado_HV == b).ToList();
                    docentesaux = db.Docentes.Where(s => s.DepartamentoSaludId == did).ToList();
            
                }
            }




            ViewBag.busqueda = docentes.Count() + " / " + docentesaux.Count();
   

                        
           
            ViewBag.DepartamentoSaludId = new SelectList(db.DepartamentoSaluds, "DepartamentoSaludId", "nombre");
           
            return View(docentes.ToList());
        }




        public ActionResult RotacionDocente2(string searchString, int id = 0)
        {

            string rotacion = Request.Params["rotacion"];
            if (rotacion == null)
            {
                ViewBag.id = id;
            }
            else
            {
                Docente docente = db.Docentes.Find(id);
                docente.rotacionId = Convert.ToInt32(rotacion);
                Rotacion rotacionE = db.Rotacions.Find(Convert.ToInt32(rotacion));
                
                docente.Rotacion = rotacionE;
                db.Entry(rotacionE).State = EntityState.Modified;
                db.SaveChanges();
                docente.Rotacion = rotacionE;
                db.Entry(docente).State = EntityState.Modified;
                db.SaveChanges();

            }


            return RedirectToAction("RotacionDocente/" + rotacion);
        }
    }
}