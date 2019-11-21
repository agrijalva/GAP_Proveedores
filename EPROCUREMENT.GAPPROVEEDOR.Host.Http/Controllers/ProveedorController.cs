using EPROCUREMENT.GAPPROVEEDOR.Business.Proveedor;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using EPROCUREMENT.GAPPROVEEDOR.Entities.Proveedor;
using System.Web.Http;

namespace EPROCUREMENT.GAPPROVEEDOR.Host.Http.Controllers
{
    [RoutePrefix("api/Proveedor")]
    public class ProveedorController : ApiController
    {
        /// <summary>
        /// Operacion para insertar un proveedor
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Insertar")]
        public ProveedorResponseDTO Process([FromBody]ProveedorRequesteDTO request)
        {
            var response = new HandlerProveedor().GuardarProveedor(request);
            return response;
        }

        [HttpPost]
        [Route("GuardarProveedorCuenta")]
        public ProveedorCuentaResponseDTO GuardarProveedorCuenta([FromBody]ProveedorCuentaRequestDTO request)
        {
            var response = new HandlerProveedor().GuardarProveedorCuenta(request);
            return response;
        }

        [HttpPost]
        [Route("GuardarProveedorDocumento")]
        public ProveedorDocumentoResponseDTO GuardarProveedorDocumento([FromBody]ProveedorDocumentoRequestDTO request)
        {
            var response = new HandlerProveedor().GuardarProveedorDocumento(request);
            return response;
        }

        // GET: api/ProveedorEstatusList
        [HttpPost]
        [Route("ProveedorEstatusList")]
        public ProveedorEstatusResponseDTO GetProveedorEstatusList([FromBody]ProveedorEstatusRequestDTO request)
        {
            var proveedorEstatus = new HandlerProveedor().GetProveedorEstatusList(request);

            return proveedorEstatus;
        }

        // GET: api/ProveedorElemento
        [HttpPost]
        [Route("ProveedorElemento")]
        public ProveedorDetalleResponseDTO GetProveedorElemento([FromBody]ProveedorDetalleRequestDTO request)
        {
            var proveedorElemento = new HandlerProveedor().GetProveedorElemento(request);

            return proveedorElemento;
        }

        [HttpPost]
        [Route("AprobarProveedor")]
        public ProveedorEstatusResponseDTO EstatusProveedorInsertar([FromBody]ProveedorAprobarRequestDTO request)
        {
            var response = new HandlerProveedor().EstatusProveedorInsertar(request);
            return response;
        }

        [HttpPost]
        [Route("InsertarTempProveedor")]
        public ProveedorResponseDTO GuardarTempProveedor([FromBody]ProveedorRequesteDTO request)
        {
            var response = new HandlerProveedor().TempProveedorInsertar(request);
            return response;
        }

        [HttpPost]
        [Route("ProveedorInfoFinanciera")]
        public ProveedorInformacionFinanciera GetProveedorInfoFinanciera([FromBody]ProveedorInformacionFinancieraRequestDTO request)
        {
            var response = new HandlerProveedor().GetProveedorInfoFinanciera(request);
            return response;
        }

    }
}