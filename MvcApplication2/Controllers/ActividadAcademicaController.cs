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
            string dato = ",\"GRUPOS_MAXIMO\":\"\"";
            string dato2 = ",\"CUPOS_MAXIMO\":\"\"";
            string dato5 = ",\"PMO_NOMBRE\":\"\"";
            string dato3 = "\"AMARTYA SEN\"";
            string dato4 = "\"PROFUNDIZACION EN SISTEMAS DE PRODUCCION\"";

            json = json.Replace(dato5, " ");
            //json = json.Replace(dato2, " ");
            //json = json.Replace(dato3, " ");
            //json = json.Replace(dato4, " ");
            //json = json.Replace("\"\"", " ");

            //json = json.Substring(0, 699957);

            //json += "}]}";

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

                    if (item.NOM_DEPTO.Equals("QUIRÚRGICO".ToUpper()))
                    {
                        estado = true;
                        iddept = item2.DepartamentoSaludId;
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
                    if (item.CUPOS_MAXIMO != null)
                    {
                        academica.max_estudiantes_rotacion = Int32.Parse(item.CUPOS_MAXIMO);

                    }
                    academica.modalidad_practica = item.PMO_NOMBRE;
                    if (item.GRUPOS_MAXIMO != null)
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