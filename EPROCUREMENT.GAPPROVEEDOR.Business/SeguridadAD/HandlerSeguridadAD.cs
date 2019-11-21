using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPROCUREMENT.GAPPROVEEDOR.Data.SeguridadADData;
using EPROCUREMENT.GAPPROVEEDOR.Entities;

namespace EPROCUREMENT.GAPPROVEEDOR.Business.SeguridadAD
{
  
        public class HandlerSeguridadAD
        {
            private readonly SeguridadADData seguridadADData;

            public HandlerSeguridadAD()
            {
                seguridadADData = new SeguridadADData();
            }

            public LoginUsuarioResponseDTO LoginUsuario(LoginUsuarioRequestDTO request)
            {
                var response = seguridadADData.SignIn(request);
                if (!response.Success)
                {
                    response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
                }
                return response;
            }


        }
   
}
