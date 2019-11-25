using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EprocurementWeb.Controllers
{
    public class GestionController : Controller
    {
        // GET: Gestion
        public ActionResult Index()
        {
            return View();
        }

        // GET: SolicitudFacturacion
        public ActionResult SolicitudFacturacion()
        {
            return View();
        }

        // GET: SolicitudCotizacion
        public ActionResult SolicitudCotizacion()
        {
            return View();
        }

        // GET: OrdenCompra
        public ActionResult OrdenCompra()
        {
            return View();
        }
    }
}