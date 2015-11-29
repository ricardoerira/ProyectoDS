using MvcApplication2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication2.Controllers
{
    public class ActualizacionController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /Actualizacion/

        public ActionResult Index()
        {
           // importaMaterias();
          //  importaGruposMateria();
          //  importaDocentes();
            //importaEstudiantes();
            importaEstudiantesRotacion();
            return View();
        }



        public void importaMaterias()
        {
            ServiceReference1.WSFacultadSaludSoapClient ser = new ServiceReference1.WSFacultadSaludSoapClient();

            string json = ser.getMaterias();

            json = json.Replace("\"materias\"", "6@");

            json = json.Replace("\":\"", "1@");
            json = json.Replace("\",\"", "2@");
            json = json.Replace("{\"", "3@");
            json = json.Replace("\"}", "4@");
            json = json.Replace("\"\"", "5@");

            json = json.Replace("\"", "");


            json = json.Replace("1@", "\":\"");
            json = json.Replace("2@", "\",\"");
            json = json.Replace("3@", "{\"");
            json = json.Replace("4@", "\"}");
            json = json.Replace("5@", "\"\"");
            json = json.Replace("6@", "\"materias\"");



            MvcApplication2.Models.Materia.ESObject0 listmaterias = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.Materia.ESObject0>(json);
            List<DepartamentoSalud> departamentos = db.DepartamentoSaluds.ToList();
            int cont = 0;
            foreach (var item in listmaterias.materias)
            {
                ActividadAcademica academica = new ActividadAcademica();
                Boolean estado = false;
                int iddept = 0;
                foreach (var item2 in departamentos)
                {

                    if (item2.nombre.ToUpper().Equals(item.NOM_DEPTO))
                    {
                        estado = true;

                        iddept = item2.DepartamentoSaludId;
                    }



                }
                if (estado)
                {
                    academica.DepartamentoSaludId = iddept;

                }

                cont++;
                if (academica.DepartamentoSaludId != 0)
                {
                    academica.asignatura = item.NOM_MATERIA;
                    academica.nombre = item.NOM_MATERIA;
                    academica.codigo_AA = item.COD_MATERIA;

                    academica.modalidad_practica = item.PMO_NOMBRE;
                    if (item.GRUPOS_MAXIMO != null && !item.GRUPOS_MAXIMO.Equals(String.Empty))
                    {
                        academica.grupo_maximo = Int32.Parse(item.GRUPOS_MAXIMO);

                    }
                    db.ActividadAcademicas.Add(academica);
                    db.SaveChanges();

                }

            }
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
                                if (docente.ToList().Count() > 0)
                                {
                                    docente2 = (Docente)docente.ToList().ElementAt(0);
                                    if (docente2 == null)
                                    {
                                        docente2.rotacionId = item.rotacionId;

                                    }
                                }




                            }
                            var estudiante = db.Estudiantes.Where(r => r.codigo == codigo);


                            estudiante2 = (Estudiante)estudiante.ToList().ElementAt(0);
                            estudiante2.rotacionId = item.rotacionId;


                            RotacionEstudiante re = new RotacionEstudiante();
                            if (docente2 != null)
                            {
                                re.docenteId = docente2.docenteId;

                            }
                            else
                            {
                                re.docenteId = 1385;

                            }
                            re.estudianteId = estudiante2.estudianteId;
                            re.rotacionId = item.rotacionId;
                            re.IPS_ESEId = 11;
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





        public void importaDocentes()
        {

            List<DepartamentoSalud> departamentos = db.DepartamentoSaluds.ToList();
            foreach (var item in departamentos)
            {
                ServiceReference1.WSFacultadSaludSoapClient ser = new ServiceReference1.WSFacultadSaludSoapClient();
                string json;

                try
                {
                    json = ser.getProfesoresActivos(item.codigo.Trim());
                }

                catch (Exception e)
                {
                    json = null;
                }
                if (json != null)
                {
                    MvcApplication2.Models.Profesor.ESObject0 profesoresActivos = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.Profesor.ESObject0>(json);
                    foreach (var item2 in profesoresActivos.profesoresActivos)
                    {
                        string json2;
                        try
                        {
                            json2 = ser.getDatosProfesor(item2.CEDULA);
                        }

                        catch (Exception e)
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
                                    HojaVida hojavida = hvs.ElementAt(0);
                                    hojavida.genero = item3.CHOV_SEXO;
                             
                                    hojavida.municipio_procedencia = item3.CHOV_LUGARNACE;
                                    db.Entry(hojavida).State = EntityState.Modified;
                                    if(hojavida.municipio_procedencia.Equals(String.Empty))
                                    {
                                        hojavida.municipio_procedencia = ".";

                                    }
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
                                    if (!estado)
                                    {
                                        docente.DepartamentoSaludId = iddept;


                                    }
                                    docente.DepartamentoSaludId = iddept;




                                    try
                                    {

                                        db.Docentes.Add(docente);
                                        db.SaveChanges();
                                    }
                                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                    {
                                        Console.WriteLine(e.Data);
                                    }

                                }
                                else
                                {


                                    InsertaFamilia();
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
                                    hojavida.num_celular =3000000000;
                                    hojavida.municipio_procedencia = ".";

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

                                   
                                    try
                                    {
                                        db.HojaVidas.Add(hojavida);
                                        db.SaveChanges();
                                    }
                                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                    {
                                        Console.WriteLine(e.Data);
                                    }
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

                                    try
                                    {

                                        db.Docentes.Add(docente);
                                        db.SaveChanges();
                                    }
                                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                    {
                                        Console.WriteLine(e.Data);
                                    }


                                    if (hojavida.Docente != null)
                                        hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotosp/" + hojavida.Docente.ElementAt(0).num_documento + ".jpg";
                                    // else
                                    //    hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotos/" + estudiante.codigo + ".jpg";
                                    //  hojavida = getSalud(hojavida);
                                    InsertaVacunas(iffam);
                                }
                            }
                        }
                    }


                }




            }

            

        }

        private void InsertaVacunas(int iffam)
        {
            Vacuna vacuna = new Vacuna();
            vacuna.hojaVidaId = iffam;
            vacuna.lote = ".";


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

        public void importaEstudiantes()
        {



            List<Programa> programas = db.Programas.ToList();
            foreach (var item in programas)
            {
                ServiceReference1.WSFacultadSaludSoapClient ser = new ServiceReference1.WSFacultadSaludSoapClient();

                string json = ser.getEstudiantesMatriculados(item.codigo);
                if (json != null)
                {


                    MvcApplication2.Models.Estudiante2.ESObject0 profesoresActivos = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.Estudiante2.ESObject0>(json);
                    foreach (var item2 in profesoresActivos.estudiantesMatriculados)
                    {
                        string json2;
                        try
                        {
                            json2 = ser.getDatosEstudiante(item2.NUM_DOC);

                        }
                        catch (Exception e)
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
                                if (hvs.Count > 0)
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
                                    if (hvida.municipio_procedencia.Equals(String.Empty))
                                    {
                                        hvida.municipio_procedencia = ".";

                                    }
                                    short s;
                                    short.TryParse(item3.SEMESTRE, out s);
                                    estudiante.semestre = s; estudiante.hojaVidaId = hvida.hojaVidaId;



                                    estudiante.programaId = item.programaId;

                                    db.Entry(hvida).State = EntityState.Modified;
                                    try
                                    {

                                        db.Estudiantes.Add(estudiante);
                                        db.SaveChanges();
                                    }
                                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                    {
                                        Console.WriteLine(e.Data);
                                    }

                                    

                                }
                                else
                                {
                                    try
                                    {

                                        InsertaFamilia();

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
                                    hojavida.direccion_manizales = item3.DIR_CORREO;
                                    hojavida.num_celular = 3000000000;
                                    if (hojavida.direccion_manizales.Equals(String.Empty))
                                    {
                                        hojavida.direccion_manizales = ".";

                                    }
                                    hojavida.genero = item3.SEXO;
                                    hojavida.municipio_procedencia = item3.MUN_PROC;
                                    if (hojavida.municipio_procedencia.Equals(String.Empty))
                                    {
                                        hojavida.municipio_procedencia = ".";

                                    }
                                    short s;
                                    short.TryParse(item3.SEMESTRE, out s);
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
                                    try
                                    {

                                        db.HojaVidas.Add(hojavida);
                                        db.SaveChanges();
                                    }
                                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                    {
                                        Console.WriteLine(e.Data);
                                    }

                                   

                                    iffam = db.HojaVidas.Max(p => p.hojaVidaId);

                                    estudiante.hojaVidaId = iffam;
                                    estudiante.rotacionId = 10;


                                    estudiante.programaId = item.programaId;

                                    try
                                    {

                                        db.Estudiantes.Add(estudiante);
                                        db.SaveChanges();
                                    }
                                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                    {
                                        Console.WriteLine(e.Data);
                                    };

                                    hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotos/" + estudiante.codigo + ".jpg";
                                    // hojavida = getSalud(hojavida);
                                    InsertaVacunas(iffam);
                                }
                            }
                        }


                    }

                }

            }
        }

        private void InsertaFamilia()
        {
            Familia familia = new Familia();
            familia.primer_nombre_padre = "";
            familia.segundo_nombre_padre = "";
            familia.primer_apellido_padre = "";
            familia.segundo_apellido_madre = "";
            familia.telefono_padre = 3000000000;
            familia.primer_nombre_madre = "";
            familia.segundo_nombre_madre = "";
            familia.primer_apellido_madre = "";
            familia.segundo_apellido_madre = "";
            familia.telefono_madre = 3000000000;
            familia.primer_nombre_acudiente = ".";
            familia.segundo_nombre_acudiente = ".";
            familia.primer_apellido_acudiente = ".";
            familia.segundo_apellido_acudiente = ".";
            familia.telefono_acudiente = 3000000000;
            familia.celular_acudiente = 3000000000;
            familia.direccion_acudiente = ".";
            familia.direccion_madre = "";
            familia.direccion_padre = "";
            try
            {
                db.Familias.Add(familia);
                db.SaveChanges();
            }
            catch(System.Data.Entity.Validation.DbEntityValidationException  e)
            {
                Console.WriteLine(e.Data);
            }

          
        }





















    }

}
