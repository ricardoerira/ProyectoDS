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
    public class ProgramaController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /Programa/

        public ActionResult Index()
        {
            return View(db.Programas.ToList());
        }

        //
        // GET: /Programa/Details/5

        public ActionResult Details(int id = 0)
        {
            Programa programa = db.Programas.Find(id);
            if (programa == null)
            {
                return HttpNotFound();
            }
            return View(programa);
        }

        //
        // GET: /Programa/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Programa/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Programa programa)
        {
            if (ModelState.IsValid)
            {
                db.Programas.Add(programa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(programa);
        }

        //
        // GET: /Programa/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Programa programa = db.Programas.Find(id);
            if (programa == null)
            {
                return HttpNotFound();
            }
            return View(programa);
        }

        //
        // POST: /Programa/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Programa programa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(programa);
        }

        //
        // GET: /Programa/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Programa programa = db.Programas.Find(id);
            if (programa == null)
            {
                return HttpNotFound();
            }
            return View(programa);
        }

        //
        // POST: /Programa/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Programa programa = db.Programas.Find(id);
            db.Programas.Remove(programa);
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