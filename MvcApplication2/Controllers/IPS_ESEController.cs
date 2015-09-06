using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace MvcApplication2.Controllers
{
    public class IPS_ESEController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /IPS_ESE/

        public ActionResult Index()
        {
            
            var ips_ese = db.IPS_ESE.Include(i => i.Municipio);
            return View(ips_ese.ToList());
        }



        

        public ActionResult VistaIPS_ESE()
        {
            bool estado = User.IsInRole("IPS");
            if (!estado)
            {
                return RedirectToAction("../Account/Login");
            }
            else
            {
                return View();


            }

        }
        public ActionResult SeleccionRotacionCarta()
        {

            var municipios = db.IPS_ESE.Include(h => h.Municipio);
            List<IPS_ESE> lista = municipios.ToList();
            
            ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");
            
            ViewBag.programaId = new SelectList(db.Programas, "programaId", "nombre");

            ViewBag.DepartamentoSaludId = new SelectList(db.DepartamentoSaluds, "DepartamentoSaludId", "nombre");
            

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeleccionRotacionCarta(IPS_ESE s,FormCollection value)
        {

            IPS_ESE ips = db.IPS_ESE.Find(s.IPS_ESEId);

            int programaId =Int32.Parse( value["programaId"]);
            int departamentoId = Int32.Parse(value["DepartamentoSaludId"]);
            DepartamentoSalud ds = db.DepartamentoSaluds.Find(departamentoId);
            Programa pr= db.Programas.Find(programaId);
            int mesId = Int32.Parse(value["mesId"]);
            int añoId = Int32.Parse(value["añoId"]);
            var date = DateTime.MinValue;
           DateTime.TryParse(añoId + "/" +mesId+ "/01", out date);
         
           DateTime date2 = new DateTime(añoId, mesId,
                                     DateTime.DaysInMonth(añoId, mesId));
            ReportDocument rptH = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/reporte.rpt");
            rptH.Load(strRptPath);
            List<RotacionEstudiante> re = db.RotacionEstudiantes.Include(h => h.Docente).Where(r => r.IPS_ESEId == ips.IPS_ESEId).Where(r => r.Estudiante.programaId == programaId).Where(r => r.Rotacion.fecha_inicio >= date).Where(r => r.Rotacion.fecha_terminacion <= date2).ToList();
            List<Docente> docentes = new List<Docente>();
            List<Estudiante> estudiantes = new List<Estudiante>();
            List<Rotacion> rotaciones = new List<Rotacion>();
            List<ActividadAcademica> acti = new List<ActividadAcademica>();
            List<HojaVida> hojas = new List<HojaVida>();
            List<HojaVida> hojas2 = new List<HojaVida>();
            foreach (var item in re)
            {
                docentes.Add(item.Docente);
                hojas.Add(item.Docente.HojaVida);
                estudiantes.Add(item.Estudiante);
                hojas2.Add(item.Estudiante.HojaVida);
                rotaciones.Add(item.Rotacion);
                acti.Add(item.Rotacion.ActividadAcademica);
            }
            rptH.Database.Tables[0].SetDataSource(re);
            rptH.Database.Tables[1].SetDataSource(docentes);
            rptH.Database.Tables[2].SetDataSource(estudiantes);
            rptH.Database.Tables[3].SetDataSource(hojas2);
            rptH.Database.Tables[4].SetDataSource(acti);
            rptH.Database.Tables[5].SetDataSource(rotaciones);
            //    rptH.Database.Tables[6].SetDataSource(hojas);




            rptH.SetParameterValue("presentacion", "A continuación le relaciono las rotaciones de los estudiantes del Programa de "+pr.nombre+" Departamento "+ds.nombre+" que realizaran su rotación en su institución y los profesores con su horario.");
            rptH.SetParameterValue("fecha", "");
            rptH.SetParameterValue("dr", ips.representante_legal);
            rptH.SetParameterValue("cargo",ips.cargo);
            rptH.SetParameterValue("nombreIPS", ips.nombre);
            




            if(re.Count>0)
            {
                Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                return File(stream, "application/pdf");
            }
            else
            {
                var municipios = db.IPS_ESE.Include(h => h.Municipio);
                List<IPS_ESE> lista = municipios.ToList();

                ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");

                ViewBag.programaId = new SelectList(db.Programas, "programaId", "nombre");

                ViewBag.DepartamentoSaludId = new SelectList(db.DepartamentoSaluds, "DepartamentoSaludId", "nombre");
            
                ViewBag.AlertMessage = "No se encontraron resultados";
                return View();
            }

         
        }


        public ActionResult SeleccionRotacionContraPrestacionC()
        {
          
            var municipios = db.IPS_ESE.Include(h => h.Municipio);
            List<IPS_ESE> lista = municipios.ToList();
            ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeleccionRotacionContraPrestacionC(IPS_ESE ipss)
        {

            IPS_ESE ips = db.IPS_ESE.Find(ipss.IPS_ESEId);
            string fecha = ViewBag.fecha;


            List<Curso> cursos = new List<Curso>();
            cursos = db.Cursoes.ToList();
            ReportDocument rptH = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/ReporteContraPrestacionC.rpt");
            rptH.Load(strRptPath);

           
     

          
            rptH.Database.Tables[0].SetDataSource(db.Cursoes.Where(d => d.IPS_ESEId == ips.IPS_ESEId).ToList());


            rptH.SetParameterValue("ips", ips.nombre);
            rptH.SetParameterValue("fecha", "");
            int total = 0;
         
            if (db.Induccions.Where(d => d.IPS_ESEId == ips.IPS_ESEId).ToList().Count > 0)
            {
                total += db.Cursoes.Where(d => d.IPS_ESEId == ips.IPS_ESEId).Sum(d => d.totalCapacitacion);

            }
            
            rptH.SetParameterValue("total", total);





            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);




            return File(stream, "application/pdf");
          
        }


        public ActionResult SeleccionRotacionContraPrestacionI()
        {

            var municipios = db.IPS_ESE.Include(h => h.Municipio);
            List<IPS_ESE> lista = municipios.ToList();
            ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeleccionRotacionContraPrestacionI(IPS_ESE ipss)
        {

            IPS_ESE ips = db.IPS_ESE.Find(ipss.IPS_ESEId);
            string fecha = ViewBag.fecha;


            List<Curso> cursos = new List<Curso>();
            cursos = db.Cursoes.ToList();
            ReportDocument rptH = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/ReporteContraPrestacionI.rpt");
            rptH.Load(strRptPath);




            rptH.Database.Tables[0].SetDataSource(db.Induccions.Where(d => d.IPS_ESEId == ips.IPS_ESEId).ToList());

           

            rptH.SetParameterValue("ips", ips.nombre);
            rptH.SetParameterValue("fecha", "");
            int total = 0;
           
            if (db.Induccions.Where(d => d.IPS_ESEId == ips.IPS_ESEId).ToList().Count > 0)
            {
                total += db.Induccions.Where(d => d.IPS_ESEId == ips.IPS_ESEId).Sum(d => d.valor);

            }

            rptH.SetParameterValue("total", total);





            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);




            return File(stream, "application/pdf");

        }

        public ActionResult SeleccionRotacionContraPrestacionE()
        {

            var municipios = db.IPS_ESE.Include(h => h.Municipio);
            List<IPS_ESE> lista = municipios.ToList();
            ViewBag.IPS_ESEId = new SelectList(lista, "IPS_ESEId", "nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeleccionRotacionContraPrestacionE(IPS_ESE ipss)
        {

            IPS_ESE ips = db.IPS_ESE.Find(ipss.IPS_ESEId);
            string fecha = ViewBag.fecha;



            ReportDocument rptH = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/ReporteEquipos.rpt");
            rptH.Load(strRptPath);



            int total =db.Equipoes.Sum((c => c.costo));
            rptH.Database.Tables[0].SetDataSource(db.Equipoes.ToList());

          


            rptH.SetParameterValue("ips", ips.nombre);
            rptH.SetParameterValue("total", total+"");
            rptH.SetParameterValue("fecha", "");






            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);




            return File(stream, "application/pdf");

        }
        public ActionResult GeneraReporte(int id = 0)
        {

            IPS_ESE ips = db.IPS_ESE.Find(id);

          
            
            ReportDocument rptH = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/reporte.rpt");
            rptH.Load(strRptPath);


            rptH.Database.Tables[0].SetDataSource(db.Estudiantes.ToList());
            rptH.Database.Tables[1].SetDataSource(db.HojaVidas.ToList());
            rptH.Database.Tables[2].SetDataSource(db.Rotacions.ToList());

            

           
            rptH.SetParameterValue("presentacion", "A continuación le relaciono las rotaciones de los estudiantes que realizaran su rotación en su institución y los profesores con su horario.");
            rptH.SetParameterValue("fecha", "");






            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return File(stream, "application/pdf");

        }
     
        //
        // GET: /IPS_ESE/Details/5

        public ActionResult Details(int id = 0)
        {
            IPS_ESE ips_ese = db.IPS_ESE.Find(id);
            if (ips_ese == null)
            {
                return HttpNotFound();
            }
            return View(ips_ese);
        }

        //
        // GET: /IPS_ESE/Create

        public ActionResult Create()
        {
            var municipios = db.Municipios.Include(h => h.Departamento);
            List<Municipio> lista = municipios.ToList();

            ViewBag.municipioId = new SelectList(lista, "municipioId", "nombre");
            
            return View();
        }

        //
        // POST: /IPS_ESE/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IPS_ESE ips_ese)
        {
            if (ModelState.IsValid)
            {
                db.IPS_ESE.Add(ips_ese);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.municipioId = new SelectList(db.Municipios, "municipioId", "nombre", ips_ese.municipioId);
            return View(ips_ese);
        }

        //
        // GET: /IPS_ESE/Edit/5

        public ActionResult Edit(int id = 0)
        {
            IPS_ESE ips_ese = db.IPS_ESE.Find(id);
            if (ips_ese == null)
            {
                return HttpNotFound();
            }
            ViewBag.municipioId = new SelectList(db.Municipios, "municipioId", "nombre", ips_ese.municipioId);
            return View(ips_ese);
        }

        //
        // POST: /IPS_ESE/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IPS_ESE ips_ese)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ips_ese).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.municipioId = new SelectList(db.Municipios, "municipioId", "nombre", ips_ese.municipioId);
            return View(ips_ese);
        }

        //
        // GET: /IPS_ESE/Delete/5

        public ActionResult Delete(int id = 0)
        {
            IPS_ESE ips_ese = db.IPS_ESE.Find(id);
            if (ips_ese == null)
            {
                return HttpNotFound();
            }
            return View(ips_ese);
        }

        //
        // POST: /IPS_ESE/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IPS_ESE ips_ese = db.IPS_ESE.Find(id);
            db.IPS_ESE.Remove(ips_ese);
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