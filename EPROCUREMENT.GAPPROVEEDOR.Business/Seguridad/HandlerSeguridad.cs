using EPROCUREMENT.GAPPROVEEDOR.Data;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Business.Seguridad
{
    public class HandlerSeguridad
    {
        private readonly SeguridadData seguridadData;

        public HandlerSeguridad()
        {
            seguridadData = new SeguridadData();
        }

        public LoginUsuarioResponseDTO LoginUsuario(LoginUsuarioRequestDTO request)
        {
            var response = seguridadData.LoginUsuario(request);
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }

        public ResetPasswordResponseDTO RecuperarPasswordUsuario(ResetPasswordRequestDTO request)
        {
            var response = seguridadData.RecuperarPasswordUsuario(request);
            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }
        public ActualizaPasswordResponseDTO ActualizaPasswordUsuario(ActualizaPasswordRequestDTO request)
        {
            var response = seguridadData.ActualizaPasswordUsuario(request);
            if (!response.Success && !response.ErrorList.Any())
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "EG" } };
            }
            return response;
        }
    }
}
