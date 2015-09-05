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
    public class DepartamentoSaludController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /DepartamentoSalud/
        

        public ActionResult VistaDepartamentoSalud()
        {
            bool estado = User.IsInRole("Departamento");
            if (!estado)
            {
                return RedirectToAction("../Account/Login");
            }
            else
            {
                return View();


            }

        }
       
        public ActionResult BuscarEnVacuna(Docente docente)
        {
            var docentes = from b in db.Docentes
                           select b;

            foreach (var b in docentes)
            {
                if (b.num_documento.Equals(docente.num_documento))
                {
                    docente = b;
                }

            }
            if (docente.docenteId == 0)
            {
                return View(docente);
            }
            else
            {
                return RedirectToAction("../Estudiante/PersonalesDS/" + docente.num_documento);
            }
        }
        public ActionResult Index()
        {
            return View(db.DepartamentoSaluds.ToList());
        }



        //
        // GET: /DepartamentoSalud/Details/5

        public ActionResult Details(int id = 0)
        {
            DepartamentoSalud departamentosalud = db.DepartamentoSaluds.Find(id);
            if (departamentosalud == null)
            {
                return HttpNotFound();
            }
            return View(departamentosalud);
        }

        //
        // GET: /DepartamentoSalud/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DepartamentoSalud/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartamentoSalud departamentosalud)
        {
            if (ModelState.IsValid)
            {
                db.DepartamentoSaluds.Add(departamentosalud);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(departamentosalud);
        }

        //
        // GET: /DepartamentoSalud/Edit/5

        public ActionResult Edit(int id = 0)
        {
            DepartamentoSalud departamentosalud = db.DepartamentoSaluds.Find(id);
            if (departamentosalud == null)
            {
                return HttpNotFound();
            }
            return View(departamentosalud);
        }

        //
        // POST: /DepartamentoSalud/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepartamentoSalud departamentosalud)
        {
            if (ModelState.IsValid)
            {
                db.Entry(departamentosalud).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(departamentosalud);
        }

        //
        // GET: /DepartamentoSalud/Delete/5

        public ActionResult Delete(int id = 0)
        {
            DepartamentoSalud departamentosalud = db.DepartamentoSaluds.Find(id);
            if (departamentosalud == null)
            {
                return HttpNotFound();
            }
            return View(departamentosalud);
        }

        //
        // POST: /DepartamentoSalud/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DepartamentoSalud departamentosalud = db.DepartamentoSaluds.Find(id);
            db.DepartamentoSaluds.Remove(departamentosalud);
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