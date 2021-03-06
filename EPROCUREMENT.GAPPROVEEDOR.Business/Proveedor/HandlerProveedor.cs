﻿using EPROCUREMENT.GAPPROVEEDOR.Data;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using EPROCUREMENT.GAPPROVEEDOR.Entities.Proveedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Business.Proveedor
{
    public class HandlerProveedor
    {
        private readonly ProveedorData proveedorData;

        /// <summary>
        /// Constructor para la inicializacion de los accesos a datos
        /// </summary>
        public HandlerProveedor()
        {
            proveedorData = new ProveedorData();
        }

        /// <summary>
        /// Metodo para obtener una lista de paises
        /// </summary>
        /// <returns></returns>
        public ProveedorResponseDTO GuardarProveedor(ProveedorRequesteDTO request)
        {
            var response = proveedorData.ProveedorInsertar(request);
            if (response.Success)
            {
                var emailData = new EmailData();

                var proveedorUsuario = proveedorData.GetProvedorDetallePorId(response.IdProveedor);
                if (proveedorUsuario != null)
                {
                    emailData.EnviarEmailProveedorNuevo(proveedorUsuario);
                    emailData.EnviarEmailProveedorNuevoCompras(proveedorUsuario);
                }
            }
            else
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public ProveedorCuentaResponseDTO GuardarProveedorCuenta(ProveedorCuentaRequestDTO request)
        {
            var response = proveedorData.GuardarProveedorCuenta(request);
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public ProveedorCuentaResponseDTO GetProveedorCuentaList(ProveedorCuentaRequestDTO request)
        {
            var response = proveedorData.GetProveedorCuentaList(request);
            var response2 = proveedorData.GetProveedorCuentaAeropuertoList(new ProveedorCuentaRequestDTO { ProveedorCuentaList = response.ProveedorCuentaList });
            response.ProveedorCuentaList = response2.ProveedorCuentaList;

            //response = proveedorData.GetProveedorCuentaAeropuertoList(request);
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public ProveedorDocumentoResponseDTO GetProveedorDocumentoList(ProveedorDocumentoRequestDTO request)
        {
            var response = proveedorData.GetProveedorDocumentoList(request);
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        //public ProveedorCuentaResponseDTO GetProveedorCuentaAeropuertoList(ProveedorCuentaRequestDTO request)
        //{
        //    var response = proveedorData.GetProveedorCuentaAeropuertoList(request);
        //    if (!response.Success)
        //    {
        //        response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
        //    }
        //    return response;
        //}

        public ProveedorDocumentoResponseDTO GuardarProveedorDocumento(ProveedorDocumentoRequestDTO request)
        {
            var response = proveedorData.GuardarProveedorDocumento(request);
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        /// <summary>
        /// Obtiene un listado de provedores por filtro
        /// </summary>
        /// <param name="request">Un objeto de tipo ProveedorEstatusRequestDTO con los filtros</param>
        /// <returns>Un obejeto de tipo ProveedorEstatusResponseDTO</returns>
        public ProveedorEstatusResponseDTO GetProveedorEstatusList(ProveedorEstatusRequestDTO request)
        {
            var response = proveedorData.GetProveedorEstatusList(request);
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        /// <summary>
        /// Obtiene un listado de provedores por filtro
        /// </summary>
        /// <param name="request">Un objeto que contiene el identificador del proveedor</param>
        /// <returns>Un obejeto con el detalle del proveedor</returns>
        public ProveedorDetalleResponseDTO GetProveedorElemento(ProveedorDetalleRequestDTO request)
        {
            var response = proveedorData.GetProveedorElemento(request);
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        /// <summary>
        /// Metodo para insertar el estatus del proveedor
        /// </summary>
        /// <returns>Un objeto de tipo ProveedorResponseDTO con la respuesta</returns>
        public ProveedorEstatusResponseDTO EstatusProveedorInsertar(ProveedorAprobarRequestDTO request)
        {
            var estatus = request.EstatusProveedor.IdEstatusProveedor;
            var response = proveedorData.EstatusProveedorInsertar(request);

            if (response.Success)
            {
                var proveedorUsuario = proveedorData.GetProvedorDetallePorId(request.EstatusProveedor.IdProveedor);
                if (proveedorUsuario != null)
                {
                    var emailData = new EmailData();
                    switch (request.EstatusProveedor.IdEstatusProveedor)
                    {
                        case 1:
                            new EmailData().EnviarEmailProveedorNuevo(proveedorUsuario);
                            new EmailData().EnviarEmailProveedorNuevoCompras(proveedorUsuario);
                            break;
                        case 2:
                            new EmailData().EnviarEmailRechazadoCompras(proveedorUsuario);
                            break;
                        case 4:
                            proveedorUsuario.Password = response.Password;
                            new EmailData().EnviarEmailProveedorAprobadoCompras(proveedorUsuario);
                            break;
                        case 5:
                            new EmailData().EnviarEmailProveedorInfoBancaria(proveedorUsuario);
                            break;
                        case 6:
                            new EmailData().EnviarEmailRechazadoTesoreria(proveedorUsuario);
                            break;
                        case 7:
                            proveedorUsuario.Observaciones = request.EstatusProveedor.Observaciones;
                            new EmailData().EnviarEmailDocumentacionPendiente(proveedorUsuario);
                            break;
                        case 8:
                            new EmailData().EnviarEmailAprobadoTesoreria(proveedorUsuario);
                            break;
                        case 11:
                            new EmailData().EnviarEmailModificacionAprobadaCompras(proveedorUsuario);
                            break;
                        case 12:
                            new EmailData().EnviarEmailModificacionRechazadaCompras(proveedorUsuario);
                            break;
                        case 13:
                            new EmailData().EnviarEmailModificacionAprobadaTesoreria(proveedorUsuario);
                            break;
                        case 14:
                            new EmailData().EnviarEmailModificacionRechazadaTesoreria(proveedorUsuario);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            response.Password = string.Empty;

            return response;
        }

        public ProveedorResponseDTO TempProveedorInsertar(ProveedorRequesteDTO request)
        {
            var response = proveedorData.TempProveedorInsertar(request);
            if (response.Success)
            {
                var proveedorUsuario = proveedorData.GetProvedorDetallePorId(request.Proveedor.IdProveedor);
                if (proveedorUsuario != null)
                {
                    var emailData = new EmailData();
                    new EmailData().EnviarEmailProveedorModificacion(proveedorUsuario);
                }
            }
            else
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public ProveedorInformacionFinanciera GetProveedorInfoFinanciera(ProveedorInformacionFinancieraRequestDTO request)
        {
            var response = new ProveedorInformacionFinanciera();
            response.ProveedorCuentaList = proveedorData.GetProveedorCuentaList(request.IdProveedor);

            var response2 = proveedorData.GetProveedorCuentaAeropuertoList(new ProveedorCuentaRequestDTO { ProveedorCuentaList = response.ProveedorCuentaList });
            response.ProveedorCuentaList = response2.ProveedorCuentaList;
            response.CatalogoDocumentoList = proveedorData.GetProveedorDocumentoList(request.IdProveedor);

            return response;
        }

        public ProveedorFiltroResponseDTO GetProvedorPorFiltro(ProveedorFiltroRequestDTO request)
        {
            var response = new ProveedorFiltroResponseDTO();
            response = proveedorData.GetProvedorPorFiltro(request);

            return response;
        }

        public ContactoResponseDTO GetContactoProveedorList(ContactoRequestDTO request)
        {
            var response = proveedorData.GetContactoProveedorList(request);
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public ContactoResponseDTO UpdateContacto(ContactoRequestDTO request)
        {
            var response = new ContactoResponseDTO();
            if (request.Contacto.IdContacto > 0)
            {
                response = proveedorData.UpdateContacto(request);
            }
            else
            {
                response = proveedorData.InsertContacto(request);
            }
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public ContactoResponseDTO DeleteContacto(ContactoRequestDTO request)
        {
            var response = proveedorData.DeleteContacto(request);
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public InformacionFinancieraResponseDTO InsertarInformacionFinanciera(InformacionFinancieraRequestDTO request)
        {
            var response = proveedorData.GuardaInformacionCuenta(request);
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            else
            {
                var proveedorUsuario = proveedorData.GetProvedorDetallePorId(request.IdProveedor);
                if (proveedorUsuario != null)
                {
                    var emailData = new EmailData();
                    new EmailData().EnviarEmailProveedorModificacion(proveedorUsuario);
                }
            }
            return response;
        }
    }
}
