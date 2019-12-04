using EPROCUREMENT.GAPPROVEEDOR.Business.SolicitudFactura;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EPROCUREMENT.GAPPROVEEDOR.Host.Http.Controllers
{
    [RoutePrefix("api/SolicitudFactura")]
    public class SolicitudFacturaController : ApiController
    {
        [HttpPost]
        [Route("SolicitudFacturaGetList")]
        public SolicitudFacturaResponseDTO GetSolicitudFacturaList([FromBody]SolicitudFacturaRequestDTO request)
        {
            var estadoResponse = new HandlerSolicitudFactura().GetSolicitudFacturaList(request);

            return estadoResponse;
        }

        [HttpPost]
        [Route("SolicitudFacturaDetalleGetList")]
        public SolicitudFacturaDetalleResponseDTO GetSolicitudFacturaCabecero([FromBody]SolicitudFacturaDetalleRequestDTO request)
        {
            var estadoResponse = new HandlerSolicitudFactura().GetSolicitudFacturaCabecero(request);

            return estadoResponse;
        }
    }
}
