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
    public class CursoController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /Curso/



        public ActionResult Index(string searchString, int id = 0)
        {

            if (!String.IsNullOrEmpty(searchString))
            {
                var cursos = db.Cursoes.Where(r => r.nombre.ToUpper().Contains(searchString.ToUpper()));
                List<Curso> listest = cursos.ToList();

                return View(cursos.ToList());
            }
            else
            {
                if (id > 0)
                {

                }
                return View(db.Cursoes.ToList());
            }
        }
        

        //
        // GET: /Curso/Details/5

        public ActionResult Details(int id = 0)
        {
            Curso curso = db.Cursoes.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }

        //
        // GET: /Curso/Create 



        public ActionResult Create()
        {
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre");

            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Select", Value = "Select" });
            li.Add(new SelectListItem { Text = "Educación continuada", Value = "Educación continuada" });
            li.Add(new SelectListItem { Text = "Curso", Value = "Curso" });
            li.Add(new SelectListItem { Text = "Taller", Value = "Taller" });
            li.Add(new SelectListItem { Text = "Curso – Taller", Value = "Curso – Taller" });
            li.Add(new SelectListItem { Text = "Diplomado", Value = "Diplomado" });
            li.Add(new SelectListItem { Text = "Conferencia", Value = "Conferencia" });
            li.Add(new SelectListItem { Text = "Seminario", Value = "Seminario" });
            li.Add(new SelectListItem { Text = "Congreso", Value = "Congreso" });
            li.Add(new SelectListItem { Text = "Simposio", Value = "Simposio" });
            ViewData["country"] = li;
            
            return View();
        }

        

        public JsonResult GetStates(string id)
        {
            List<SelectListItem> states = new List<SelectListItem>();
            switch (id)
            {
                case "Educación continuada":
                    states.Add(new SelectListItem { Text = "Select", Value = "0" });
                    states.Add(new SelectListItem { Text = "Ateneo de medicina de urgencias", Value = "Ateneo de medicina de urgencias" });
                    states.Add(new SelectListItem { Text = "Ateneo medicina interna", Value = "Ateneo medicina interna" });
                    states.Add(new SelectListItem { Text = "Gran cátedra medicina interna", Value = "Gran cátedra medicina interna" });
                    states.Add(new SelectListItem { Text = "Ateneo geriatría", Value = "Ateneo geriatría" });
                    states.Add(new SelectListItem { Text = "Ateneo pediatría", Value = "Ateneo pediatría" });
                    states.Add(new SelectListItem { Text = "Cátedra Gustavo Isaza", Value = "Cátedra Gustavo Isaza" });
                    states.Add(new SelectListItem { Text = "Sesión de radiología", Value = "Sesión de radiología" });
                    states.Add(new SelectListItem { Text = "Imagenología para todos", Value = "Imagenología para todos" });
                    states.Add(new SelectListItem { Text = "Sesión conjunta del programa de medicina", Value = "Sesión conjunta del programa de medicina" });
                    states.Add(new SelectListItem { Text = "Clínica interinstitucional de maltrato infantil", Value = "Clínica interinstitucional de maltrato infantil" });
                    
                    break;
                case "Curso":
                    states.Add(new SelectListItem { Text = "Select", Value = "0" });
                    states.Add(new SelectListItem { Text = "Educación en diabetes", Value = "Educación en diabetes" });
                    states.Add(new SelectListItem { Text = "Dolor torácico: análisis: cardiovascular/gastrointestinal y pulmonar", Value = "Dolor torácico: análisis: cardiovascular/gastrointestinal y pulmonar" });
                    states.Add(new SelectListItem { Text = "Atención integral  en salud a las personas con VIH/SIDA", Value = "Atención integral  en salud a las personas con VIH/SIDA" });
                    states.Add(new SelectListItem { Text = "Buenas prácticas clínicas", Value = "Buenas prácticas clínicas" });
                    states.Add(new SelectListItem { Text = "Certificación de formación en las estrategias clínicas comunitarias y locales de intervención social", Value = "Certificación de formación en las estrategias clínicas comunitarias y locales de intervención social" });
                    states.Add(new SelectListItem { Text = "Atención integral de las víctimas de violencia  sexual", Value = "Atención integral de las víctimas de violencia  sexual" });
                    states.Add(new SelectListItem { Text = "Educación para la maternidad  a la familia gestante", Value = "Educación para la maternidad  a la familia gestante" });
                    states.Add(new SelectListItem { Text = "Actualización en buenas prácticas de esterilización en el área quirúrgica", Value = "Actualización en buenas prácticas de esterilización en el área quirúrgica" });
                    states.Add(new SelectListItem { Text = "Certificación  en cuidado a la persona en estado crítico de salud", Value = "Certificación  en cuidado a la persona en estado crítico de salud" });
                    states.Add(new SelectListItem { Text = "Certificación en urgencias", Value = "Certificación en urgencias" });
                    states.Add(new SelectListItem { Text = "Fundamentación teórica para la investigación  ", Value = "Fundamentación teórica para la investigación  " });
                    states.Add(new SelectListItem { Text = "Jornada de fármaco vigilancia", Value = "Jornada de fármaco vigilancia" });
                    states.Add(new SelectListItem { Text = "Certificación instructores BLS AHA", Value = "Certificación instructores BLS AHA" });
                    states.Add(new SelectListItem { Text = "Certificación instructores ACLS AHA", Value = "Certificación instructores ACLS AHA" });
                    states.Add(new SelectListItem { Text = "Primeros auxilios salva corazones DEA- AHA", Value = "Primeros auxilios salva corazones DEA- AHA" });
                    states.Add(new SelectListItem { Text = "BLS – AHA", Value = "BLS – AHA" });
                    states.Add(new SelectListItem { Text = "ACLS – AHA", Value = "ACLS – AHA" });
                    states.Add(new SelectListItem { Text = "RCP con apoyo al avanzado", Value = "RCP con apoyo al avanzado" });
                    states.Add(new SelectListItem { Text = "Básico y avanzado del manejo del trauma en el paciente adulto", Value = "Básico y avanzado del manejo del trauma en el paciente adulto" });
                    
                    break;
                case "Taller":
                    states.Add(new SelectListItem { Text = "Select", Value = "0" });
                    states.Add(new SelectListItem { Text = "Sobre la estrategia AIEPI ", Value = "Sobre la estrategia AIEPI " });
                    states.Add(new SelectListItem { Text = "Capacitación en los contenidos de la resolución 412 y en el Plan Decenal de Salud Publica", Value = "Capacitación en los contenidos de la resolución 412 y en el Plan Decenal de Salud Publica" });
                    states.Add(new SelectListItem { Text = "Higiene de manos en el marco del Sistema Obligatorio de Garantía de la Calidad", Value = "Higiene de manos en el marco del Sistema Obligatorio de Garantía de la Calidad" });
                    states.Add(new SelectListItem { Text = "Uso y reúso de dispositivos médicos", Value = "Uso y reúso de dispositivos médicos" });
                    states.Add(new SelectListItem { Text = "Manejo de residuos intrahospitalarios", Value = "Manejo de residuos intrahospitalarios" });
                    states.Add(new SelectListItem { Text = "Garantía de Calidad en central de esterilización", Value = "Garantía de Calidad en central de esterilización" });
                    states.Add(new SelectListItem { Text = "Humanización de la atención en salud", Value = "Humanización de la atención en salud" });
                    break;
                case "Curso – Taller":
                    states.Add(new SelectListItem { Text = "Select", Value = "0" });
                    states.Add(new SelectListItem { Text = "Competencias para la auscultación cardíaca", Value = "Competencias para la auscultación cardíaca" });
                    states.Add(new SelectListItem { Text = "Entrenamiento en procedimientos básicos", Value = "Entrenamiento en procedimientos básicos" });
                    states.Add(new SelectListItem { Text = "Entrenamiento en monitoria de signos vitales, lectura e interpretación de electrocardiograma", Value = "Entrenamiento en monitoria de signos vitales, lectura e interpretación de electrocardiograma" });
                    states.Add(new SelectListItem { Text = "Formación en la competencia de la toma de muestras de laboratorio", Value = "Formación en la competencia de la toma de muestras de laboratorio" });
                    states.Add(new SelectListItem { Text = "Administración segura de medicamentos", Value = "Administración segura de medicamentos" });
                    states.Add(new SelectListItem { Text = "Gases Arterio- venosos", Value = "Gases Arterio- venosos" });
                    states.Add(new SelectListItem { Text = "Básico de terapias de  reemplazo renal continuo", Value = "Básico de terapias de  reemplazo renal continuo" });
                    states.Add(new SelectListItem { Text = "Competencias en terapia intravenosa cardiovascular apoyada en simulación", Value = "Competencias en terapia intravenosa cardiovascular apoyada en simulación" });
                    states.Add(new SelectListItem { Text = "Actualización en atención al paciente de cuidado intensivo neonatal", Value = "Actualización en atención al paciente de cuidado intensivo neonatal" });
                    states.Add(new SelectListItem { Text = "Soporte ventilatorio en el recién nacido", Value = "Soporte ventilatorio en el recién nacido" });
                    states.Add(new SelectListItem { Text = "Reanimación pediátrica", Value = "Reanimación pediátrica" });
                    states.Add(new SelectListItem { Text = "Reanimación neonatal básico ( minuto de oro)", Value = "Reanimación neonatal básico ( minuto de oro)" });
                    states.Add(new SelectListItem { Text = "Ayudando a los bebes a sobrevivir", Value = "Ayudando a los bebes a sobrevivir" });
                    states.Add(new SelectListItem { Text = "Reanimación neonatal avanzada", Value = "Reanimación neonatal avanzada" });
                    states.Add(new SelectListItem { Text = "Gestión del riesgo en desastres, dirigido a líderes comunitarios", Value = "Gestión del riesgo en desastres, dirigido a líderes comunitarios" });
                    states.Add(new SelectListItem { Text = "Entrenamiento certificado en toma de citologías del cuello uterino", Value = "Entrenamiento certificado en toma de citologías del cuello uterino" });
                    states.Add(new SelectListItem { Text = "Anatomía y terminología médica", Value = "Anatomía y terminología médica" });
                    break;
                case "Indias":
                    break;
            }
            return Json(new SelectList(states, "Value", "Text"));
        }


        public JsonResult GetCity(string id)
        {
            List<SelectListItem> City = new List<SelectListItem>();
            switch (id)
            {
                case "20":
                    City.Add(new SelectListItem { Text = "Select", Value = "0" });
                    City.Add(new SelectListItem { Text = "MUMBAI", Value = "1" });
                    City.Add(new SelectListItem { Text = "PUNE", Value = "2" });
                    City.Add(new SelectListItem { Text = "KOLHAPUR", Value = "3" });
                    City.Add(new SelectListItem { Text = "RATNAGIRI", Value = "4" });
                    City.Add(new SelectListItem { Text = "NAGPUR", Value = "5" });
                    City.Add(new SelectListItem { Text = "JALGAON", Value = "6" });
                    break;
            }

            return Json(new SelectList(City, "Value", "Text"));
        }
       



        //
        // GET: /Curso/VistaContraprestacion
        public ActionResult VistaContraprestacion()
        {

            return View();

        }

        //
        // POST: /Curso/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Curso curso)
        {
            if (ModelState.IsValid)
            {


                curso.totalCapacitacion = (curso.valorUnitarioPersona + curso.valorUnitarioUniversidad) * curso.asignados;
                curso.valorTotalPersona = curso.valorUnitarioPersona * curso.asignados;
                curso.totalContraprestacion = curso.valorUnitarioUniversidad * curso.asignados;
                curso.valorTotalUniversidad = curso.valorUnitarioUniversidad * curso.asignados;
                curso.porcentajeTotalPersona = (curso.valorTotalPersona * 100) / curso.totalCapacitacion;
                curso.porcentajeTotalUniversidad = (curso.valorTotalUniversidad * 100) / curso.totalCapacitacion;

                curso.fechaCreacion = DateTime.Now.Date;

                db.Cursoes.Add(curso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre", curso.IPS_ESEId);
                return View(curso);

            }
         
        }

        //
        // GET: /Curso/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Curso curso = db.Cursoes.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre", curso.IPS_ESEId);
            return View(curso);


        }



        //
        // POST: /Curso/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Curso curso)
        {
            if (ModelState.IsValid)
            {
                curso.fechaInicio = curso.fechaInicio;
                curso.totalCapacitacion = (curso.valorUnitarioPersona + curso.valorUnitarioUniversidad) * curso.asignados;
                curso.valorTotalPersona = curso.valorUnitarioPersona * curso.asignados;
                curso.totalContraprestacion = curso.valorUnitarioUniversidad * curso.asignados;
                curso.valorTotalUniversidad = curso.valorUnitarioUniversidad * curso.asignados;
                curso.porcentajeTotalPersona = (curso.valorTotalPersona * 100) / curso.totalCapacitacion;
                curso.porcentajeTotalUniversidad = (curso.valorTotalUniversidad * 100) / curso.totalCapacitacion;
                db.Entry(curso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre", curso.IPS_ESEId);
            return View(curso);
        }
           

            //curso.fechaCreacion = DateTime.Now.Date;

            //db.Cursoes.Add(curso);
           





        //
        // GET: /Curso/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Curso curso = db.Cursoes.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }

        //
        // POST: /Curso/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Curso curso = db.Cursoes.Find(id);
            db.Cursoes.Remove(curso);
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