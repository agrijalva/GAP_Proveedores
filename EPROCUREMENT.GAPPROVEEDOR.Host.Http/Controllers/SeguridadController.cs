using EPROCUREMENT.GAPPROVEEDOR.Business.Seguridad;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EPROCUREMENT.GAPPROVEEDOR.Host.Http.Controllers
{
    [RoutePrefix("api/Seguridad")]
    public class SeguridadController : ApiController
    {
        // GET: api/Login
        [HttpPost]
        [Route("Login")]
        public LoginUsuarioResponseDTO LoginUsuario([FromBody]LoginUsuarioRequestDTO request)
        {
            var response = new HandlerSeguridad().LoginUsuario(request);

            return response;
        }

        // GET: api/Login
        [HttpPost]
        [Route("IniciarRecovery")]
        public ResetPasswordResponseDTO RecuperarPasswordUsuario([FromBody]ResetPasswordRequestDTO request)
        {
            var response = new HandlerSeguridad().RecuperarPasswordUsuario(request);

            return response;
        }

        // GET: api/ActualizaPassword
        [HttpPost]
        [Route("ActualizaPassword")]
        public ActualizaPasswordResponseDTO ActualizaPasswordUsuario([FromBody]ActualizaPasswordRequestDTO request)
        {
            var response = new HandlerSeguridad().ActualizaPasswordUsuario(request);

            return response;
        }
    }
}
