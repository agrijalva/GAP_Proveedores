using Eprocurement.Compras.Business;
using Eprocurement.Compras.Models;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                UsuarioDTO usuarioDTO = new BusinessLogic().LoginUsuarioItem(usuario.NombreUsuario, usuario.Password);
                if (usuarioDTO == null)
                {
                    ViewBag.Error = "Usuario o contraseña invalida";
                    return View("Index");
                }

                Session["User"] = usuarioDTO;

                if(usuarioDTO.IdUsuarioRol == 2)
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
                return View();
            }

        }

       
    }
}
