using EprocurementWeb.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EprocurementWeb.Models;
using EprocurementWeb.Filters;

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

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetSolicitudFacturaList(int? idSolicitudFactura, int? idEstatus, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            SolicitudFacturaBusiness businessLogic = new SolicitudFacturaBusiness();
            var request = new SolicitudFacturaRequestModel
            {
                SolicitudFacturaFiltro = new SolicitudFacturaFiltroModel
                {
                    IdProveedor = usuarioInfo.IdProveedor,
                    IdSolicitudFactura = idSolicitudFactura,
                    IdEstatus = idEstatus,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin
                }
            };

            var solicitudFacturaResponse = businessLogic.GetSolicitudFacturaList(request);
            
            return Json(solicitudFacturaResponse.SolicitudFacturaList, JsonRequestBehavior.AllowGet);           

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