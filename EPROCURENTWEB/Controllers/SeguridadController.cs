using EPROCUREMENT.GAPPROVEEDOR.Entities;
using EprocurementWeb.Business;
using EprocurementWeb.Filters;
using EprocurementWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EprocurementWeb.Controllers
{
    public class SeguridadController : Controller
    {
        // GET: Seguridad
        public ActionResult Index()
        {
            UsuarioModel usuario = new UsuarioModel();
            ViewBag.Error = "";
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Login(UsuarioModel usuario)
        {
            try
            {
                UsuarioDTO usuarioDTO = new BusinessLogic().LoginUsuarioItem(usuario.NombreUsuario, usuario.Password);
                if (usuarioDTO == null)
                {
                    ViewBag.Error = "Usuario o contraseña invalida";
                    return View("Index");
                }
                if (usuarioDTO.IdEstatus == 3 ||  usuarioDTO.IdEstatus == 4 || usuarioDTO.IdEstatus == 5 || usuarioDTO.IdEstatus == 7)
                {
                    Session["User"] = usuarioDTO;
                    return RedirectToAction("InformacionBF", "AltaProveedor");
                }

                else if (usuarioDTO.IdEstatus == 1 || usuarioDTO.IdEstatus == 2 || usuarioDTO.IdEstatus == 3 || usuarioDTO.IdEstatus == 6)
                {
                    ViewBag.Error = "El usuario no está activo o fue rechazado";
                    return View("Index");
                }

                ViewBag.Error = "Usuario o contraseña invalida";
                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Se generó un problema al procesar el acceso";
                return View();
            }

        }

        [HttpGet]
        public ActionResult IniciarRecuperacion()
        {
            UsuarioModel usuario = new UsuarioModel();
            //if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
            //{
            //    ViewBag.Error = "Debe ingresar el nombre de usuario.";
            //    return View("Index");
            //}
            //string token = new BusinessLogic().RecuperarPasswordUsuario(usuario.NombreUsuario, true);
            //if (string.IsNullOrEmpty(token))
            //{
            //    ViewBag.Error = "Ocurrió un error al intentar iniciar el proceso";
            //}
            //ViewBag.Error = "Se ha enviado un email a su correo para iniciar la recuperación.";
            //return View("Index");
            return View(usuario);
        }

        [HttpPost]
        public ActionResult IniciarRecuperacion(UsuarioModel usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Email))
            {
                ViewBag.Error = "Debe ingresar el nombre de usuario.";
                return View("Index");
            }
            string token = new BusinessLogic().RecuperarPasswordUsuario(usuario.Email, true);
            if (!string.IsNullOrEmpty(token))
            {
                ViewBag.Error = "Hemos enviado un email a su correo para continuar con el proceso.";
            }
            else
            {
                ViewBag.Error = "Ocurrió un error al intentar iniciar el proceso";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Recovery(string token)
        {
            //var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            //if (usuarioInfo != null)
            //{
            //usuarioInfo.Token = new BusinessLogic().RecuperarPasswordUsuario(usuarioInfo, true);
            //if (usuarioInfo.Token != null)
            //{
            var usuarioModel = new UsuarioModel
            {
                Token = token
            };
            //    return View(usuarioModel);
            //}
            //else
            //{
            //    ViewBag.Error = "Error al generar la solicitud";
            //    return View("Index");
            //}

            //ViewBag.Error = "Error al generar la solicitud";
            return View(usuarioModel);
            //}

            //ViewBag.Error = "Tu token ha expirado";
            //return View("Index");
        }

        [HttpPost]
        public ActionResult Recovery(UsuarioModel usuario)
        {
            ViewBag.Error = "";
            if (usuario != null && usuario.PasswordNueva != null && usuario.ConfirmarPassword != null)
            {
                //var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
                //if (usuarioInfo != null)
                //{
                //usuarioInfo.Token = usuario.Token;
                if (!new BusinessLogic().RecuperarPasswordUsuario(usuario, false))
                {
                    ViewBag.Error = "Error al generar la solicitud";
                } else
                {
                    ViewBag.Error = "Sin error";
                }
                //}
            } else
            {
                ViewBag.Error = "Las contraseñas deben ser igules";
            }

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetInoformacionUsuario()
        {
            UsuarioDTO usuario = null;
            usuario = new ValidaSession().ObtenerUsuarioSession();
            return Json(usuario, JsonRequestBehavior.AllowGet);
        }
    }
}