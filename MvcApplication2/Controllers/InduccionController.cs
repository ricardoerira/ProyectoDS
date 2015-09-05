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
    public class InduccionController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /Induccion/

        public ActionResult Index(string searchString)
        {



            var cursoes = from s in db.Induccions
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                cursoes = cursoes.Where(s => s.IPS_ESE.nombre.Contains(searchString));
            }
            return View(cursoes.ToList());
        }

        //
        // GET: /Induccion/Details/5

        public ActionResult Details(int id = 0)
        {
            Induccion induccion = db.Induccions.Find(id);
            if (induccion == null)
            {
                return HttpNotFound();
            }
            return View(induccion);
        }

        //
        // GET: /Induccion/Create

        public ActionResult Create()
        {
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre");
            return View();
        }

        //
        // POST: /Induccion/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Induccion induccion)
        {
            if (ModelState.IsValid)
            {
                db.Induccions.Add(induccion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre", induccion.IPS_ESEId);
            return View(induccion);
        }

        //
        // GET: /Induccion/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Induccion induccion = db.Induccions.Find(id);
            if (induccion == null)
            {
                return HttpNotFound();
            }
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "origen", induccion.IPS_ESEId);
            return View(induccion);
        }

        //
        // POST: /Induccion/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Induccion induccion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(induccion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "origen", induccion.IPS_ESEId);
            return View(induccion);
        }

        //
        // GET: /Induccion/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Induccion induccion = db.Induccions.Find(id);
            if (induccion == null)
            {
                return HttpNotFound();
            }
            return View(induccion);
        }

        //
        // POST: /Induccion/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Induccion induccion = db.Induccions.Find(id);
            db.Induccions.Remove(induccion);
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