using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class VacunaController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /Vacuna/
      
        public ActionResult Index()
        {
            var vacunas = db.Vacunas.Include(v => v.HojaVida);
            return View(vacunas.ToList());
        }

        //
        // GET: /Vacuna/Details/5

        public ActionResult Details(int id = 0)
        {
            Vacuna vacuna = db.Vacunas.Find(id);
            if (vacuna == null)
            {
                return HttpNotFound();
            }
            return View(vacuna);
        }


        //
        // GET: /Vacuna/Create

        public ActionResult Create()
        {
            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre");
            return View();
        }

        //
        // POST: /Vacuna/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vacuna vacuna)
        {

            if (ModelState.IsValid)
            {
                db.Vacunas.Add(vacuna);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre", vacuna.hojaVidaId);
            return View(vacuna);
        }

        //
        // GET: /Vacuna/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Vacuna vacuna = db.Vacunas.Find(id);
            if (vacuna == null)
            {
                return HttpNotFound();
            }
            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre", vacuna.hojaVidaId);
            return View(vacuna);
        }

        //
        // POST: /Vacuna/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vacuna vacuna)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vacuna).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre", vacuna.hojaVidaId);
            return View(vacuna);
        }

        //
        // GET: /Vacuna/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Vacuna vacuna = db.Vacunas.Find(id);
            if (vacuna == null)
            {
                return HttpNotFound();
            }
            return View(vacuna);
        }

        //
        // POST: /Vacuna/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vacuna vacuna = db.Vacunas.Find(id);
            db.Vacunas.Remove(vacuna);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EsquemaVacunacionDocente(Vacuna vacuna, int id = 0)
        {
            if (ModelState.IsValid)
            {
            db.Entry(vacuna).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("EsquemaVacunacionDocente/" + id);
            }
            return View(vacuna);
        }
        
        public ActionResult VistaIPS_Universitaria()
        {

            return View();

        }


        public ActionResult Precios()
        {

            return View();

        }

        public ActionResult PreciosIpsU()
        {

            return View();

        }

        public ActionResult EsquemaObligatorio()
        {

            return View();

        } //anticuerposVaricela








        public ActionResult Anticuerpos(int id = 0)
        {
            string vacuna = Request.Params["vacuna"];

            Estudiante estudiante = db.Estudiantes.Find(id);
            int i = 0;
            foreach (Vacuna item in estudiante.HojaVida.Vacunas)
            {
                i++;
                if (item.nombre_generico.Equals(vacuna))
                {
                    break;


                }
            }
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            Vacuna ivacuna;
            try
            {
                ivacuna = estudiante.HojaVida.Vacunas.ElementAt(i - 1);
            }
            catch (Exception e)
            {
                ivacuna = estudiante.HojaVida.Vacunas.ElementAt(0);
            }
            ivacuna.fecha_vacunacion = DateTime.Now.Date;
            ivacuna.fecha_prox_vacunacion = DateTime.Now.Date.AddMonths(1);

            ViewBag.num_documento = estudiante.num_documento;
            return View(ivacuna);
            
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Anticuerpos(Vacuna vacuna, int id = 0)
        {
            if (ModelState.IsValid)
            {

                db.Entry(vacuna).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("anticuerposVaricela/" + id);
            }
            vacuna = db.Vacunas.Find(vacuna.vacunaId);
            ViewBag.num_documento = vacuna.HojaVida.Estudiante.ElementAt(0).num_documento;

            return View(vacuna);
            //ME DEBE RETORNAR TAMBIEN EL ESTUDIANTE PARA QUE SE PUEDA VISUALIZAR EN LA VISTA
        }


        public ActionResult EsquemaVacunacion(int id = 0)
        {
            string vacuna = Request.Params["vacuna"];

            Estudiante estudiante = db.Estudiantes.Find(id);
            int i = 0;
            foreach (Vacuna item in estudiante.HojaVida.Vacunas)
            {
                i++;
                if (item.nombre_generico.Equals(vacuna))
                {
                    break;


                }
            }
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            Vacuna ivacuna ;
            try
            {
                 ivacuna = estudiante.HojaVida.Vacunas.ElementAt(i-1);
            }
            catch (Exception e)
            {
                 ivacuna = estudiante.HojaVida.Vacunas.ElementAt(0);
            }
            ivacuna.fecha_vacunacion = DateTime.Now.Date;
            ivacuna.fecha_prox_vacunacion = DateTime.Now.Date.AddMonths(1);

            ViewBag.num_documento = estudiante.num_documento;
            return View(ivacuna);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EsquemaVacunacion(Vacuna vacuna, int id = 0)
        {
            if (ModelState.IsValid)
            {

                db.Entry(vacuna).State = EntityState.Modified;
                db.SaveChanges();
                cargaDocumento(vacuna);
                return RedirectToAction("EsquemaVacunacion/" + id);
            }
            vacuna = db.Vacunas.Find(vacuna.vacunaId);
            ViewBag.num_documento = vacuna.HojaVida.Estudiante.ElementAt(0).num_documento;
         
            return View(vacuna);
            //ME DEBE RETORNAR TAMBIEN EL ESTUDIANTE PARA QUE SE PUEDA VISUALIZAR EN LA VISTA
        }

        public ActionResult cargaDocumento(Vacuna vacuna)
        {
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
                    string path1="";
                    if (vacuna.nombre_generico.Equals("Anticuerpos contra hepatitis B "))
                     path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), "anticuerpoHepatitis_" + vacuna.hojaVidaId, ".jpg");
                    else

                        path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/Uploads/"), "anticuerpoVaricela_" + vacuna.hojaVidaId, ".jpg");
                  
                    if (System.IO.File.Exists(path1))
                        System.IO.File.Delete(path1);
                     Request.Files["InputFile"].SaveAs(path1);
                }
            }
            return View(vacuna);
        }
                        
                     
       
                

        public ActionResult EsquemaVacunacionDocente(int id = 0)
        {
            string vacuna = Request.Params["vacuna"];

            
            Docente docente = db.Docentes.Find(id);
            int i = 0;
            foreach (Vacuna item in docente.HojaVida.Vacunas)
            {
                i++;
                if (item.nombre_generico.Equals(vacuna))
                {
                    break;


                }
            }
            if (docente == null)
            {
                return HttpNotFound();
            }
            Vacuna ivacuna;
            try
            {
                ivacuna = docente.HojaVida.Vacunas.ElementAt(i - 1);
            }
            catch (Exception e)
            {
                ivacuna = docente.HojaVida.Vacunas.ElementAt(0);
            }
            ivacuna.fecha_vacunacion = DateTime.Now.Date;
            ivacuna.fecha_prox_vacunacion = DateTime.Now.Date.AddMonths(1);

            ViewBag.num_documento = docente.num_documento;
            return View(ivacuna);

        }
    }
}