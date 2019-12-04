using EprocurementWeb.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EprocurementWeb.Models;
using EprocurementWeb.Filters;
using EPROCUREMENT.GAPPROVEEDOR.Entities;

namespace EprocurementWeb.Controllers
{
    public class GestionController : Controller
    {
        public string nav_factura;
        public string nav_sFactura;
        public string nav_sCotizacion;
        public string nav_sOC;
        // GET: Gestion
        public ActionResult Index()
        {
            return View();
        }


        // GET: SolicitudFacturacion
        public ActionResult SolicitudFacturacion()
        {
            List<EstatusSolicitudModel> estatusList = new List<EstatusSolicitudModel>();
            estatusList.Add(new EstatusSolicitudModel { IdEstatus = 1, Descripcion = "Pendiente" });
            estatusList.Add(new EstatusSolicitudModel { IdEstatus = 2, Descripcion = "En Proceso" });
            estatusList.Add(new EstatusSolicitudModel { IdEstatus = 3, Descripcion = "Aceptada" });
            estatusList.Add(new EstatusSolicitudModel { IdEstatus = 4, Descripcion = "Cancelada por AX" });
            estatusList.Add(new EstatusSolicitudModel { IdEstatus = 5, Descripcion = "Pagada" });
            ViewBag.EstatusSolicitudList = estatusList;

            List<SolicitudFactura> numeroSolicitudList = new List<SolicitudFactura>();
            numeroSolicitudList.Add(new SolicitudFactura { IdSolicitudFactura = 4 });
            numeroSolicitudList.Add(new SolicitudFactura { IdSolicitudFactura = 5 });
            numeroSolicitudList.Add(new SolicitudFactura { IdSolicitudFactura = 6 });
            numeroSolicitudList.Add(new SolicitudFactura { IdSolicitudFactura = 7 });
            ViewBag.NumeroSolicitudList = numeroSolicitudList;


            return View();
        }

        // GET: SolicitudFacturacion
        public ActionResult SolicitudDetalle(int idSolicitudFactura)
        {
            ViewBag.IdSolicitudFactura = idSolicitudFactura;
            return View();
        }

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

        public JsonResult GetSolicitudDetalleList(int idSolicitudFactura)
        {
            SolicitudFacturaBusiness businessLogic = new SolicitudFacturaBusiness();
            var request = new SolicitudFacturaDetalleRequestDTO
            {
                IdSolicitudFactura = idSolicitudFactura
            };

            var solicitudDetalleResponse = businessLogic.GetSolicitudFacturaDetalle(request);

            return Json(solicitudDetalleResponse, JsonRequestBehavior.AllowGet);

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