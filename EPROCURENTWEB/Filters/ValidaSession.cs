using EPROCUREMENT.GAPPROVEEDOR.Entities;
using EprocurementWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EprocurementWeb.Filters
{
    public class ValidaSession : ActionFilterAttribute
    {
        private UsuarioDTO usuarioInfo;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);
                usuarioInfo = (UsuarioDTO)HttpContext.Current.Session["User"];
                if (usuarioInfo == null)
                {

                    if (filterContext.Controller is SeguridadController == false && filterContext.Controller is HomeController == false && filterContext.Controller is AltaProveedorController == false)
                    {
                        //filterContext.HttpContext.Response.Redirect("~/Seguridad/Index");
                        filterContext.Result = new RedirectResult("~/Seguridad/Index");
                    }
                }
            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Seguridad/Index");
            }
        }

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