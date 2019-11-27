using Eprocurement.Compras.Business;
using Eprocurement.Compras.Models;
using Eprocurement.Compras.Service;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using EPROCUREMENT.GAPPROVEEDOR.Entities.Seguridad;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.Mvc;

namespace Eprocurement.Compras.Controllers
{
    public class SeguridadADController : Controller
    {
        // GET: SeguridadAD
        public ActionResult Index()
        {
            UsuarioModel usuario = new UsuarioModel();
            ViewBag.Error = "";
            return View(usuario);
        }
        [HttpPost]
        public ActionResult Login(UsuarioModel usuario)
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            var authenticationService = new AuthenticationService(authenticationManager);

            AuthenticationResult authenticationResult = authenticationService.SignIn(usuario.NombreUsuario, usuario.Password);

            if (authenticationResult.IsSuccess)
            {
                try
                {
                    UsuarioDTO usuarioDTO = new BusinessLogic().LoginUsuarioItem(usuario.NombreUsuario, usuario.Password);
                    if (usuarioDTO == null)
                    {
                        ViewBag.Error = "Usuario o contraseña invalida";
                        return View("Index", "SeguridadAD");
                    }

                    Session["User"] = usuarioDTO;

                    if (usuarioDTO.IdUsuarioRol == 2)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("AprobarTesoreria", "Tesoreria");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View("Index");
                }
            }

            else
            {
                ViewBag.Error = authenticationResult.ErrorMessage;
                return View("Index");
            }
        }


    }
}
