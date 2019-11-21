using EPROCUREMENT.GAPPROVEEDOR.Business.SeguridadAD;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EPROCUREMENT.GAPPROVEEDOR.Host.Http.Controllers
{
    [RoutePrefix("api/SeguridadAD")]
    public class SeguridadADController : ApiController
    {
        [HttpPost]
        [Route("Login")]
        public LoginUsuarioResponseDTO LoginUsuario([FromBody]LoginUsuarioRequestDTO request)
        {
            var response = new HandlerSeguridadAD().LoginUsuario(request);

            return response;
        }
    }
}
