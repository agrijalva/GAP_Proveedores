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
    public class MiCuentaController : Controller
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

        private void CargarCatalogosAceptar()
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

        // GET: MiCuenta
        public ActionResult Index()
        {
            MiCuentaModel miCuenta = new MiCuentaModel
            {
                Proveedor = new ProveedorRegistro(),
                ActualizaPassword = new ActualizaPasswordModel()
            };
            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            CargarCatalogosAceptar();
            ProveedorRegistro proveedor;
            ViewBag.GiroList = giroList;
            ViewBag.ZonaHorariaList = zonaHorariaList;
            ViewBag.NacionalidadList = nacionalidadList;
            ViewBag.PaisList = paisList;
            ViewBag.IdiomaList = idiomaList;
            ViewBag.TipoProveedorList = tipoProveedorList;
            ViewBag.idProveedor = usuarioInfo.IdProveedor;

            ViewBag.Respuesta = "";
            ViewBag.Accion = 1;
            try
            {
                BusinessLogic businessLogic = new BusinessLogic();
                ProveedorDetalleRequestDTO request = new ProveedorDetalleRequestDTO();
                request.IdProveedor = usuarioInfo.IdProveedor;

                var response = businessLogic.GetProveedorElemento(request).Proveedor;
                var empresaList = response.EmpresaList;
                proveedor = new ProveedorRegistro
                {
                    AeropuertoList = aeropuertoList.Select(a => new AeropuertoDTO { Id = a.Id, Nombre = a.Nombre, Checado = empresaList.Where(el => el.IdCatalogoAeropuerto == a.Id).Count() > 0 ? true : false }).ToList(),
                    AXFechaRegistro = response.AXFechaRegistro,
                    AXNumeroProveedor = response.AXNumeroProveedor,
                    Contacto = response.Contacto,
                    Direccion = response.Direccion,
                    EmpresaList = response.EmpresaList,
                    IdNacionalidad = response.IdNacionalidad,
                    IdProveedor = response.IdProveedor,
                    IdTipoProveedor = response.IdTipoProveedor,
                    IdZonaHoraria = response.IdZonaHoraria,
                    NIF = response.NIF,
                    NombreEmpresa = response.NombreEmpresa,
                    PaginaWeb = response.PaginaWeb,
                    ProveedorGiroList = response.ProveedorGiroList,
                    ProvFax = response.ProvFax,
                    ProvTelefono = response.ProvTelefono,
                    RazonSocial = response.RazonSocial,
                    RFC = response.RFC
                };
                ViewBag.EstadoList = estadoList;
                ViewBag.MunicipioList = municipioList;
                ViewBag.idEstado = proveedor.Direccion.IdEstado;
                ViewBag.idMunicipio = proveedor.Direccion.IdMunicipio;
                ViewBag.colonias = new List<string>();
                proveedor.Mexicana = true;
                CodigoPostalModel infoCodigo;
                if (proveedor.Direccion.IdPais == 1)
                {
                    infoCodigo = new BusinessLogic().RecuperaCodigoPostalInfo(proveedor.Direccion.CodigoPostal);
                    ViewBag.colonias = infoCodigo.colonias;
                }
                //if (proveedor.Direccion.IdPais == 1)
                //{
                //    ViewBag.EstadoList = businessLogic.GetEstadoList(proveedor.Direccion.IdPais);

                //    if (proveedor.Direccion.IdEstado > 0)
                //    {
                //        ViewBag.idEstado = proveedor.Direccion.IdEstado;
                //        ViewBag.MunicipioList = businessLogic.GetMunicipioList(proveedor.Direccion.IdEstado);
                //    }
                //}
                miCuenta.Proveedor = proveedor;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
            return View(miCuenta);
        }

        [HttpPost]
        public ActionResult ActualizarPassword(ActualizaPasswordModel actualizaPassword)
        {
            if (ModelState.IsValid)
            {
                var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
                if (new SeguridadBusiness().ResetPasswordUsuario(actualizaPassword, usuarioInfo.IdUsuario))
                {
                    ViewBag.Respuesta = "Se ha actualizado su contraseña";                    
                }
                else
                {
                    ModelState.AddModelError("ErrorGenerico", "Se genero un error al procesar la solicitud");
                }
            }
            ViewBag.Accion = 2;
            return View("Index");
        }

        [HttpPost]
        public ActionResult ActualizarProveedor(ProveedorRegistro proveedor)
        {
            //if (ModelState.IsValid)
            //{
            //var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            //if (new BusinessLogic().PostTempProveedor(proveedor))
            //{
            //    //    ViewBag.Respuesta = "Se ha actualizado su contraseña";
            //}
            //else
            //{
            //    ModelState.AddModelError("ErrorGenerico", "Se genero un error al procesar la solicitud");
            //}
            //}
            return View("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCodigoPostalInfo(string codigoPostal)
        {
            var infoCodigo = new BusinessLogic().RecuperaCodigoPostalInfo(codigoPostal); ;
            return Json(infoCodigo, JsonRequestBehavior.AllowGet);
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

    }
}