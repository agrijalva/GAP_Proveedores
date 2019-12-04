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
    public class ContactoController : Controller
    {
        // GET: Contacto
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetContactos()
        {
            ContactoResponseDTO proveedor;
            List<ProveedorContactoDTO> contactoList = new List<ProveedorContactoDTO>();
            try
            {
                var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
                BusinessLogic businessLogic = new BusinessLogic();
                ContactoRequestDTO request = new ContactoRequestDTO();
                request.IdProveedor = usuarioInfo.IdProveedor;

                var response = businessLogic.GetContactoProveedorList(request);
                if (response.Success)
                {
                    foreach(var contacto in response.ContactoList)
                    {
                        contacto.EsPrincipal = contacto.ContactoPrincipal == 1;
                        contacto.Tipo = contacto.ContactoPrincipal == 1 ? "Principal" : "Opcional";
                    }
                    var totalPrincipal = response.ContactoList.Count(x => x.EsPrincipal);
                    ViewBag.totalPrincipal = totalPrincipal;
                    contactoList = response.ContactoList;
                }

            }
            catch (Exception ex)
            {
                return Json(new List<ProveedorContactoDTO>(), JsonRequestBehavior.AllowGet);
            }
            return Json(contactoList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NuevoContacto()
        {
            ProveedorContactoModel contacto = new ProveedorContactoModel();
            BusinessLogic businessLogic = new BusinessLogic();
            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            contacto.IdProveedor = usuarioInfo.IdProveedor;
            ViewBag.ZonaHorariaList = businessLogic.GetZonaHorariaList();
            ViewBag.NacionalidadList = businessLogic.GetNacionalidadList();
            ViewBag.PaisList = businessLogic.GetPaisesList();
            return View(contacto);
        }

        public ActionResult EditarContacto(int idContacto)
        {
            ProveedorContactoModel contacto = new ProveedorContactoModel();
            BusinessLogic businessLogic = new BusinessLogic();
            ViewBag.ZonaHorariaList = businessLogic.GetZonaHorariaList();
            ViewBag.NacionalidadList = businessLogic.GetNacionalidadList();
            ViewBag.PaisList = businessLogic.GetPaisesList();

            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            ContactoRequestDTO request = new ContactoRequestDTO();
            request.IdProveedor = usuarioInfo.IdProveedor;
            var response = businessLogic.GetContactoProveedorList(request);
            if (response.Success)
            {
                var contactoItem = response.ContactoList.FirstOrDefault(x => x.IdContacto == idContacto);
                if (contactoItem != null)
                {
                    contacto = new ProveedorContactoModel
                    {
                        IdContacto = contactoItem.IdContacto,
                        Cargo = contactoItem.Cargo,
                        Email = contactoItem.Email,
                        Fax = contactoItem.Fax,
                        IdIdioma = contactoItem.IdIdioma,
                        IdNacionalidad = contactoItem.IdNacionalidad,
                        IdPais = contactoItem.IdPais,
                        IdProveedor = contactoItem.IdProveedor,
                        IdZonaHoraria = contactoItem.IdZonaHoraria,
                        NombreContacto = contactoItem.NombreContacto,
                        TelefonoDirecto = contactoItem.TelefonoDirecto,
                        TelefonoMovil = contactoItem.TelefonoMovil,
                        EsPrincipal = contactoItem.ContactoPrincipal > 0
                    };
                }
            }
            return View(contacto);
        }

        [HttpPost, ActionName("EditarContacto")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarContacto(ProveedorContactoModel contactoItem)
        {
            try
            {
                BusinessLogic businessLogic = new BusinessLogic();
                if (ModelState.IsValid)
                {
                    ContactoRequestDTO request = new ContactoRequestDTO();
                    request.Contacto = new ProveedorContactoDTO
                    {
                        IdContacto = contactoItem.IdContacto,
                        Cargo = contactoItem.Cargo,
                        Email = contactoItem.Email,
                        Fax = contactoItem.Fax,
                        IdIdioma = contactoItem.IdIdioma,
                        IdNacionalidad = contactoItem.IdNacionalidad,
                        IdPais = contactoItem.IdPais,
                        IdProveedor = contactoItem.IdProveedor,
                        IdZonaHoraria = contactoItem.IdZonaHoraria,
                        NombreContacto = contactoItem.NombreContacto,
                        TelefonoDirecto = contactoItem.TelefonoDirecto,
                        TelefonoMovil = contactoItem.TelefonoMovil,
                        ContactoPrincipal = contactoItem.EsPrincipal ? 1 : 0
                    };
                    var response = businessLogic.UpdateContacto(request);
                    if (response.Success)
                    {
                        return Redirect("/Contacto/Index#updateSuccess");
                    }
                }
                else
                {
                    ViewBag.ZonaHorariaList = businessLogic.GetZonaHorariaList();
                    ViewBag.NacionalidadList = businessLogic.GetNacionalidadList();
                    ViewBag.PaisList = businessLogic.GetPaisesList();
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("ErrorGeneral", "Se generó un error al procesar la solicitud.");
            }

            return View(contactoItem);
        }

        [HttpPost, ActionName("NuevoContacto")]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoContacto(ProveedorContactoModel contactoItem)
        {
            try
            {
                BusinessLogic businessLogic = new BusinessLogic();

                var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
                if (ModelState.IsValid)
                {
                    var correoValido = businessLogic.ValidacionCampos(new ContactoRequestDTO { Contacto = new ProveedorContactoDTO { Email = contactoItem.Email } });

                    if (correoValido)
                    {
                        ContactoRequestDTO request = new ContactoRequestDTO();
                        request.Contacto = new ProveedorContactoDTO
                        {
                            Cargo = contactoItem.Cargo,
                            Email = contactoItem.Email,
                            Fax = contactoItem.Fax,
                            IdIdioma = contactoItem.IdIdioma,
                            IdNacionalidad = contactoItem.IdNacionalidad,
                            IdPais = contactoItem.IdPais,
                            IdProveedor = usuarioInfo.IdProveedor,
                            IdZonaHoraria = contactoItem.IdZonaHoraria,
                            NombreContacto = contactoItem.NombreContacto,
                            TelefonoDirecto = contactoItem.TelefonoDirecto,
                            TelefonoMovil = contactoItem.TelefonoMovil,
                            ContactoPrincipal = contactoItem.EsPrincipal ? 1 : 0
                        };
                        var response = businessLogic.UpdateContacto(request);
                        if (response.Success)
                        {
                            return Redirect("/Contacto/Index#insertSuccess");
                        }
                    }
                    else
                    {
                        ViewBag.ZonaHorariaList = businessLogic.GetZonaHorariaList();
                        ViewBag.NacionalidadList = businessLogic.GetNacionalidadList();
                        ViewBag.PaisList = businessLogic.GetPaisesList();
                        ModelState.AddModelError("Email", "Este Email ya se encuentra registrado");
                    }                  
                }
                else
                {
                    ViewBag.ZonaHorariaList = businessLogic.GetZonaHorariaList();
                    ViewBag.NacionalidadList = businessLogic.GetNacionalidadList();
                    ViewBag.PaisList = businessLogic.GetPaisesList();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ErrorGeneral", "Se generó un error al procesar la solicitud.");
            }

            return View(contactoItem);
        }

        public JsonResult EliminarContacto(int idContacto)
        {
            try
            {
                BusinessLogic businessLogic = new BusinessLogic();
                var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
                ContactoRequestDTO request = new ContactoRequestDTO();
                request.Contacto = new ProveedorContactoDTO
                {
                    IdContacto = idContacto,
                    IdProveedor = usuarioInfo.IdProveedor
                };

                var response = businessLogic.DeleteContacto(request);
                if (response.Success)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                //ProveedorAprobarRequestDTO request = new ProveedorAprobarRequestDTO();
                //request.EstatusProveedor = new HistoricoEstatusProveedorDTO { IdProveedor = idProveedor, IdEstatusProveedor = estatus, IdUsuario = 3, Observaciones = observaciones };

                //var response = businessLogic.SetProveedorEstatus(request);
            }
            catch (Exception ex)
            {

                return null;
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ActualizaPrincipal(int idContacto, bool principal)
        {
            var respuesta = false;
            try
            {

                var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
                BusinessLogic businessLogic = new BusinessLogic();
                ContactoRequestDTO request = new ContactoRequestDTO();
                request.IdProveedor = usuarioInfo.IdProveedor;

                var response = businessLogic.GetContactoProveedorList(request);
                if (response.Success)
                {
                    var contacto = response.ContactoList.First(x => x.IdContacto == idContacto);
                    contacto.EsPrincipal = principal;
                    contacto.ContactoPrincipal = principal ? 1 : 0;
                    request.Contacto = contacto;
                    var responseUpdate = businessLogic.UpdateContacto(request);
                    respuesta = response.Success;
                }

            }
            catch (Exception ex)
            {

                return null;
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
    }
}