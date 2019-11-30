using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EprocurementWeb.Models;
using EprocurementWeb.Content.Texts;
using System.Net.Http;
using System.Web.Script.Serialization;
using EprocurementWeb.Business;
using System.Threading.Tasks;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using EprocurementWeb.Models;
using System.Net;

namespace EprocurementWeb.Controllers
{
    public class HomeController : BaseController
    {
        public List<AeropuertoModel> aeropuertoList;
        public List<ZonaHorariaDTO> zonaHorariaList;
        public List<NacionalidadDTO> nacionalidadList;
        public List<GiroDTO> giroList;
        public List<PaisDTO> paisList;
        public List<IdiomaDTO> idiomaList;
        public List<EstadoDTO> estadoList;
        public List<MunicipioDTO> municipioList;
        public List<TipoProveedorDTO> tipoProveedorList;

        public ActionResult Index()
        {
            CargarCatalogos();
            ProveedorModel proveedor = new ProveedorModel { Contacto = null, Direccion = null };
            proveedor.AeropuertoList = aeropuertoList;
            ViewBag.GiroList = giroList;
            if (giroList != null)
            {
                ViewBag.cantidadGiro = giroList.Count;
            }
            else
            {
                ViewBag.cantidadGiro = 0;
            }
            ViewBag.ZonaHorariaList = zonaHorariaList;
            ViewBag.NacionalidadList = nacionalidadList;
            ViewBag.PaisList = paisList;
            ViewBag.IdiomaList = idiomaList;
            ViewBag.EstadoList = estadoList;
            ViewBag.MunicipioList = municipioList;
            ViewBag.TipoProveedorList = tipoProveedorList;
            proveedor.Mexicana = true;
            ViewBag.colonias = new List<string>();
            ViewBag.errorResultado = 0;
            return View(proveedor);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarProveedor(ProveedorModel proveedor)
        {
            try
            {
                proveedor.IdTipoProveedor = 1;
                proveedor.IdNacionalidad = 1;
                proveedor.Direccion.DireccionValidada = true;
                //proveedor.Direccion.IdPais = proveedor.i
                CargarCatalogos();
                ViewBag.GiroList = giroList; if (giroList != null)
                {
                    ViewBag.cantidadGiro = giroList.Count;
                }
                else
                {
                    ViewBag.cantidadGiro = 0;
                }
                ViewBag.ZonaHorariaList = zonaHorariaList;
                ViewBag.NacionalidadList = nacionalidadList;
                ViewBag.PaisList = paisList;
                ViewBag.IdiomaList = idiomaList;
                ViewBag.EstadoList = estadoList;
                ViewBag.MunicipioList = municipioList;
                ViewBag.TipoProveedorList = tipoProveedorList;
                ViewBag.errorResultado = 0;
                proveedor.EmpresaList = proveedor.AeropuertoList.Where(a => a.Checado).Select(a => new ProveedorEmpresaModel { IdCatalogoAeropuerto = a.Id }).ToList();
                ViewBag.colonias = new List<string>();
                if (proveedor.ProveedorGiroList == null || !proveedor.ProveedorGiroList.Exists(x => x.IdCatalogoGiro > 0))
                {
                    ModelState.AddModelError("ErrorEmpresa", "Debe agregar al menos una empresa");
                }
                if (proveedor.AeropuertoList == null || !proveedor.AeropuertoList.Exists(x => x.Checado))
                {
                    ModelState.AddModelError("ErrorAeropuerto", "Debe seleccionar al menos un aeropuerto");
                }
                if (!proveedor.Mexicana && !proveedor.Extranjera)
                {
                    ModelState.AddModelError("ErrorTipoEmpresa", "Debe seleccionar una opción");
                }

                for (var pos = 0; pos < proveedor.ProveedorGiroList.Count; pos++)
                {
                    if (ModelState["ProveedorGiroList[" + pos + "].IdCatalogoGiro"] != null)
                    {
                        ModelState["ProveedorGiroList[" + pos + "].IdCatalogoGiro"].Errors.Clear();
                    }
                }
                if (ModelState.IsValid)
                {
                    BusinessLogic businessLogic = new BusinessLogic();
                    ProveedorResponseModel response = businessLogic.PostProveedor(proveedor);
                    if (response.Success)
                    {
                        return Redirect("/Home/Index#success");
                        //return RedirectToAction("Contact");
                    }
                    else
                    {
                        ViewBag.errorResultado = 1;
                    }
                }
            }
            catch
            {

            }
            return View(proveedor);
        }

        private void CargarCatalogos()
        {
            BusinessLogic businessLogic = new BusinessLogic();
            aeropuertoList = businessLogic.GetAeropuertosList();
            zonaHorariaList = businessLogic.GetZonaHorariaList();
            nacionalidadList = businessLogic.GetNacionalidadList();
            giroList = businessLogic.GetGirosList();
            paisList = businessLogic.GetPaisesList();
            idiomaList = businessLogic.GetIdiomaList();
            tipoProveedorList = businessLogic.GetTipoProveedorList();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetEstados(int idPais)
        {
            BusinessLogic businessLogic = new BusinessLogic();
            estadoList = businessLogic.GetEstadoList(idPais);
            return Json(estadoList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetMunicipios(int idEstado)
        {
            BusinessLogic businessLogic = new BusinessLogic();
            municipioList = businessLogic.GetMunicipioList(idEstado);
            return Json(municipioList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCodigoPostalInfo(string codigoPostal)
        {
            var infoCodigo = new BusinessLogic().RecuperaCodigoPostalInfo(codigoPostal);
            return Json(infoCodigo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("http://localhost:54535/api/Proveedor/");
            //    //HTTP GET
            //    var responseTask = client.GetAsync("getlist");
            //    responseTask.Wait();


            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        var readTask = result.Content.ReadAsStringAsync();
            //    JavaScriptSerializer JSserializer = new JavaScriptSerializer();

            //    var proveedor = JSserializer.Deserialize<List<ProveedorModel>>(readTask.Result);
            //    var list = proveedor;
            //    //    readTask.Wait();
            //    //    var proveedores = readTask.Result;
            //    }
            //    //else //web api sent error response 
            //    //{
            //    //    //log response status here..

            //    //    students = Enumerable.Empty<StudentViewModel>();

            //    //    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            //    //}
            //}


            ViewBag.Title = EprocurementWeb.GlobalResources.RHome.About;
            ViewBag.Message = @EprocurementWeb.GlobalResources.RHome.AboutMessage;

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Title = @EprocurementWeb.GlobalResources.RHome.Contact;
            ViewBag.Message = @EprocurementWeb.GlobalResources.RHome.ContactMessage;
            ViewBag.ContactResult = TempData["ContactResult"];
            ViewBag.ContactResultMessage = TempData["ContactResultMessage"] ?? "";
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactModel model)
        {
            ViewBag.Title = @EprocurementWeb.GlobalResources.RHome.Contact;
            ViewBag.Message = @EprocurementWeb.GlobalResources.RHome.ContactMessage;
            if (ModelState.IsValid)
            {
                /* Do something with this information */
                TempData["ContactResult"] = true;
                TempData["ContactResultMessage"] = @EprocurementWeb.GlobalResources.RHome.ContactMessageSendOk;
                return RedirectToAction("Contact"); /* Post-Redirect-Get Pattern */
            }
            ViewBag.ContactResult = false;
            ViewBag.ContactResultMessage = @EprocurementWeb.GlobalResources.RHome.ContactMessageSendNok;
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ValidaRFC(string rfc)
        {
            var resultado = string.Empty;
            BusinessLogic businessLogic = new BusinessLogic();
            ProveedorFiltroRequestModel request = new ProveedorFiltroRequestModel { Filtro = rfc, TipoFiltro = Models.TipoFiltro.RFC };
            var response = businessLogic.ObtenerProveedorFiltro(request);
            if(response.Success)
            {
                if(response.Proveedor != null)
                {
                    resultado = "El RFC: " + rfc + " ya se encuentra registrado.";
                }
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ValidaEmail(string email)
        {
            var resultado = string.Empty;
            BusinessLogic businessLogic = new BusinessLogic();
            ProveedorFiltroRequestModel request = new ProveedorFiltroRequestModel { Filtro = email, TipoFiltro = Models.TipoFiltro.Email };
            var response = businessLogic.ObtenerProveedorFiltro(request);
            if (response.Success)
            {
                if (response.Proveedor != null)
                {
                    resultado = "El Email: " + email + " ya se encuentra registrado.";
                }
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
    }
}