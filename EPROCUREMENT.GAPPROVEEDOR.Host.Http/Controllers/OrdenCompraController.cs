using EPROCUREMENT.GAPPROVEEDOR.Business.OrdenCompra;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace EPROCUREMENT.GAPPROVEEDOR.Host.Http.Controllers
{
    [RoutePrefix("api/OrdenCompra")]
    public class OrdenCompraController : ApiController
    {
        [HttpPost]
        [Route("OrdenCompraGetList")]
        public OrdenCompraResponseDTO GetOrdenCompraList([FromBody]OrdenCompraRequestDTO request)
        {
            var response = new HandlerOrdenCompra().GetOrdenCompraList(request);

            return response;
        }

        [HttpPost]
        [Route("OrdenCompraDetalleGetList")]
        public OrdenCompraDetalleResponseDTO GetOrdenCompraDetalleList([FromBody]OrdenCompraDetalleRequestDTO request)
        {
            var response = new HandlerOrdenCompra().GetOrdenCompraDetalleList(request);

            return response;
        }
    }
}
