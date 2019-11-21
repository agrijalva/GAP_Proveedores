using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eprocurement.Compras.Filters
{
    public class ValidaSession : ActionFilterAttribute
    {
        private UsuarioDTO usuarioInfo;

        public UsuarioDTO ObtenerUsuarioSession()
        {
            return (UsuarioDTO)HttpContext.Current.Session["User"];
        }

        public List<ProveedorCuentaDTO> RecuperaRegistrosSession()
        {
            return (List<ProveedorCuentaDTO>)HttpContext.Current.Session["ProveedorCuentaListRegistro"];
        }
        public int RecuperaIdProveedorSession()
        {
            return (int)HttpContext.Current.Session["IdProveedor"];
        }

    }
}