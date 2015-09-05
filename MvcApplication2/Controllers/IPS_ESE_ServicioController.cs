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
    public class IPS_ESE_ServicioController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /IPS_ESE_Servicio/

        public ActionResult Index()
        {
         
            var ips_ese_servicio = db.IPS_ESE_Servicio.Include(i => i.IPS_ESE).Include(i => i.Servicio);
            return View(ips_ese_servicio.ToList());
        }

        //
        // GET: /IPS_ESE_Servicio/Details/5

        public ActionResult Details(int id = 0)
        {
            IPS_ESE_Servicio ips_ese_servicio = db.IPS_ESE_Servicio.Find(id);
            if (ips_ese_servicio == null)
            {
                return HttpNotFound();
            }
            return View(ips_ese_servicio);
        }

        //
        // GET: /IPS_ESE_Servicio/Create

        public ActionResult Create()
        {

            var municipios = db.IPS_ESE.Include(h => h.Municipio);
            List<IPS_ESE> lista = municipios.ToList();

            ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");
            ViewBag.servicioId = new SelectList(db.Servicios, "servicioId", "nombre");
            return View();
        }

        //
        // POST: /IPS_ESE_Servicio/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IPS_ESE_Servicio ips_ese_servicio)
        {
            if (ModelState.IsValid)
            {
                db.IPS_ESE_Servicio.Add(ips_ese_servicio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "origen", ips_ese_servicio.IPS_ESEId);
            ViewBag.servicioId = new SelectList(db.Servicios, "servicioId", "nombre", ips_ese_servicio.servicioId);
            return View(ips_ese_servicio);
        }

        //
        // GET: /IPS_ESE_Servicio/Edit/5

        public ActionResult Edit(int id = 0)
        {
            IPS_ESE_Servicio ips_ese_servicio = db.IPS_ESE_Servicio.Find(id);
            if (ips_ese_servicio == null)
            {
                return HttpNotFound();
            }
            var municipios = db.IPS_ESE.Include(h => h.Municipio);
            List<IPS_ESE> lista = municipios.ToList();

            ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre"); 
            ViewBag.servicioId = new SelectList(db.Servicios, "servicioId", "nombre", ips_ese_servicio.servicioId);
            return View(ips_ese_servicio);
        }

        //
        // POST: /IPS_ESE_Servicio/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IPS_ESE_Servicio ips_ese_servicio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ips_ese_servicio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "origen", ips_ese_servicio.IPS_ESEId);
            ViewBag.servicioId = new SelectList(db.Servicios, "servicioId", "nombre", ips_ese_servicio.servicioId);
            return View(ips_ese_servicio);
        }

        //
        // GET: /IPS_ESE_Servicio/Delete/5

        public ActionResult Delete(int id = 0)
        {
            IPS_ESE_Servicio ips_ese_servicio = db.IPS_ESE_Servicio.Find(id);
            if (ips_ese_servicio == null)
            {
                return HttpNotFound();
            }
            return View(ips_ese_servicio);
        }

        //
        // POST: /IPS_ESE_Servicio/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IPS_ESE_Servicio ips_ese_servicio = db.IPS_ESE_Servicio.Find(id);
            db.IPS_ESE_Servicio.Remove(ips_ese_servicio);
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