using EPROCUREMENT.GAPPROVEEDOR.Data;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System.Collections.Generic;

namespace EPROCUREMENT.GAPPROVEEDOR.Business.Catalogo
{
    public class HandlerCatalogo
    {
        private readonly CatalogoData catalogoData;

        /// <summary>
        /// Constructor para la inicializacion de los accesos a datos
        /// </summary>
        public HandlerCatalogo()
        {
            catalogoData = new CatalogoData();
        }

        /// <summary>
        /// Metodo para obtener una lista de paises
        /// </summary>
        /// <returns>Un objeto de tipo PaisResponseDTO</returns>
        public PaisResponseDTO GetPaisList()
        {
            PaisResponseDTO response = catalogoData.GetPaisList();
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public EstatusProveedorResponseDTO GetEstatusProveedorList()
        {
            EstatusProveedorResponseDTO response = catalogoData.GetEstatusProveedorList();
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }
        
        /// <summary>
        /// Metodo para obtener una lista de Aeropuertos
        /// </summary>
        /// <returns>Un objeto de tipo AeropuertoResponseDTO</returns>
        public AeropuertoResponseDTO GetAeropuertoList()
        {
            AeropuertoResponseDTO response = catalogoData.GetAeropuertoList();
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        /// <summary>
        /// Metodo para obtener una lista de Aeropuertos
        /// </summary>
        /// <returns>Un objeto de tipo GiroResponseDTO</returns>
        public GiroResponseDTO GetGiroList()
        {
            GiroResponseDTO response = catalogoData.GetGiroList();
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        /// <summary>
        /// Metodo para obtener una lista de Nacionalidades
        /// </summary>
        /// <returns>Un objeto de tipo NacionalidadResponseDTO</returns>
        public NacionalidadResponseDTO GetNacionalidadList()
        {
            NacionalidadResponseDTO response = catalogoData.GetNacionalidadList();
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        /// <summary>
        /// Metodo para obtener una lista de ZonaHoraria
        /// </summary>
        /// <returns>Un objeto de tipo ZonaHorariaResponseDTO</returns>
        public ZonaHorariaResponseDTO GetZonaHorariaList()
        {
            ZonaHorariaResponseDTO response = catalogoData.GetZonaHorariaList();
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        /// <summary>
        /// Metodo para obtener una lista de Estados
        /// </summary>
        /// <param name="request">Un ibjeto de tipo EstadoRequesteDTO</param>
        /// <returns>Un objeto de tipo EstadoResponseDTO</returns>
        public EstadoResponseDTO GetEstadoList(EstadoRequesteDTO request)
        {
            EstadoResponseDTO response = catalogoData.GetEstadoList(request);
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        /// <summary>
        /// Metodo para obtener una lista de Municipios
        /// </summary>
        /// <param name="request">Un ibjeto de tipo MunicipioRequesteDTO</param>
        /// <returns>Un objeto de tipo MunicipioResponseDTO</returns>
        public MunicipioResponseDTO GetMunicipioList(MunicipioRequesteDTO request)
        {
            var response = catalogoData.GetMunicipioList(request);
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        /// <summary>
        /// Metodo para obtener una lista de Idiomas
        /// </summary>
        /// <returns>Un objeto de tipo IdiomaResponseDTO</returns>
        public IdiomaResponseDTO GetIdiomaList()
        {
            var  response = catalogoData.GetIdiomaList();
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        /// <summary>
        /// Metodo para obtener una lista de TipoProveedor
        /// </summary>
        /// <returns>Un objeto de tipo TipoProveedorResponseDTO</returns>
        public TipoProveedorResponseDTO GetTipoProveedorList()
        {
            var response = catalogoData.GetTipoProveedorList();
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public TipoCuentaResponseDTO GetTipoCuentaList()
        {
            TipoCuentaResponseDTO response = catalogoData.GetTipoCuentaList();
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public BancoResponseDTO GetBancoList()
        {
            BancoResponseDTO response = catalogoData.GetBancoList();
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public CatalogoDocumentoResponseDTO GetCatalogoDocumentoList()
        {
            CatalogoDocumentoResponseDTO response = catalogoData.GetCatalogoDocumentoList();
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public FormatoArchivoResponseDTO GetFormatoArchivoList()
        {
            FormatoArchivoResponseDTO response = catalogoData.GetFormatoArchivoList();
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }
    }
}
