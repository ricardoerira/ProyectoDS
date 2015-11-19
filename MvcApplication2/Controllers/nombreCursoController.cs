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
    public class nombreCursoController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /nombreCurso/

        public ActionResult Index()
        {
            var nombrecursoes = db.nombreCursoes.Include(n => n.Curso);
            return View(nombrecursoes.ToList());
        }

        //
        // GET: /nombreCurso/Details/5

        public ActionResult Details(int id = 0)
        {
            nombreCurso nombrecurso = db.nombreCursoes.Find(id);
            if (nombrecurso == null)
            {
                return HttpNotFound();
            }
            return View(nombrecurso);
        }

        //
        // GET: /nombreCurso/Create

        public ActionResult Create()
        {
            ViewBag.CursoId = new SelectList(db.Cursoes, "CursoId", "tipo");
            return View();
        }

        //
        // POST: /nombreCurso/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(nombreCurso nombrecurso)
        {
            if (ModelState.IsValid)
            {
                db.nombreCursoes.Add(nombrecurso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CursoId = new SelectList(db.Cursoes, "CursoId", "tipo", nombrecurso.CursoId);
            return View(nombrecurso);
        }

        //
        // GET: /nombreCurso/Edit/5

        public ActionResult Edit(int id = 0)
        {
            nombreCurso nombrecurso = db.nombreCursoes.Find(id);
            if (nombrecurso == null)
            {
                return HttpNotFound();
            }
            ViewBag.CursoId = new SelectList(db.Cursoes, "CursoId", "tipo", nombrecurso.CursoId);
            return View(nombrecurso);
        }

        //
        // POST: /nombreCurso/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(nombreCurso nombrecurso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nombrecurso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CursoId = new SelectList(db.Cursoes, "CursoId", "tipo", nombrecurso.CursoId);
            return View(nombrecurso);
        }

        //
        // GET: /nombreCurso/Delete/5

        public ActionResult Delete(int id = 0)
        {
            nombreCurso nombrecurso = db.nombreCursoes.Find(id);
            if (nombrecurso == null)
            {
                return HttpNotFound();
            }
            return View(nombrecurso);
        }

        //
        // POST: /nombreCurso/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            nombreCurso nombrecurso = db.nombreCursoes.Find(id);
            db.nombreCursoes.Remove(nombrecurso);
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