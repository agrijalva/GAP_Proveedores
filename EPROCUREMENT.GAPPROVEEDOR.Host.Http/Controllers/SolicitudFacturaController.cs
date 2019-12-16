using EPROCUREMENT.GAPPROVEEDOR.Business.SolicitudFactura;
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

        [HttpPost]
        [Route("FacturaGetList")]
        public FacturaResponseDTO GetFacturaList([FromBody]FacturaRequestDTO request)
        {
            var ListResponse = new HandlerSolicitudFactura().GetFacturaList(request);

            return ListResponse;
        }

        [HttpPost]
        [Route("GuardarHistoricoEstatusSolicitud")]
        public EstatusSolicitudResponseDTO GuardarHistoricoEstatusSolicitud([FromBody]EstatusSolicitudRequestDTO request)
        {
            var ListResponse = new HandlerSolicitudFactura().GuardarHistoricoEstatusSolicitud(request);

            return ListResponse;
        }

        [HttpPost]
        [Route("Upload")]
        public HttpResponseMessage Upload()
        {
            HttpResponseMessage result = null;

            if (Request.Content.IsMimeMultipartContent("form-data"))
            {
                var request = HttpContext.Current.Request;

                if (request.Files.Count > 0)
                {
                    var nombreArchivo = request.Files[0].FileName;
                    string[] authorsList = nombreArchivo.Split('_');
                    string ruta = ConfigurationManager.AppSettings["rutaDocuments"] + "/Facturas/" + authorsList[0] + "/" + authorsList[1] + "/";
                    string rutaF = HttpContext.Current.Server.MapPath(ruta);
                    if (!Directory.Exists(rutaF))
                    {
                        Directory.CreateDirectory(rutaF);
                    }

                    var docfiles = new List<string>();
                    foreach (string file in request.Files)
                    {
                        var postedFile = request.Files[file];
                        var filePath = Path.Combine(rutaF + "\\" + postedFile.FileName);
                        postedFile.SaveAs(filePath);
                        docfiles.Add(filePath);
                    }
                    result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return result;
        }

        [HttpGet]
        [Route("AeropuertoGetList")]
        public AeropuertoListaResponseDTO GetAeropuertoLista()
        {
            var ListResponse = new HandlerSolicitudFactura().GetAeropuertoLista();

            return ListResponse;
        }

        [HttpPost]
        [Route("GetFacturaDetalle")]
        public FacturaDetalleResponseDTO GetFacturaDetalle([FromBody]FacturaDetalleRequestDTO request)
        {
            var estadoResponse = new HandlerSolicitudFactura().GetFacturaDetalle(request);

            return estadoResponse;
        }
    }
}
