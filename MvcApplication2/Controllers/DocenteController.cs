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
            
            Docente docente = db.Docentes.Find(id);

            if (docente == null)
            {
                return HttpNotFound();
            }
            ViewBag.Details = docente.HojaVida.Vacunas;
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

               return RedirectToAction("../Docente/Login/");
            }
            else
            {
                Docente docente_aux = docenteList.ElementAt(0);
          
                return RedirectToAction("../Docente/InformacionDocente/" + docente_aux.docenteId);
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
            if (ModelState.IsValid)
            {
                Docente doc = db.Docentes.Find(docente.docenteId);

                if (docente.clave.Equals(docente.tipo_documento))
                {
                    doc.clave = docente.clave;


                    db.Entry(doc).State = EntityState.Modified;



                    db.SaveChanges();
                    return RedirectToAction("../Docente/InformacionDocente/" + doc.docenteId);

                }
            }
            return RedirectToAction("../Docente/CambioContraseña/" + docente.docenteId);
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
                return RedirectToAction("../Docente/CambioContraseña/" + docenteReal.docenteId);
            }
            else
            {

                return RedirectToAction("../Docente/LoginCC/" + docenteReal.docenteId);

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
        public ActionResult InformacionDocente(int id = 0) {

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
            if (docente == null)
            {
                return HttpNotFound();
            }
            int edad = DateTime.Today.AddTicks(-docente.HojaVida.fecha_nacimiento.Ticks).Year - 1;
            string edadDocente = edad.ToString();
            docente.diploma_profesional = edadDocente;//Reemplaza edad
            if (docente.HojaVida.genero.Equals("F"))
            {
                docente.num_libreta_militar = "NO APLICA";
            }
           
            return View(docente);

            
        }

        public ActionResult Soportes(int id = 0)
        {
            //TempData["notice"] = null;

            Docente docente = db.Docentes.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
            return View(docente);
        }

        public ActionResult SoportesDS(int id = 0)
        {
            //TempData["notice"] = null;

            Docente docente = db.Docentes.Find(id);
            if (docente == null)
            {
                return HttpNotFound();
            }
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
                return RedirectToAction("../Docente/InformacionDocenteVista/" + docente.docenteId);
            }
        }
        public ActionResult InformacionDocenteVista(int id = 0)
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
            if (docente == null)
            {
                return HttpNotFound();
            }
            int edad = DateTime.Today.AddTicks(-docente.HojaVida.fecha_nacimiento.Ticks).Year - 1;
            string edadDocente = edad.ToString();
            docente.diploma_profesional = edadDocente;//Reemplaza edad

            if (docente.HojaVida.genero.Equals("F"))
            {
                docente.num_libreta_militar = "NO APLICA";
            }
            return View(docente);

            
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InformacionDocenteVista(Docente docente) {
            int numFiles = Request.Files.Count;
            if (Request != null)
            {


                int uploadedCount = 0;
                string[] documentos = { "doc_identidad", "acta_grado", "dip_prof", "dip_espe", "tp_terr", "tp_dept", "tp_nal", "otro" };
                for (int i = 0; i < numFiles; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    if (file.ContentLength > 0)
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        string path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[i] + docente.num_documento, ".jpg");
                        if (System.IO.File.Exists(path1))
                            System.IO.File.Delete(path1);

                        file.SaveAs(path1);
                        uploadedCount++;
                    }
                }
            }

             docente = db.Docentes.Find(docente.docenteId);
            return View(docente);
           

            }
        




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InformacionDocente(Docente docente)
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
                    string[] documentos={"doc_identidad","acta_grado","dip_prof","dip_espe","tp_terr","tp_dept","tp_nal","otro"};
                    for (int i = 0; i < numFiles; i++)
                    {
                         HttpPostedFileBase  file = Request.Files[i];
                        if (file.ContentLength > 0)
                        {
                            string fileName = file.FileName;
                            string fileContentType = file.ContentType;
                            byte[] fileBytes = new byte[file.ContentLength];
                            file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                            string path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), documentos[i] + docente.num_documento, ".jpg");
                            if (System.IO.File.Exists(path1))
                                System.IO.File.Delete(path1);

                            file.SaveAs(path1);
                            uploadedCount++;
                        }
                    }
                }
               
                db.SaveChanges();
                return View(docente1);
            }
            return View(docente);
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




        public ActionResult RotacionDocente(string searchString, int id = 0)
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
                db.Entry(docente).State = EntityState.Modified;
                db.SaveChanges();

            }
            var docentes = from s in db.Docentes
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                docentes = docentes.Where(s => s.num_documento.Equals(searchString));
            }


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