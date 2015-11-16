using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Text;

namespace MvcApplication2.Controllers
{
    public class ActividadAcademicaController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /ActividadAcademica/

        public ActionResult Index()
        {
            importaMaterias();
            var actividadacademicas = db.ActividadAcademicas.Include(a => a.DepartamentoSalud);
            return View(actividadacademicas.ToList());
        }

        //
        // GET: /ActividadAcademica/Details/5
        public void importaMaterias()
        {
            ServiceReference1.WSFacultadSaludSoapClient ser = new ServiceReference1.WSFacultadSaludSoapClient();

            string json = ser.getMaterias();
         
            json = json.Replace("\"materias\"", "6@");
      
            json = json.Replace("\":\"", "1@");
      json = json.Replace("\",\"", "2@");
      json = json.Replace("{\"", "3@");
      json = json.Replace("\"}", "4@");
      json = json.Replace("\"\"", "5@");
     
            json = json.Replace("\"", "");


      json = json.Replace("1@", "\":\"");
      json = json.Replace("2@", "\",\"");
      json = json.Replace("3@", "{\"");
      json = json.Replace("4@", "\"}");
      json = json.Replace("5@","\"\"" );
      json = json.Replace("6@","\"materias\"" );
     
          

            MvcApplication2.Models.Materia.ESObject0 listmaterias = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.Materia.ESObject0>(json);
            List<DepartamentoSalud> departamentos = db.DepartamentoSaluds.ToList();
            int cont = 0;
            foreach (var item in listmaterias.materias)
            {
                ActividadAcademica academica = new ActividadAcademica();
                Boolean estado = false;
                int iddept = 0;
                foreach (var item2 in departamentos)
                {

                    if (item.NOM_DEPTO!=null && item.NOM_DEPTO.Equals("QUIRÚRGICO".ToUpper()) )
                    {
                        estado = true;

                        iddept = 4;
                    }

                }
                if (estado)
                {
                    academica.DepartamentoSaludId = iddept;

                }

                cont++;
                if (academica.DepartamentoSaludId != 0)
                {
                    academica.asignatura = item.NOM_MATERIA;
                    academica.nombre = item.NOM_MATERIA;
                    academica.codigo_AA = item.COD_MATERIA;
                    
                    academica.modalidad_practica = item.PMO_NOMBRE;
                    if (item.GRUPOS_MAXIMO != null && !item.GRUPOS_MAXIMO.Equals(String.Empty))
                    {
                        academica.grupo_maximo = Int32.Parse(item.GRUPOS_MAXIMO);

                    }
                    db.ActividadAcademicas.Add(academica);
                    db.SaveChanges();

                }

            }
        }
        public ActionResult Details(int id = 0)
        {
            ActividadAcademica actividadacademica = db.ActividadAcademicas.Find(id);
            if (actividadacademica == null)
            {
                return HttpNotFound();
            }
            return View(actividadacademica);
        }

        //
        // GET: /ActividadAcademica/Create

        public ActionResult Create()
        {
            ViewBag.DepartamentoSaludId = new SelectList(db.DepartamentoSaluds, "DepartamentoSaludId", "nombre");
            return View();
        }

        //
        // POST: /ActividadAcademica/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActividadAcademica actividadacademica)
        {
            if (ModelState.IsValid)
            {
                db.ActividadAcademicas.Add(actividadacademica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartamentoSaludId = new SelectList(db.DepartamentoSaluds, "DepartamentoSaludId", "nombre", actividadacademica.DepartamentoSaludId);
            return View(actividadacademica);
        }

        //
        // GET: /ActividadAcademica/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ActividadAcademica actividadacademica = db.ActividadAcademicas.Find(id);
            if (actividadacademica == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartamentoSaludId = new SelectList(db.DepartamentoSaluds, "DepartamentoSaludId", "nombre", actividadacademica.DepartamentoSaludId);
            return View(actividadacademica);
        }

        //
        // POST: /ActividadAcademica/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ActividadAcademica actividadacademica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actividadacademica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartamentoSaludId = new SelectList(db.DepartamentoSaluds, "DepartamentoSaludId", "nombre", actividadacademica.DepartamentoSaludId);
            return View(actividadacademica);
        }

        //
        // GET: /ActividadAcademica/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ActividadAcademica actividadacademica = db.ActividadAcademicas.Find(id);
            if (actividadacademica == null)
            {
                return HttpNotFound();
            }
            return View(actividadacademica);
        }

        //
        // POST: /ActividadAcademica/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActividadAcademica actividadacademica = db.ActividadAcademicas.Find(id);
            db.ActividadAcademicas.Remove(actividadacademica);
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