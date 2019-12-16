﻿using EPROCUREMENT.GAPPROVEEDOR.Data;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Business.SolicitudFactura
{
    public class HandlerSolicitudFactura
    {
        private readonly SolicitudFacturaData solicitudFacturaData;

        public HandlerSolicitudFactura()
        {
            solicitudFacturaData = new SolicitudFacturaData();
        }

        public SolicitudFacturaResponseDTO GetSolicitudFacturaList(SolicitudFacturaRequestDTO request)
        {
            var response = solicitudFacturaData.GetSolicitudFacturaList(request);

            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public SolicitudFacturaDetalleResponseDTO GetSolicitudFacturaCabecero(SolicitudFacturaDetalleRequestDTO request)
        {
            var response = solicitudFacturaData.GetSolicitudFacturaCabecero(request);

            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public FacturaResponseDTO GetFacturaList(FacturaRequestDTO request)
        {
            var response = solicitudFacturaData.GetFacturaList(request);

            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public EstatusSolicitudResponseDTO GuardarHistoricoEstatusSolicitud(EstatusSolicitudRequestDTO request)
        {
            var response = solicitudFacturaData.GuardarHistoricoEstatusSolicitud(request);

            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public AeropuertoListaResponseDTO GetAeropuertoLista()
        {
            var response = solicitudFacturaData.GetAeropuertoLista();

            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

    }
}
