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
    public class ServicioController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /Servicio/

        public ActionResult Index()
        {
            return View(db.Servicios.ToList());
        }

        //
        // GET: /Servicio/Details/5

        public ActionResult Details(int id = 0)
        {
            Servicio servicio = db.Servicios.Find(id);
            if (servicio == null)
            {
                return HttpNotFound();
            }
            return View(servicio);
        }

        //
        // GET: /Servicio/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Servicio/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                db.Servicios.Add(servicio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(servicio);
        }

        //
        // GET: /Servicio/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Servicio servicio = db.Servicios.Find(id);
            if (servicio == null)
            {
                return HttpNotFound();
            }
            return View(servicio);
        }

        //
        // POST: /Servicio/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(servicio);
        }

        //
        // GET: /Servicio/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Servicio servicio = db.Servicios.Find(id);
            if (servicio == null)
            {
                return HttpNotFound();
            }
            return View(servicio);
        }

        //
        // POST: /Servicio/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Servicio servicio = db.Servicios.Find(id);
            db.Servicios.Remove(servicio);
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