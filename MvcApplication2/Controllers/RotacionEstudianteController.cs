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
    public class RotacionEstudianteController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /RotacionEstudiante/

        public ActionResult Index(int id = 0)
        {

            var rotacionestudiantes = db.RotacionEstudiantes.Where(r => r.rotacionId == id).Include(r=>r.Rotacion);
            ViewBag.nombre = db.Rotacions.Find(id).ActividadAcademica.nombre;
            return View(rotacionestudiantes.ToList());
        }

        //
        // GET: /RotacionEstudiante/Details/5

        public ActionResult Details(int id = 0)
        {
            RotacionEstudiante rotacionestudiante = db.RotacionEstudiantes.Find(id);
            if (rotacionestudiante == null)
            {
                return HttpNotFound();
            }
            return View(rotacionestudiante);
        }

        //
        // GET: /RotacionEstudiante/Create

        public ActionResult Create(int id = 0)
        {
            Rotacion rotacion = db.Rotacions.Find(id);
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre");
            ViewBag.rotacionId = id;
            ViewBag.docenteId = new SelectList(db.Docentes, "docenteId", "tipo_documento");
            ViewBag.estudianteId = new SelectList(db.Estudiantes, "estudianteId", "tipo_documento");
            return View();
        }

        //
        // POST: /RotacionEstudiante/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RotacionEstudiante rotacionestudiante)
        {
            if (ModelState.IsValid)
            {
                db.RotacionEstudiantes.Add(rotacionestudiante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "origen", rotacionestudiante.IPS_ESEId);
            ViewBag.rotacionId = new SelectList(db.Rotacions, "rotacionId", "grupo", rotacionestudiante.rotacionId);
            ViewBag.docenteId = new SelectList(db.Docentes, "docenteId", "tipo_documento", rotacionestudiante.docenteId);
            ViewBag.estudianteId = new SelectList(db.Estudiantes, "estudianteId", "tipo_documento", rotacionestudiante.estudianteId);
            return View(rotacionestudiante);
        }

        //
        // GET: /RotacionEstudiante/Edit/5

        public ActionResult Edit(int id = 0)
        {
            
            RotacionEstudiante rotacionestudiante = db.RotacionEstudiantes.Find(id);
            if (rotacionestudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.rotacionEstudianteId = rotacionestudiante.rotacionEstudianteId;

            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre", rotacionestudiante.IPS_ESEId);
            ViewBag.rotacionId = new SelectList(db.Rotacions, "rotacionId", "grupo", rotacionestudiante.rotacionId);
            ViewBag.docenteId = new SelectList(db.Docentes, "docenteId", "num_documento", rotacionestudiante.docenteId);
            ViewBag.estudianteId = new SelectList(db.Estudiantes, "estudianteId", "tipo", rotacionestudiante.estudianteId);
            return View(rotacionestudiante);
        }

        //
        // POST: /RotacionEstudiante/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RotacionEstudiante rotacionestudiante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rotacionestudiante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index/" + rotacionestudiante.rotacionId);
            }
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre", rotacionestudiante.IPS_ESEId);
            ViewBag.rotacionId = new SelectList(db.Rotacions, "rotacionId", "grupo", rotacionestudiante.rotacionId);
            ViewBag.docenteId = new SelectList(db.Docentes, "docenteId", "num_documento", rotacionestudiante.docenteId);
            ViewBag.estudianteId = new SelectList(db.Estudiantes, "estudianteId", "tipo_documento", rotacionestudiante.estudianteId);
            return View(rotacionestudiante);
        }

        //
        // GET: /RotacionEstudiante/Delete/5

        public ActionResult Delete(int id = 0)
        {
            RotacionEstudiante rotacionestudiante = db.RotacionEstudiantes.Find(id);
            if (rotacionestudiante == null)
            {
                return HttpNotFound();
            }
            return View(rotacionestudiante);
        }

        //
        // POST: /RotacionEstudiante/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RotacionEstudiante rotacionestudiante = db.RotacionEstudiantes.Find(id);
            db.RotacionEstudiantes.Remove(rotacionestudiante);
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