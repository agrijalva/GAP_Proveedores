using EPROCUREMENT.GAPPROVEEDOR.Business.Catalogo;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System.Web.Http;

namespace EPROCUREMENT.GAPPROVEEDOR.Host.Http.Controllers
{
    [RoutePrefix("api/Catalogo")]
    public class CatalogoController : ApiController
    {
        // GET: api/Pais
        [HttpGet]
        [Route("PaisGetList")]
        public PaisResponseDTO GetPaisList()
        {
            var paisResponseDTO = new HandlerCatalogo().GetPaisList();

            return paisResponseDTO;
        }

        [HttpGet]
        [Route("EstatusProveedorGetList")]
        public EstatusProveedorResponseDTO GetEstatusProveedorList()
        {
            var paisResponseDTO = new HandlerCatalogo().GetEstatusProveedorList();

            return paisResponseDTO;
        }
        // GET: api/Aeropuerto
        [HttpGet]
        [Route("AeropuertoGetList")]
        public AeropuertoResponseDTO GetAeropuertoList()
        {
            var aeropuertoResponseDTO = new HandlerCatalogo().GetAeropuertoList();

            return aeropuertoResponseDTO;
        }

        // GET: api/Giro
        [HttpGet]
        [Route("GiroGetList")]
        public GiroResponseDTO GetGiroList()
        {
            var giroResponseDTO = new HandlerCatalogo().GetGiroList();

            return giroResponseDTO;
        }

        // GET: api/Nacionalidad
        [HttpGet]
        [Route("NacionalidadGetList")]
        public NacionalidadResponseDTO GetNacionalidadList()
        {
            var nacionalidadResponseDTO = new HandlerCatalogo().GetNacionalidadList();

            return nacionalidadResponseDTO;
        }

        // GET: api/ZonaHoraria
        [HttpGet]
        [Route("ZonaHorariaGetList")]
        public ZonaHorariaResponseDTO GetZonaHorariaList()
        {
            var zonaHorariaResponseDTO = new HandlerCatalogo().GetZonaHorariaList();

            return zonaHorariaResponseDTO;
        }

        // GET: api/Estado
        [HttpPost]
        [Route("EstadoGetList")]
        public EstadoResponseDTO GetEstadoList([FromBody]EstadoRequesteDTO request)
        {
            var estadoResponse = new HandlerCatalogo().GetEstadoList(request);

            return estadoResponse;
        }

        // GET: api/Municipio
        [HttpPost]
        [Route("MunicipioGetList")]
        public MunicipioResponseDTO GetMunicipioList([FromBody]MunicipioRequesteDTO request)
        {
            var municipioResponse = new HandlerCatalogo().GetMunicipioList(request);

            return municipioResponse;
        }

        // GET: api/Idioma
        [HttpGet]
        [Route("IdiomaGetList")]
        public IdiomaResponseDTO GetIdiomaList()
        {
            var idiomaResponse = new HandlerCatalogo().GetIdiomaList();

            return idiomaResponse;
        }

        // GET: api/TipoProveedor
        [HttpGet]
        [Route("TipoProveedorGetList")]
        public TipoProveedorResponseDTO GetTipoProveedorList()
        {
            var tipoProveedorResponse = new HandlerCatalogo().GetTipoProveedorList();

            return tipoProveedorResponse;
        }

        // GET: api/TipoCuenta
        [HttpGet]
        [Route("TipoCuentaGetList")]
        public TipoCuentaResponseDTO GetTipoCuentaList()
        {
            var tipoCuentaResponse = new HandlerCatalogo().GetTipoCuentaList();

            return tipoCuentaResponse;
        }

        // GET: api/Banco
        [HttpGet]
        [Route("BancoGetList")]
        public BancoResponseDTO GetBancoList()
        {
            var bancoResponse = new HandlerCatalogo().GetBancoList();

            return bancoResponse;
        }

        // GET: api/CatalogoDocumento
        [HttpGet]
        [Route("CatalogoDocumentoList")]
        public CatalogoDocumentoResponseDTO GetCatalogoDocumentoList()
        {
            var catalogoDocumento = new HandlerCatalogo().GetCatalogoDocumentoList();

            return catalogoDocumento;
        }

        // GET: api/FormatoArchivo
        [HttpGet]
        [Route("FormatoArchivoList")]
        public FormatoArchivoResponseDTO GetFormatoArchivoList()
        {
            var formatoArchivoResponse = new HandlerCatalogo().GetFormatoArchivoList();

            return formatoArchivoResponse;
        }
    }
}
