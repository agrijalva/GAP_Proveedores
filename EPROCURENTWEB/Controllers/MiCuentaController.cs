using EPROCUREMENT.GAPPROVEEDOR.Entities;
using EPROCUREMENT.GAPPROVEEDOR.Entities.Proveedor;
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
            CargarCatalogosAceptar();
            MiCuentaModel miCuenta = new MiCuentaModel
            {
                Proveedor = new ProveedorModel(),
                ActualizaPassword = new ActualizaPasswordModel()
            };
           
            ViewBag.GiroList = giroList;
            ViewBag.ZonaHorariaList = zonaHorariaList;
            ViewBag.NacionalidadList = nacionalidadList;
            ViewBag.PaisList = paisList;
            ViewBag.IdiomaList = idiomaList;
            ViewBag.TipoProveedorList = tipoProveedorList;
            ViewBag.idProveedor = 0;
            ViewBag.accionForm = 1;
            ViewBag.cantidadGiro = giroList.Count;
            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            ViewBag.IdEstatus = usuarioInfo.IdEstatus;
            try
            {
                var proveedor = ObtenerProveedor();
                ViewBag.idProveedor = proveedor.IdProveedor;
                ViewBag.EstadoList = estadoList;
                ViewBag.MunicipioList = municipioList;
                ViewBag.idEstado = proveedor.Direccion.IdEstado;
                ViewBag.idMunicipio = proveedor.Direccion.IdMunicipio;
                foreach(var giro in giroList)
                {
                    giro.Registrado = proveedor.ProveedorGiroList.Exists(x => x.IdCatalogoGiro == giro.IdGiro);
                };

                ViewBag.GiroList = (from g in giroList orderby g.Registrado descending select g).ToList();
                ViewBag.colonias = new List<string>();
                CodigoPostalModel infoCodigo;
                if (proveedor.Direccion.IdPais == 1)
                {
                    infoCodigo = new BusinessLogic().RecuperaCodigoPostalInfo(proveedor.Direccion.CodigoPostal);
                    ViewBag.colonias = infoCodigo.colonias;
                }
                miCuenta.Proveedor = proveedor;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
            return View(miCuenta);
        }

        public ActionResult ActualizarPassword()
        {
            ActualizaPasswordModel actualizaPassword = new ActualizaPasswordModel();
            
            return View(actualizaPassword);
        }

        [HttpPost]
        public ActionResult ActualizarPassword(ActualizaPasswordModel actualizaPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
                    if (new SeguridadBusiness().ResetPasswordUsuario(actualizaPassword, usuarioInfo.IdUsuario))
                    {
                        return Redirect("/MiCuenta/ActualizarPassword#success");
                    }
                    else
                    {
                        return Redirect("/MiCuenta/ActualizarPassword#errorSuccess");
                        ModelState.AddModelError("ErrorGenerico", "Se genero un error al procesar la solicitud");
                    }
                }
            }
            catch
            {
                return Redirect("/MiCuenta/ActualizarPassword#errorSuccess");
            }
            return View(actualizaPassword);
        }

        public JsonResult GetDetalleCuentaList(int idProveedor)
        {
            try
            {
                ProveedorInformacionFinanciera informacionFinanciera = new ProveedorInformacionFinanciera();
                informacionFinanciera = new BusinessLogic().GetProveedorInfoFinanciera(idProveedor);

                ProveedorCuentaResponseDTO proveedorCuentaResponse = new BusinessLogic().GetProveedorCuentaAeropuertoList(new ProveedorCuentaRequestDTO
                {
                    ProveedorCuentaList = informacionFinanciera.ProveedorCuentaList
                });

                //informacionFinanciera.ProveedorCuentaListRegistro = new List<ProveedorCuentaDTO>();
                //informacionFinanciera.ProveedorCuentaListRegistro.Add(new ProveedorCuentaDTO {
                //    CLABE = "123456789012345678",
                //    Cuenta = "1234567890",
                //    NombreBanco = "BBVA",
                //    TipoCuenta = "Débito"
                //});

                return Json(proveedorCuentaResponse.ProveedorCuentaList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public JsonResult GetDocumentosList(int idProveedor)
        {
            try
            {
                ProveedorInformacionFinanciera informacionFinanciera = new ProveedorInformacionFinanciera();
                informacionFinanciera = new BusinessLogic().GetProveedorInfoFinanciera(idProveedor);
                //informacionFinanciera.CatalogoDocumentoList = new List<CatalogoDocumentoDTO>();
                //informacionFinanciera.CatalogoDocumentoList.Add(new CatalogoDocumentoDTO
                //{
                //    NombreDocumento = "PDF",
                //    RutaDocumento = "localhost"
                //});

                ProveedorDocumentoRequestDTO proveedorDocumentoRequest = new ProveedorDocumentoRequestDTO
                {
                    IdProveedor = idProveedor
                };

                var proveedorDocumentoResponse = new BusinessLogic().GetProveedorDocumentoList(proveedorDocumentoRequest);

                return Json(proveedorDocumentoResponse.ProveedorDocumentoList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        [HttpPost]
        public ActionResult ActualizarProveedor(ProveedorModel proveedor)
        {
            try
            {
                var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
                ViewBag.IdEstatus = usuarioInfo.IdEstatus;
                ViewBag.accionForm = 1;
                proveedor.IdTipoProveedor = 1;
                proveedor.IdNacionalidad = 1;
                proveedor.Direccion.DireccionValidada = true;
                CargarCatalogos();
                ViewBag.GiroList = giroList;
                ViewBag.ZonaHorariaList = zonaHorariaList;
                ViewBag.NacionalidadList = nacionalidadList;
                ViewBag.PaisList = paisList;
                ViewBag.IdiomaList = idiomaList;
                ViewBag.EstadoList = estadoList;
                ViewBag.MunicipioList = municipioList;
                ViewBag.TipoProveedorList = tipoProveedorList;
                proveedor.EmpresaList = proveedor.AeropuertoList.Where(a => a.Checado).Select(a => new ProveedorEmpresaModel { IdCatalogoAeropuerto = a.Id }).ToList();
                ViewBag.colonias = new List<string>();
                ViewBag.cantidadGiro = giroList.Count;
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
                    proveedor.IdProveedor = usuarioInfo.IdProveedor;
                    if (new BusinessLogic().PostTempProveedor(proveedor))
                    {
                        //    ViewBag.Respuesta = "Se ha actualizado su contraseña";
                        return Redirect("/MiCuenta/Index#isuccess");
                    }
                    else
                    {
                        ModelState.AddModelError("ErrorGenerico", "Se genero un error al procesar la solicitud");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ErrorGenerico", "Se genero un error al procesar la solicitud");
            }

            MiCuentaModel model = new MiCuentaModel { Proveedor = proveedor };

            return View("Index", model);
        }

        private ProveedorModel ObtenerProveedor()
        {

            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            BusinessLogic businessLogic = new BusinessLogic();
            ProveedorDetalleRequestModel request = new ProveedorDetalleRequestModel();
            request.IdProveedor = usuarioInfo.IdProveedor;
            ProveedorModel proveedor;
            var result = businessLogic.GetProveedorElemento(request);
            if (result.Success)
            {
                var response = businessLogic.GetProveedorElemento(request).Proveedor;
                var empresaList = response.EmpresaList;
                proveedor = new ProveedorModel
                {
                    AeropuertoList = aeropuertoList.Select(a => new AeropuertoModel { Id = a.Id, Nombre = a.Nombre, Checado = empresaList.Where(el => el.IdCatalogoAeropuerto == a.Id).Count() > 0 ? true : false }).ToList(),
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
                    RFC = response.RFC,
                    Mexicana = response.Mexicana,
                    Extranjera = response.Extranjera
                };
            } 
            else
            {
                proveedor = new ProveedorModel();
            }

            return proveedor;
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