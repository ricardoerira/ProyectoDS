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
    public class FamiliaController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /Familia/

        public ActionResult Index()
        {
            return View(db.Familias.ToList());
        }

        //
        // GET: /Familia/Details/5

        public ActionResult Details(int id = 0)
        {
            Familia familia = db.Familias.Find(id);
            if (familia == null)
            {
                return HttpNotFound();
            }
            return View(familia);
        }

        //
        // GET: /Familia/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Familia/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Familia familia)
        {
            if (ModelState.IsValid)
            {
                db.Familias.Add(familia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(familia);
        }

        //
        // GET: /Familia/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Familia familia = db.Familias.Find(id);
            if (familia == null)
            {
                return HttpNotFound();
            }
            return View(familia);
        }

        //
        // POST: /Familia/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Familia familia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(familia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(familia);
        }

        //
        // GET: /Familia/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Familia familia = db.Familias.Find(id);
            if (familia == null)
            {
                return HttpNotFound();
            }
            return View(familia);
        }

        //
        // POST: /Familia/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Familia familia = db.Familias.Find(id);
            db.Familias.Remove(familia);
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