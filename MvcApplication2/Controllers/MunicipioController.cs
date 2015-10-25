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
    public class MunicipioController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /Municipio/

        public ActionResult Index()
        {
            var municipios = db.Municipios.Include(m => m.Departamento);
            return View(municipios.ToList());
        }

        //
        // GET: /Municipio/Details/5

        public ActionResult Details(int id = 0)
        {
            Municipio municipio = db.Municipios.Find(id);
            if (municipio == null)
            {
                return HttpNotFound();
            }
            return View(municipio);
        }

        //
        // GET: /Municipio/Create

        public ActionResult Create()
        {
            ViewBag.departamentoId = new SelectList(db.Departamentoes, "departamentoID", "nombre");
            return View();
        }

        //
        // POST: /Municipio/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Municipio municipio)
        {
            if (ModelState.IsValid)
            {
                db.Municipios.Add(municipio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.departamentoId = new SelectList(db.Departamentoes, "departamentoID", "nombre", municipio.departamentoId);
            return View(municipio);


        }

        //
        // GET: /Municipio/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Municipio municipio = db.Municipios.Find(id);
            if (municipio == null)
            {
                return HttpNotFound();
            }
            ViewBag.departamentoId = new SelectList(db.Departamentoes, "departamentoID", "nombre", municipio.departamentoId);
            return View(municipio);
        }

        //
        // POST: /Municipio/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Municipio municipio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(municipio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.departamentoId = new SelectList(db.Departamentoes, "departamentoID", "nombre", municipio.departamentoId);
            return View(municipio);
        }

        //
        // GET: /Municipio/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Municipio municipio = db.Municipios.Find(id);
            if (municipio == null)
            {
                return HttpNotFound();
            }
            return View(municipio);
        }

        //
        // POST: /Municipio/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Municipio municipio = db.Municipios.Find(id);
            db.Municipios.Remove(municipio);
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