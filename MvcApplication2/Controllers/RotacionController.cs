using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;
using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;
using Microsoft.Reporting.WebForms;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.SqlClient;
using System.Data.Entity.Validation;

namespace MvcApplication2.Controllers
{
    public class RotacionController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /Rotacion/

        public ActionResult Index()
        {
             importaGruposMateria();
            //importaEstudiantesRotacion();
            var rotacions = db.Rotacions.Include(r => r.ActividadAcademica).Include(r => r.IPS_ESE);
            return View(rotacions.ToList());
        }

        public void importaEstudiantesRotacion()
        {
            ServiceReference1.WSFacultadSaludSoapClient ser = new ServiceReference1.WSFacultadSaludSoapClient();

            List<Rotacion> rotaciones = db.Rotacions.ToList();
            foreach (var item in rotaciones)
            {
                string json2 = ser.getInscritosGrupo(item.ActividadAcademica.codigo_AA, item.grupo, item.year_academico + "", item.periodo_academico + "");
                if (json2 != null && !json2.Equals(""))
                {
                  
                      try
                        {
                    MvcApplication2.Models.GruposInscritos.ESObject0 gruposInscritos = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.GruposInscritos.ESObject0>(json2);
                    foreach (var item3 in gruposInscritos.inscritosGrupo)
                    {
                        long codigo = Int64.Parse(item3.CODIGO);
                        string cedula = item3.CEDULA_PROFESOR;
                        Docente docente2 = null;
                        Estudiante estudiante2 = null;
                      
                            cedula = cedula.Substring(0, cedula.Length - 1);



                        if (!cedula.Equals(""))
                        {

                            var docente = db.Docentes.Where(r => r.num_documento == cedula);
                         
                                docente2 = (Docente)docente.ToList().ElementAt(0);
                                docente2.rotacionId = item.rotacionId;

                           


                        }
                        var estudiante = db.Estudiantes.Where(r => r.codigo == codigo);

                      
                            estudiante2 = (Estudiante)estudiante.ToList().ElementAt(0);
                            estudiante2.rotacionId = item.rotacionId;
                      

                        RotacionEstudiante re = new RotacionEstudiante();
                        re.docenteId = docente2.docenteId;
                        re.estudianteId = estudiante2.estudianteId;
                        re.rotacionId = item.rotacionId;
                        re.IPS_ESEId = 1;
                        db.RotacionEstudiantes.Add(re);
                        db.SaveChanges();




                       

                    }
                        }
                      catch (Exception e)
                      {
                          continue;
                      }
                }
            }

        }
        public void importaGruposMateria()
        {
            List<ActividadAcademica> materias = db.ActividadAcademicas.ToList();
            foreach (var item in materias)
            {
                ServiceReference1.WSFacultadSaludSoapClient ser = new ServiceReference1.WSFacultadSaludSoapClient();

                string json = ser.getGruposMateria(item.codigo_AA);
                if (json != null && !json.Equals(""))
                {


                    MvcApplication2.Models.Grupos.ESObject0 gruposMaterias = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.Grupos.ESObject0>(json);
                    foreach (var item2 in gruposMaterias.gruposMaterias)
                    {
                      


                        if (item2.ANO >= 2015 && item2.PERIODO == 2)
                        {
                            var datos = db.Rotacions.Where(r => r.actividadacademicaId == item.actividadacademicaId).Where(r => r.year_academico == item2.ANO).Where(r => r.periodo_academico == item2.PERIODO).Where(r => r.grupo.Equals(item2.GRUPO));
                            List<Rotacion> lista = datos.ToList();
                            if (lista.Count() == 0)
                            {
                                Rotacion rotacion = new Rotacion();
                                rotacion.year_academico = item2.ANO;
                                rotacion.periodo_academico = item2.PERIODO;

                          
                                rotacion.grupo = item2.GRUPO;

                                rotacion.horario = "";
                                rotacion.numero_estudiantes = item2.INSCRITOS;
                                DateTime myDate = DateTime.ParseExact(item2.FECHA_INICIO, "dd/MM/yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                rotacion.fecha_inicio = myDate;

                                DateTime myDate2 = DateTime.ParseExact(item2.FECHA_FINAL, "dd/MM/yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                rotacion.fecha_terminacion = myDate2;
                                rotacion.actividadacademicaId = item.actividadacademicaId;
                                rotacion.IPS_ESEId = 1;
                                rotacion.grupo = item2.GRUPO;
                                db.Rotacions.Add(rotacion);
                                db.SaveChanges();
                                string json2 = ser.getInscritosGrupo(item2.COD_MATERIA, item2.GRUPO, item2.ANO + "", item2.PERIODO + "");
                                if (json2 != null && !json2.Equals(""))
                                {
                                    MvcApplication2.Models.GruposInscritos.ESObject0 gruposInscritos = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.GruposInscritos.ESObject0>(json2);
                                    foreach (var item3 in gruposInscritos.inscritosGrupo)
                                    {
                                        long codigo = Int64.Parse(item3.CODIGO);
                                        string cedula = item3.CEDULA_PROFESOR;
                                        var iffam = db.Rotacions.Max(p => p.rotacionId);
                                        Docente docente2 = null;
                                        Estudiante estudiante2 = null;
                                        if (!cedula.Equals(""))
                                        {

                                            cedula = cedula.Substring(0, cedula.Length - 1);
                                            var docente = db.Docentes.Where(r => r.num_documento == cedula);
                                            List<Docente> listest2 = docente.ToList();

                                            if (listest2.Count > 0)
                                            {
                                                docente2 = listest2.ElementAt(0);
                                                docente2.rotacionId = iffam;
                                            }

                                        }
                                        var estudiante = db.Estudiantes.Where(r => r.codigo == codigo);
                                        List<Estudiante> listest = estudiante.ToList();

                                        if (listest.Count > 0)
                                        {
                                            estudiante2 = listest.ElementAt(0);
                                            estudiante2.rotacionId = iffam;
                                        }
                                        if (estudiante2 != null && docente2 != null)
                                        {
                                            RotacionEstudiante re = new RotacionEstudiante();
                                            re.docenteId = docente2.docenteId;
                                            re.estudianteId = estudiante2.estudianteId;
                                            re.rotacionId = iffam;
                                            re.IPS_ESEId = 1;
                                            re.horario = "";
                                            db.RotacionEstudiantes.Add(re);
                                            try
                                            {
   
                                            db.SaveChanges();
                                            }
catch (DbEntityValidationException e)
{
    foreach (var eve in e.EntityValidationErrors)
    {
        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            eve.Entry.Entity.GetType().Name, eve.Entry.State);
        foreach (var ve in eve.ValidationErrors)
        {
            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                ve.PropertyName, ve.ErrorMessage);
        }
    }
    throw;
}
                                        }




                                    }

                                }


                            }
                        }
                    }
                }
            }
        }
      

        

        public ActionResult SeleccionRotacion()
        {
            return View(db.Rotacions.ToList());
        }
        public ActionResult SeleccionRotacionCarta()
        {

            return View(db.Rotacions.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConsultaRotacion(int id = 0)
        {
            return RedirectToAction("Estudiante/RotacionEstudiante/" + id);
        }
        public ActionResult ConsultaRotacion()
        {
            return View(db.Rotacions.ToList());
        }
        public ActionResult ConsultaEstudiantes(int id = 0)
        {
            Rotacion rotacion = db.Rotacions.Find(id);
            if (rotacion == null)
            {
                return HttpNotFound();
            }
            return View(rotacion);
        }
        public ActionResult VistaODS()
        {
            bool estado = User.IsInRole("DocenciaServicio");
            if(!estado)
            {
                return RedirectToAction("../Account/Login");
            }
            else {
                return View();  


            }           

        }

        public DataTable ToDataTable<T>(T entity) where T : class
        {
            var properties = typeof(T).GetProperties();
            var table = new DataTable();

            foreach (var property in properties)
            {
                table.Columns.Add(property.Name, property.PropertyType);
            }

            table.Rows.Add(properties.Select(p => p.GetValue(entity, null)).ToArray());
            return table;
        }
        public ActionResult GeneraReporte(int id = 0)
        {




            Rotacion rotacion = db.Rotacions.Find(id);
        
          ReportDocument rptH = new ReportDocument();
          string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/reporte.rpt");
          rptH.Load(strRptPath);

         
          rptH.Database.Tables[0].SetDataSource(db.Estudiantes.ToList());
          rptH.Database.Tables[1].SetDataSource(db.HojaVidas.ToList());
          rptH.Database.Tables[2].SetDataSource(db.Rotacions.ToList());
               
rptH.SetParameterValue("Nombre_Doctor", rotacion.IPS_ESE.representante_legal);
rptH.SetParameterValue("cargo", rotacion.IPS_ESE.cargo_representanteDS);
rptH.SetParameterValue("clinica", rotacion.IPS_ESE.nombre);
rptH.SetParameterValue("presentacion", "A continuación le relaciono las rotaciones de los estudiantes del Programa de " + rotacion.Estudiante.ElementAt(0).Programa.nombre + ", que realizaran su rotación en su institución y los profesores con su horario.");
rptH.SetParameterValue("docente", rotacion.Docente.ElementAt(0).HojaVida.primer_nombre + " " + rotacion.Docente.ElementAt(0).HojaVida.primer_apellido + " " + rotacion.Docente.ElementAt(0).HojaVida.segundo_apellido);
rptH.SetParameterValue("fecha", rotacion.fecha_inicio);






Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

return File(stream, "application/pdf");
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeleccionRotacion(string searchString, int id = 0)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                var rotaciones = db.Rotacions.Where(r => r.ActividadAcademica.nombre.ToUpper().Contains(searchString.ToUpper())
                    || r.ActividadAcademica.DepartamentoSalud.nombre.ToUpper().Contains(searchString.ToUpper()));
                List<Rotacion> listest = rotaciones.ToList();

                return View(rotaciones.ToList());
            }
            else
            {
                if (id > 0)
                {

                }
                return View(db.Rotacions.ToList());
            }


        }
        //
        // GET: /Rotacion/Details/5

        public ActionResult Details(int id = 0)
        {
            Rotacion rotacion = db.Rotacions.Find(id);
            if (rotacion == null)
            {
                return HttpNotFound();
            }
            return View(rotacion);
        }

        //
        // GET: /Rotacion/Create




        public ActionResult Create()
        {
            ViewBag.actividadacademicaId = new SelectList(db.ActividadAcademicas, "actividadacademicaId", "nombre");
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre");
            return View();
        }

        //
        // POST: /Rotacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rotacion rotacion)
        {
            if (ModelState.IsValid)
            {
                db.Rotacions.Add(rotacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.actividadacademicaId = new SelectList(db.ActividadAcademicas, "actividadacademicaId", "tipo", rotacion.actividadacademicaId);
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "origen", rotacion.IPS_ESEId);
            return View(rotacion);
        }





        //
        // GET: /Rotacion/Edit/5


        //
        // GET: /Rotacion/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Rotacion rotacion = db.Rotacions.Find(id);
            if (rotacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre", rotacion.IPS_ESEId);
            ViewBag.actividadacademicaId = new SelectList(db.ActividadAcademicas, "actividadacademicaId", "nombre", rotacion.actividadacademicaId);

            return View(rotacion);
        }




        //
        // POST: /Rotacion/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Rotacion rotacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rotacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SeleccionRotacion");
            }
            return View(rotacion);
        }


        //
        // GET: /Rotacion/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Rotacion rotacion = db.Rotacions.Find(id);
            if (rotacion == null)
            {
                return HttpNotFound();
            }
            return View(rotacion);
        }



        //
        // POST: /Rotacion/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rotacion rotacion = db.Rotacions.Find(id);
            db.Rotacions.Remove(rotacion);
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