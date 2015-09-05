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
    public class PostgradoController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /Postgrado/

        public ActionResult Index()
        {
            return View(db.Postgradoes.ToList());
        }
   
        //
        // GET: /Postgrado/Details/5

        public ActionResult Details(int id = 0)
        {
            Postgrado postgrado = db.Postgradoes.Find(id);
            if (postgrado == null)
            {
                return HttpNotFound();
            }
            return View(postgrado);
        }

        //
        // GET: /Postgrado/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Postgrado/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Postgrado postgrado)
        {
            if (ModelState.IsValid)
            {
                db.Postgradoes.Add(postgrado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(postgrado);
        }

        //
        // GET: /Postgrado/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Postgrado postgrado = db.Postgradoes.Find(id);
            if (postgrado == null)
            {
                return HttpNotFound();
            }
            return View(postgrado);
        }

        //
        // POST: /Postgrado/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Postgrado postgrado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(postgrado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(postgrado);
        }

        //
        // GET: /Postgrado/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Postgrado postgrado = db.Postgradoes.Find(id);
            if (postgrado == null)
            {
                return HttpNotFound();
            }
            return View(postgrado);
        }

        //
        // POST: /Postgrado/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Postgrado postgrado = db.Postgradoes.Find(id);
            db.Postgradoes.Remove(postgrado);
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