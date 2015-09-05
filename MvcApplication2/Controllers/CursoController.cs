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
    public class CursoController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /Curso/

        public ActionResult Index(string searchString)
        {

          

            var cursoes = from s in db.Cursoes
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                cursoes = cursoes.Where(s => s.IPS_ESE.nombre.Contains(searchString));
            }
            return View(cursoes.ToList());
        }

        //
        // GET: /Curso/Details/5

        public ActionResult Details(int id = 0)
        {
            Curso curso = db.Cursoes.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }

        //
        // GET: /Curso/Create 



        public ActionResult Create()
        {
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre");
            return View();
        }

        //
        // GET: /Curso/VistaContraprestacion
        public ActionResult VistaContraprestacion()
        {

            return View();

        }

        //
        // POST: /Curso/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Curso curso)
        {

            curso.valorUnitarioPersona = curso.valorParticipante / curso.asignados;
                curso.totalCapacitacion = (curso.valorUnitarioPersona+curso.valorUnitarioUniversidad)*curso.asignados;
                curso.valorTotalUniversidad = curso.valorUnitarioUniversidad * curso.asignados;
                curso.valorTotalPersona = curso.valorUnitarioPersona * curso.asignados;
                curso.totalContraprestacion = curso.valorTotalUniversidad;
             
                curso.porcentajeTotalPersona = (curso.valorTotalPersona * 100) / curso.totalCapacitacion;
                curso.porcentajeTotalUniversidad = (curso.valorTotalUniversidad * 100) / curso.totalCapacitacion;
             
                db.Cursoes.Add(curso);
                db.SaveChanges();
                return RedirectToAction("Index");
            

            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre", curso.IPS_ESEId);
            return View(curso);
        }

        //
        // GET: /Curso/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Curso curso = db.Cursoes.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "origen", curso.IPS_ESEId);
            return View(curso);
        }

        //
        // POST: /Curso/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(curso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "origen", curso.IPS_ESEId);
            return View(curso);
        }

        //
        // GET: /Curso/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Curso curso = db.Cursoes.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }

        //
        // POST: /Curso/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Curso curso = db.Cursoes.Find(id);
            db.Cursoes.Remove(curso);
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