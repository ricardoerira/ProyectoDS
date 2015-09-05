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
    public class EquipoController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /Equipo/

        public ActionResult Index(string searchString)
        {



            var cursoes = from s in db.Equipoes
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                cursoes = cursoes.Where(s => s.IPS_ESE.nombre.Contains(searchString));
            }
            return View(cursoes.ToList());
        }
        //
        // GET: /Equipo/Details/5

        public ActionResult Details(int id = 0)
        {
            Equipo equipo = db.Equipoes.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            return View(equipo);
        }

        //
        // GET: /Equipo/Create

        public ActionResult Create()
        {
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre");
            return View();
        }
        public ActionResult Equipo()
        {
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Equipo(Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                equipo.recibido= equipo.IPS_ESE.representante_legal;
                db.Equipoes.Add(equipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "origen", equipo.IPS_ESEId);
            return View(equipo);
        }

        //
        // POST: /Equipo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                db.Equipoes.Add(equipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "origen", equipo.IPS_ESEId);
            return View(equipo);
        }

        //
        // GET: /Equipo/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Equipo equipo = db.Equipoes.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "origen", equipo.IPS_ESEId);
            return View(equipo);
        }

        //
        // POST: /Equipo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "origen", equipo.IPS_ESEId);
            return View(equipo);
        }

        //
        // GET: /Equipo/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Equipo equipo = db.Equipoes.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            return View(equipo);
        }

        //
        // POST: /Equipo/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Equipo equipo = db.Equipoes.Find(id);
            db.Equipoes.Remove(equipo);
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