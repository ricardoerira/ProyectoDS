using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;
using System.Data.SqlTypes;
using System.Net;
using System.IO;
using System.Text;

namespace MvcApplication2.Controllers
{
    public class HojaVidaController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /HojaVida/
        public ActionResult vistaHV()
        {
            return View();

        }

        public ActionResult HV_IPS_Universitaria()
        {

            return View();

        }

    public ActionResult HV_Departamento()
        {

            return View();

        }
        public void importaDocentes()
        {

            List<DepartamentoSalud> departamentos = db.DepartamentoSaluds.ToList();
            foreach (var item in departamentos)
            {
                ServiceReference1.WSFacultadSaludSoapClient ser = new ServiceReference1.WSFacultadSaludSoapClient();
                 string json;
               
            try{
                 json = ser.getProfesoresActivos(item.codigo);
            }     
            
                    catch(Exception e)
                        {
                            json = null;
                        }
                if (json != null)
                {
                    MvcApplication2.Models.Profesor.ESObject0 profesoresActivos = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.Profesor.ESObject0>(json);
                    foreach (var item2 in profesoresActivos.profesoresActivos)
                    {
                        string json2 ;
                        try
                        {
                         json2 = ser.getDatosProfesor(item2.CEDULA);
                         }     
            
                    catch(Exception e)
                        {
                            json2 = null;
                        }
                        if (json2 != null)
                        {
                            MvcApplication2.Models.DocenteWS.ESObject0 profesores = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.DocenteWS.ESObject0>(json2);
                            foreach (var item3 in profesores.datosProfesor)
                            {

                                 var hv = db.HojaVidas.Where(r => r.correo.Equals(item3.EMAIL));
                            List<HojaVida> hvs = hv.ToList();
                            if (hvs.Count > 0)
                            {
                                Docente docente = new Docente();
                                docente.tipo_documento = "Cedula de Ciudadania";
                                docente.num_documento = item3.CEDULA;
                                if (!item3.LIBREMIL.Equals(""))
                                {
                                    docente.num_libreta_militar = item3.LIBREMIL;
                                }

                                docente.clave = item3.CEDULA;
                                docente.titulo_pregrado = item3.CHIN_TITULO;
                                docente.maximo_nivel_formacion = item3.CNIA_DESCRIPCION;
                                docente.dedicacion = item3.CTUR_DESCRIPCION;
                                HojaVida hojavida = hvs.ElementAt(0);
                                hojavida.genero = item3.CHOV_SEXO;
                                hojavida.municipio_procedencia = item3.CHOV_LUGARNACE;
                                db.Entry(hojavida).State = EntityState.Modified;

                                docente.hojaVidaId = hojavida.hojaVidaId;
                                docente.rotacionId = 10;
                                Boolean estado = false;
                                int iddept = 3;
                               
                                foreach (var item4 in departamentos)
                                {

                                    if (item3.NOM_DEPTO.Equals(item4.nombre.ToUpper()))
                                    {
                                        estado = true;
                                        iddept = item4.DepartamentoSaludId;
                                    }
                                    
                                }
                              if(!estado)
                              {
                                  docente.DepartamentoSaludId = iddept;


                              }
                                    docente.DepartamentoSaludId = iddept;

                                

                                db.Docentes.Add(docente);
                                db.SaveChanges();

                            }
                            else
                            {


                                Familia familia = new Familia();
                                familia.primer_nombre_padre = "";
                                familia.segundo_nombre_padre = "";
                                familia.primer_apellido_padre = "";
                                familia.segundo_apellido_madre = "";
                                familia.telefono_padre = 0;
                                familia.primer_nombre_madre = "";
                                familia.segundo_nombre_madre = "";
                                familia.primer_apellido_madre = "";
                                familia.segundo_apellido_madre = "";
                                familia.telefono_madre = 0;
                                familia.primer_nombre_acudiente = "";
                                familia.segundo_nombre_acudiente = "";
                                familia.primer_apellido_acudiente = "";
                                familia.segundo_apellido_acudiente = "";
                                familia.telefono_acudiente = 0;
                                db.Familias.Add(familia);
                                db.SaveChanges();



                                var iffam = db.Familias.Max(p => p.familiaId);
                                HojaVida hojavida = new HojaVida();
                                hojavida.familiaId = iffam;


                                Docente docente = new Docente();
                                docente.tipo_documento = "CC";
                                docente.num_documento = item3.CEDULA;
                                if (!item3.LIBREMIL.Equals(""))
                                {
                                    docente.num_libreta_militar = item3.LIBREMIL;
                                }

                                docente.clave = item3.CEDULA;
                                docente.titulo_pregrado = item3.CHIN_TITULO;
                                docente.maximo_nivel_formacion = item3.CNIA_DESCRIPCION;
                                docente.dedicacion = item3.CTUR_DESCRIPCION;

                                hojavida.primer_nombre = item3.NOMBRE;
                                hojavida.primer_apellido = item3.P_APELLIDO;
                                hojavida.segundo_apellido = item3.S_APELLIDO;
                                hojavida.direccion_manizales = item3.DIRECCION;

                                hojavida.num_telefono = item3.TELEFONO;

                                if (!item3.FECHANAC.Equals(""))
                                {

                                    DateTime myDate = DateTime.ParseExact(item3.FECHANAC, "dd/MM/yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                    hojavida.fecha_nacimiento = myDate;
                                }
                                else
                                {
                                    hojavida.fecha_nacimiento = SqlDateTime.MinValue.Value;
                                }


                                hojavida.correo = item3.EMAIL;

                                db.HojaVidas.Add(hojavida);
                                db.SaveChanges();

                                iffam = db.HojaVidas.Max(p => p.hojaVidaId);

                                docente.hojaVidaId = iffam;
                                docente.rotacionId = 10;
                                Boolean estado = false;
                                int iddept = 3;
                                foreach (var item4 in departamentos)
                                {

                                    if (item3.NOM_DEPTO.Equals(item4.nombre.ToUpper()))
                                    {
                                        estado = true;
                                        iddept = item4.DepartamentoSaludId;
                                    }

                                }
                                
                                    docente.DepartamentoSaludId = iddept;

                                

                                db.Docentes.Add(docente);
                                db.SaveChanges();

                                if (hojavida.Docente != null)
                                    hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotosp/" + hojavida.Docente.ElementAt(0).num_documento + ".jpg";
                                // else
                                //    hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotos/" + estudiante.codigo + ".jpg";
                              //  hojavida = getSalud(hojavida);
                                Vacuna vacuna = new Vacuna();
                                vacuna.hojaVidaId = iffam;


                                vacuna.nombre_generico = ("Hepatitis B Dosis 1");
                                vacuna.fecha_vacunacion = SqlDateTime.MinValue.Value;
                                vacuna.fecha_prox_vacunacion = SqlDateTime.MinValue.Value;

                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Hepatitis B Dosis 2");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Hepatitis B Dosis 3");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Hepatitis A Dosis 1");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Hepatitis A Dosis 2");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Triple Viral Dosis 1");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Varicela Dosis 1");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 1");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 2");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 3");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 4");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 5");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Fiebre Amarilla Dosis 1");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Influenza Dosis 1");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("DTP Acelular Dosis 1");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Virus del papiloma humano Dosis 1");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Anticuerpos contra varicela");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();

                                vacuna.nombre_generico = ("Anticuerpos contra hepatitis B");
                                db.Vacunas.Add(vacuna);
                                db.SaveChanges();
                            }
                            }
                        }
                        }


                    }

            

                
            }

            var hojavidas = db.HojaVidas.Include(h => h.Familia);
                    
        }

        public void importaEstudiantes()
        {

            

            List<Programa> programas = db.Programas.ToList();
            foreach (var item in programas)
            {
                ServiceReference1.WSFacultadSaludSoapClient ser = new ServiceReference1.WSFacultadSaludSoapClient();

                string json = ser.getEstudiantesMatriculados(item.codigo);
                if(json!=null)
                {

                
                MvcApplication2.Models.Estudiante2.ESObject0 profesoresActivos = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.Estudiante2.ESObject0>(json);
                foreach (var item2 in profesoresActivos.estudiantesMatriculados)
                {
                    string json2;
                    try{
                         json2 = ser.getDatosEstudiante(item2.NUM_DOC);
                 
                    }
                    catch(Exception e)
                        {
                            json2 = null;
                        }
                   if (json2 != null)
                    {
                        MvcApplication2.Models.EstudianteWS.ESObject0 profesores = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.EstudianteWS.ESObject0>(json2);
                        foreach (var item3 in profesores.datosEstudiante)
                        {
                            var hv = db.HojaVidas.Where(r => r.correo.Equals(item3.EMAIL));
                            List<HojaVida> hvs = hv.ToList();
                            if(hvs.Count>0)
                            {
                                Estudiante estudiante = new Estudiante();
                               
                                estudiante.num_documento = item3.NUM_DOC;
                                estudiante.codigo = Int64.Parse(item3.CODIGO);

                                estudiante.clave = item3.CODIGO;
                                estudiante.tipo_documento = item3.NOM_DOC;

                                estudiante.modalidad = item3.MODALIDAD;
                                estudiante.estado_academico = item3.NOM_ESTADO;
                                estudiante.rotacionId = 10;
                                estudiante.direccion_procedencia = item3.DIR_CORREO;
                                HojaVida hvida = hvs.ElementAt(0);
                                hvida.genero = item3.SEXO;
                                hvida.municipio_procedencia = item3.MUN_PROC;
                                short s;
                                short.TryParse(item3.SEMESTRE, out s);
                                estudiante.semestre = s; estudiante.hojaVidaId = hvida.hojaVidaId;
                                
                               

                                estudiante.programaId = item.programaId;

                                db.Entry(hvida).State = EntityState.Modified;

                                db.Estudiantes.Add(estudiante);
                                db.SaveChanges();

                            }
                            else
                            {

                            
                            Familia familia = new Familia();
                            familia.primer_nombre_padre = "";
                            familia.segundo_nombre_padre = "";
                            familia.primer_apellido_padre = "";
                            familia.segundo_apellido_madre = "";
                            familia.telefono_padre = 0;
                            familia.primer_nombre_madre = "";
                            familia.segundo_nombre_madre = "";
                            familia.primer_apellido_madre = "";
                            familia.segundo_apellido_madre = "";
                            familia.telefono_madre = 0;
                            familia.primer_nombre_acudiente = "";
                            familia.segundo_nombre_acudiente = "";
                            familia.primer_apellido_acudiente = "";
                            familia.segundo_apellido_acudiente = "";
                            familia.telefono_acudiente = 0;
                            db.Familias.Add(familia);
                            db.SaveChanges();



                            var iffam = db.Familias.Max(p => p.familiaId);
                            HojaVida hojavida = new HojaVida();
                            hojavida.familiaId = iffam;
                                

                            Estudiante estudiante = new Estudiante();
                            estudiante.num_documento = item3.NUM_DOC;
                            estudiante.codigo = Int64.Parse(item3.CODIGO);
                            estudiante.tipo_documento = item3.NOM_DOC;

                            estudiante.modalidad = item3.MODALIDAD;
                            estudiante.clave = item3.NUM_DOC;
                            


                            estudiante.estado_academico = item3.NOM_ESTADO;
                            estudiante.rotacionId = 1;


                            hojavida.primer_nombre = item3.NOMBRE;
                            hojavida.primer_apellido = item3.P_APELLIDO;
                            hojavida.segundo_apellido = item3.S_APELLIDO;
                            estudiante.direccion_procedencia = item3.DIR_CORREO;
                            //hojavida.direccion_manizales = item3.DIR_CORREO;
                            
                            hojavida.genero = item3.SEXO;
                            hojavida.municipio_procedencia = item3.MUN_PROC;
                            short s;
                    short.TryParse(item3.SEMESTRE,out s);
                            estudiante.semestre = s;
                            hojavida.num_telefono = item3.TEL_CORREO;

                            if (!item3.FECHA_NACIMIENTO.Equals("") && !item3.FECHA_NACIMIENTO.Equals("//"))
                            {

                                DateTime myDate = DateTime.ParseExact(item3.FECHA_NACIMIENTO, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                hojavida.fecha_nacimiento = myDate;
                            }
                            else
                            {
                                hojavida.fecha_nacimiento = SqlDateTime.MinValue.Value;
                            }


                            hojavida.correo = item3.EMAIL;

                            db.HojaVidas.Add(hojavida);
                            db.SaveChanges();

                            iffam = db.HojaVidas.Max(p => p.hojaVidaId);

                            estudiante.hojaVidaId = iffam;
                            estudiante.rotacionId = 10;


                            estudiante.programaId = item.programaId;


                            db.Estudiantes.Add(estudiante);
                            db.SaveChanges();

                            hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotos/" + estudiante.codigo + ".jpg";
                           // hojavida = getSalud(hojavida);
                            Vacuna vacuna = new Vacuna();
                            vacuna.hojaVidaId = iffam;


                            vacuna.nombre_generico = ("Hepatitis B Dosis 1");
                            vacuna.fecha_vacunacion = SqlDateTime.MinValue.Value;
                            vacuna.fecha_prox_vacunacion = SqlDateTime.MinValue.Value;

                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Hepatitis B Dosis 2");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Hepatitis B Dosis 3");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Hepatitis A Dosis 1");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Hepatitis A Dosis 2");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Triple Viral Dosis 1");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Varicela Dosis 1");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Toxoide Tetánico Dosis 1");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Toxoide Tetánico Dosis 2");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Toxoide Tetánico Dosis 3");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Toxoide Tetánico Dosis 4");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Toxoide Tetánico Dosis 5");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Fiebre Amarilla Dosis 1");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Influenza Dosis 1");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("DTP Acelular Dosis 1");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Virus del papiloma humano Dosis 1");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Anticuerpos contra varicela");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();

                            vacuna.nombre_generico = ("Anticuerpos contra hepatitis B");
                            db.Vacunas.Add(vacuna);
                            db.SaveChanges();
                            }
                        }
                    }


                }

}

            }
        }

        public ActionResult Index(int id = 0)
        {



            var hojavidas = db.HojaVidas.Include(h => h.Familia);
          //  importaEstudiantes();
           // importaDocentes();
            return View(hojavidas.ToList());
        }

        //
        // GET: /HojaVida/Details/5

        public ActionResult Details(int id = 0)
        {
            HojaVida hojavida = db.HojaVidas.Find(id);
            if (hojavida == null)
            {
                return HttpNotFound();
            }
            return View(hojavida);
        }

        //
        // GET: /HojaVida/Create

        public ActionResult Create()
        {
            ViewBag.familiaId = new SelectList(db.Familias, "familiaId", "primer_nombre_padre");
            return View();
        }

        public ActionResult CreateDocente()
        {
            ViewBag.familiaId = new SelectList(db.Familias, "familiaId", "primer_nombre_padre");
            return View();
        }
        //
        // POST: /HojaVida/Create

        
        //public ActionResult Create(HojaVida hojavida)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.HojaVidas.Add(hojavida);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.familiaId = new SelectList(db.Familias, "familiaId", "primer_nombre_padre", hojavida.familiaId);
        //    return View(hojavida);
        //}
        public HojaVida getSalud(HojaVida hojavida)
        {
             string urlAddress ="";
            
                if (hojavida.Docente != null)
             urlAddress = "http://www.fosyga.gov.co/Aplicaciones/AfiliadoWebBDUA/Afiliado/Formulario/buda_consulta_afil_sin_dnn.aspx?id="+ hojavida.Docente.ElementAt(0).num_documento+"&tipodocumento=CC";
                else
             urlAddress = "http://www.fosyga.gov.co/Aplicaciones/AfiliadoWebBDUA/Afiliado/Formulario/buda_consulta_afil_sin_dnn.aspx?id="+ hojavida.Estudiante.ElementAt(0).num_documento+"&tipodocumento=CC";
           
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
              
                string data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                int pos = data.IndexOf("ldlEstadodata2");
                string estado = data.Substring( pos + 16);
                estado = estado.Substring(0, 1);
                hojavida.estado_afiliacion = estado;
               // if (estado.Equals("A"))
                //{
                     pos = data.IndexOf("lblEntidadData2");

                     string entidad = data.Substring(pos );
                     int pos2 = entidad.IndexOf("<");
                     string pos3 = pos2.ToString();
                     entidad = entidad.Substring( 17, pos2-17);
                     hojavida.entidad_salud = entidad;
                     
              //  }

         
            }
        
            return hojavida;

        }

        //
        // GET: HOJA DE VIDA EDITADA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HojaVida hojavida)
        {
            if (ModelState.IsValid)
            {

                Familia familia = new Familia();
                familia.primer_nombre_padre = "";
                familia.segundo_nombre_padre = "";
                familia.primer_apellido_padre = "";
                familia.segundo_apellido_madre = "";
                familia.telefono_padre = 0;
                familia.primer_nombre_madre = "";
                familia.segundo_nombre_madre = "";
                familia.primer_apellido_madre = "";
                familia.segundo_apellido_madre = "";
                familia.telefono_madre = 0;
                familia.primer_nombre_acudiente = "";
                familia.segundo_nombre_acudiente = "";
                familia.primer_apellido_acudiente = "";
                familia.segundo_apellido_acudiente = "";
                familia.telefono_acudiente = 0;
                db.Familias.Add(familia);
                db.SaveChanges();



                var iffam = db.Familias.Max(p => p.familiaId);
                hojavida.familiaId = iffam;
                db.HojaVidas.Add(hojavida);
                db.SaveChanges();

                iffam = db.HojaVidas.Max(p => p.hojaVidaId);


                //pte crear estudiante
                //Estudiante estudiante = new Estudiante();
                //estudiante.tipo_documento = "CC";
                //estudiante.num_documento = "10184756378";
                //estudiante.codigo = 1700921759;
                //estudiante.direccion_procedencia = "calle 56";
                //estudiante.barrio_procedencia = "linares";
                //estudiante.telefono_procedencia = "564565";
                //estudiante.clave = "12345";
                //estudiante.modalidad = "Universitario";
                //estudiante.programaId = 1;
                //estudiante.semestre = 8;
                //estudiante.estado_academico = "matriculado";
                //estudiante.hojaVidaId = iffam;
                //estudiante.rotacionId = 1;
                //db.Estudiantes.Add(estudiante);
                //db.SaveChanges();


                List<DepartamentoSalud> departamentos = db.DepartamentoSaluds.ToList();
                foreach (var item2 in departamentos)
                {
                    ServiceReference1.WSFacultadSaludSoapClient ser = new ServiceReference1.WSFacultadSaludSoapClient();

                    string json = ser.getProfesoresActivos(item2.codigo);
                }
                Docente docente = new Docente();
                docente.tipo_documento = "CC";
                docente.num_documento = "1053793956";


                docente.clave = "12345";

                docente.hojaVidaId = iffam;

                db.Docentes.Add(docente);
                db.SaveChanges();


                if (
                    hojavida.Docente != null)
                    hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotosp/" + hojavida.Docente.ElementAt(0).num_documento + ".jpg";
              // else
                //    hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotos/" + estudiante.codigo + ".jpg";

                Vacuna vacuna = new Vacuna();
                vacuna.hojaVidaId = iffam;


                vacuna.nombre_generico = ("Hepatitis B Dosis 1");
                vacuna.fecha_vacunacion = SqlDateTime.MinValue.Value;
                vacuna.fecha_prox_vacunacion = SqlDateTime.MinValue.Value;

                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis B Dosis 2");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis B Dosis 3");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis A Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis A Dosis 2");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Triple Viral Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Varicela Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 2");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 3");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 4");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 5");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Fiebre Amarilla Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Influenza Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("DTP Acelular Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Virus del papiloma humano Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Anticuerpos contra varicela");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Anticuerpos contra hepatitis B");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();






                db.SaveChanges();
                return View(hojavida);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDocente(HojaVida hojavida)
        {
            if (ModelState.IsValid)
            {

                Familia familia = new Familia();
                familia.primer_nombre_padre = "";
                familia.segundo_nombre_padre = "";
                familia.primer_apellido_padre = "";
                familia.segundo_apellido_madre = "";
                familia.telefono_padre = 0;
                familia.primer_nombre_madre = "";
                familia.segundo_nombre_madre = "";
                familia.primer_apellido_madre = "";
                familia.segundo_apellido_madre = "";
                familia.telefono_madre = 0;
                familia.primer_nombre_acudiente = "";
                familia.segundo_nombre_acudiente = "";
                familia.primer_apellido_acudiente = "";
                familia.segundo_apellido_acudiente = "";
                familia.telefono_acudiente = 0;
                db.Familias.Add(familia);
                db.SaveChanges();



                var iffam = db.Familias.Max(p => p.familiaId);
                hojavida.familiaId = iffam;
                db.HojaVidas.Add(hojavida);
                db.SaveChanges();

                iffam = db.HojaVidas.Max(p => p.hojaVidaId);


                //pte crear estudiante
                Docente docente = new Docente();
                docente.tipo_documento = "CC";
                docente.num_documento = "10184756378";
                docente.clave = "12345";
                docente.DepartamentoSaludId = 1;
                docente.num_libreta_militar = "";
                docente.hojaVidaId = iffam;
                docente.rotacionId = 1;
                db.Docentes.Add(docente);
                db.SaveChanges();


                    hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotos/" + hojavida.Docente.ElementAt(0).num_documento + ".jpg";
               
                Vacuna vacuna = new Vacuna();
                vacuna.hojaVidaId = iffam;


                vacuna.nombre_generico = ("Hepatitis B Dosis 1");
                vacuna.fecha_vacunacion = SqlDateTime.MinValue.Value;
                vacuna.fecha_prox_vacunacion = SqlDateTime.MinValue.Value;

                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis B Dosis 2");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis B Dosis 3");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis A Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis A Dosis 2");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Triple Viral Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Varicela Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 2");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 3");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 4");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 5");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Fiebre Amarilla Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Influenza Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("DTP Acelular Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Virus del papiloma humano Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Anticuerpos contra varicela");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Anticuerpos contra hepatitis B");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();






                db.SaveChanges();
                return View(hojavida);
            }
            return RedirectToAction("Index");
        }


        //
        // Adicionar anticuerpos
        public ActionResult adicionarAnticuerpo()
        {
            List<HojaVida> listaHV = db.HojaVidas.ToList();
            foreach (var item in listaHV)
            {
                Vacuna vacuna = new Vacuna();

                //Vacuna.hojaVidaId = listaHV.ElementAt(0).hojaVidaId;

                vacuna.nombre_generico = "Anticuerpos contra varicela"; 
                vacuna.fecha_vacunacion = SqlDateTime.MinValue.Value;
                vacuna.fecha_prox_vacunacion = SqlDateTime.MinValue.Value;

                db.Vacunas.Add(vacuna);
                db.SaveChanges();


                vacuna.nombre_generico = "Anticuerpos contra hepatitis B "; 
                vacuna.fecha_vacunacion = SqlDateTime.MinValue.Value;
                vacuna.fecha_prox_vacunacion = SqlDateTime.MinValue.Value;

                db.Vacunas.Add(vacuna);
                db.SaveChanges();
                
            }
                 
           return RedirectToAction("Index");
        }

        //
        // GET: /HojaVida/Edit/5

        public ActionResult Edit(int id = 0)
        {
            HojaVida hojavida = db.HojaVidas.Find(id);
            if (hojavida == null)
            {
                return HttpNotFound();
            }
            ViewBag.familiaId = new SelectList(db.Familias, "familiaId", "primer_nombre_padre", hojavida.familiaId);
            return View(hojavida);
        }

        //
        // POST: /HojaVida/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HojaVida hojavida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hojavida).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.familiaId = new SelectList(db.Familias, "familiaId", "primer_nombre_padre", hojavida.familiaId);
            return View(hojavida);
        }

        //
        // GET: /HojaVida/Delete/5

        public ActionResult Delete(int id = 0)
        {
            HojaVida hojavida = db.HojaVidas.Find(id);
            if (hojavida == null)
            {
                return HttpNotFound();
            }
            return View(hojavida);
        }

        //
        // POST: /HojaVida/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HojaVida hojavida = db.HojaVidas.Find(id);
            db.HojaVidas.Remove(hojavida);
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