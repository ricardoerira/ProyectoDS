using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if(User.IsInRole("IPS"))
                {
                    return RedirectToAction("../IPS_ESE/VistaIPS_ESE");
                }
                if (User.IsInRole("DocenciaServicio"))
                {
                    return RedirectToAction("../Rotacion/VistaODS");
                    
                }
                if (User.IsInRole("Departamento"))
                {
                    return RedirectToAction("../DepartamentoSalud/VistaDepartamentoSalud");
                }
                if (User.IsInRole("IPSVacunadora"))
                {
                    return RedirectToAction("../Vacuna/VistaIPS_Universitaria");
                }
                if (User.IsInRole("ContraPrestacion"))
                {
                    return RedirectToAction("../Curso/VistaContraprestacion");
                }
                
            }
            else
            {
               // return RedirectToAction("../Account/Login");
                
            }
           
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
