using EPROCUREMENT.GAPPROVEEDOR.Entities;
using EPROCUREMENT.GAPPROVEEDOR.Entities.Proveedor;
using EprocurementWeb.Business;
using EprocurementWeb.Filters;
using EprocurementWeb.Models;
using Newtonsoft.Json;
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
            
            ViewBag.accionForm = 1;
            ViewBag.cantidadGiro = giroList.Count;
            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            ViewBag.IdEstatus = usuarioInfo.IdEstatus;
            ViewBag.IdProveedor = usuarioInfo.IdProveedor;
            try
            {
                BusinessLogic business = new BusinessLogic();
                ProveedorCuentaRequestDTO proveedorCuentaRequest = new ProveedorCuentaRequestDTO
                {
                    IdProveedor = usuarioInfo.IdProveedor
                };
                var proveedorCuentaResponse = business.GetProveedorCuentaList(proveedorCuentaRequest);
                ViewBag.ProveedorCuentaList = proveedorCuentaResponse.ProveedorCuentaList;

                ProveedorDocumentoRequestDTO proveedorDocumentoRequest = new ProveedorDocumentoRequestDTO
                {
                    IdProveedor = usuarioInfo.IdProveedor
                };

                var aeropuertos = business.GetAeropuertosList();
                ViewBag.BancoList = business.GetBancoList();
                ViewBag.TipoCuentaList = business.GetTipoCuentaList();
                ProveedorDetalleRequestModel request = new ProveedorDetalleRequestModel();
                request.IdProveedor = usuarioInfo.IdProveedor;
                var response = business.GetProveedorElemento(request).Proveedor;
                var aeropuertosAsignados = response.EmpresaList;

                var proveedorDocumentoResponse = business.GetProveedorDocumentoList(proveedorDocumentoRequest);
                ViewBag.ProveedorDocumentoList = proveedorDocumentoResponse.ProveedorDocumentoList;

                var ProveedorCuentaList = new List<ProveedorCuentaDTO> { new ProveedorCuentaDTO { Cuenta = null, IdBanco = 0, CLABE = null, IdTipoCuenta = 0, IdProveedor = usuarioInfo.IdProveedor } };
                ProveedorCuentaList[0].AeropuertoList = (from aeropuerto in aeropuertos
                                                                join aeropuertoA in aeropuertosAsignados on aeropuerto.Id equals aeropuertoA.IdCatalogoAeropuerto
                                                                select new AeropuertoDTO { Id = aeropuerto.Id, Nombre = aeropuerto.Nombre, Checado = false }).ToList();
                miCuenta.ProveedorCuentaList = ProveedorCuentaList;

                var proveedorDocumento = business.GetCatalogoDocumentoList();
                miCuenta.CatalogoDocumentoList = proveedorDocumento;

                var proveedor = ObtenerProveedor();
                ViewBag.IdEstatus = proveedor.IdEstatus;
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
                ProveedorCuentaRequestDTO proveedorCuentaRequest = new ProveedorCuentaRequestDTO
                {
                    IdProveedor = idProveedor
                };
                var proveedorCuentaResponse = new BusinessLogic().GetProveedorCuentaList(proveedorCuentaRequest);
                ViewBag.ProveedorCuentaList = proveedorCuentaResponse.ProveedorCuentaList;

                //ProveedorInformacionFinanciera informacionFinanciera = new ProveedorInformacionFinanciera();
                //informacionFinanciera = new BusinessLogic().GetProveedorInfoFinanciera(idProveedor);

                //ProveedorCuentaResponseDTO proveedorCuentaResponse = new BusinessLogic().GetProveedorCuentaAeropuertoList(new ProveedorCuentaRequestDTO
                //{
                //    ProveedorCuentaList = informacionFinanciera.ProveedorCuentaList
                //});

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
                        usuarioInfo.IdEstatus = 9;
                        Session["User"] = usuarioInfo;
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
                    Extranjera = response.Extranjera,
                    IdEstatus = response.IdEstatus
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

        [HttpPost]
        public ActionResult Upload()
        {
            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            // Logica anterior
            BusinessLogic business = new BusinessLogic();
            int idProveedor = usuarioInfo.IdProveedor;
            var aeropuertos = business.GetAeropuertosList();
            ViewBag.BancoList = business.GetBancoList();
            ViewBag.TipoCuentaList = business.GetTipoCuentaList();
            List<ProveedorDocumentoDTO> provDoctos = new List<ProveedorDocumentoDTO>();

            string cuentaJson = Request["cuenta"];
            if (cuentaJson.Length > 0)
            {
                var cuenta = (ProveedorInformacionFinanciera)JsonConvert.DeserializeObject(cuentaJson, typeof(ProveedorInformacionFinanciera));

                var informacionfinanciera = new InformacionFinancieraRequestDTO()
                {
                    IdProveedor = idProveedor,
                    ProveedorCuentaList = cuenta.ProveedorCuentaListRegistro,
                    ProveedorDocumentoList = new List<ProveedorDocumentoDTO>()
                };

                bool extensionesValidas = true;
                for (int j = 0; j < cuenta.ProveedorDocumentoList.Count; j++)
                {
                    var division = cuenta.ProveedorDocumentoList[j].NombreArchivo.Split('.');
                    var extension = division.Last();
                    if (!cuenta.ProveedorDocumentoList[j].Extensiones.Contains(extension))
                    {
                        extensionesValidas = false;
                    }
                    string nombreArchivo = idProveedor + "-" + cuenta.ProveedorDocumentoList[j].IdCatalogoDocumento + '.' + extension;

                    cuenta.ProveedorDocumentoList[j] = new ProveedorDocumentoDTO
                    {
                        IdCatalogoDocumento = Convert.ToInt32(cuenta.ProveedorDocumentoList[j].IdCatalogoDocumento),
                        IdProveedor = idProveedor,
                        DescripcionDocumento = "NA",
                        DocumentoAutorizado = false,
                        NombreArchivo = nombreArchivo
                    };
                }

                int contador = Request.Files.Count;

                cuenta.CatalogoDocumentoList = new List<CatalogoDocumentoDTO>();

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    string idCatalogoDocumento = Request.Files.GetKey(i);
                    var division = file.FileName.Split('.');
                    var extension = division.Last();
                    string nombreArchivo = idProveedor + "-" + idCatalogoDocumento + '.' + extension;

                    for(int j=0;j<cuenta.ProveedorDocumentoList.Count; j++)
                    {
                        if (!cuenta.ProveedorDocumentoList[j].Extensiones.Contains(extension))
                        {
                            extensionesValidas = false;
                        }
                        if (cuenta.ProveedorDocumentoList[j].IdCatalogoDocumento == Convert.ToInt32(idCatalogoDocumento))
                        {
                            cuenta.ProveedorDocumentoList[j] = new ProveedorDocumentoDTO
                            {
                                IdCatalogoDocumento = Convert.ToInt32(idCatalogoDocumento),
                                IdProveedor = idProveedor,
                                DescripcionDocumento = "NA",
                                DocumentoAutorizado = false,
                                NombreArchivo = nombreArchivo
                            };
                        }
                    }

                    cuenta.CatalogoDocumentoList.Add(new CatalogoDocumentoDTO
                    {
                        IdCatalogoDocumento = Convert.ToInt32(idCatalogoDocumento),
                        NombreDocumento = nombreArchivo,
                        File = file
                    });
                }

                if (extensionesValidas)
                {
                    ProveedorDetalleRequestModel request = new ProveedorDetalleRequestModel();
                    var response = business.GetProveedorElemento(request).Proveedor;
                    cuenta.RFC = response.RFC;
                    informacionfinanciera.ProveedorDocumentoList = cuenta.ProveedorDocumentoList;
                    var responseInfo = business.InsertarInformacionFinanciera(informacionfinanciera);
                
                    if (responseInfo.Success)
                    {
                        bool respuestaDoc = business.GuardarDocumentos(cuenta.RFC, cuenta.CatalogoDocumentoList);
                        if (respuestaDoc)
                        {
                            ProveedorAprobarRequestDTO requestAprobador = new ProveedorAprobarRequestDTO { EstatusProveedor = new HistoricoEstatusProveedorDTO { IdEstatusProveedor = 10, IdProveedor = idProveedor, IdUsuario = usuarioInfo.IdUsuario } };
                            var responseAprobar = business.SetProveedorEstatus(requestAprobador);
                            if (responseAprobar.Success)
                            {
                                usuarioInfo.IdEstatus = 5;
                                Session["User"] = usuarioInfo;
                            }
                        }
                    }

                    request.IdProveedor = idProveedor;
                    var aeropuertosAsignados = response.EmpresaList;
                    var proveedorDocumento = business.GetCatalogoDocumentoList();
                    var formatoDocumento = business.GetFormatoArchivoList();
                    if (cuenta == null)
                    {
                        cuenta = new ProveedorInformacionFinanciera();
                        cuenta.ProveedorCuentaList = new List<ProveedorCuentaDTO> { new ProveedorCuentaDTO { Cuenta = null, IdBanco = 0, CLABE = null, IdTipoCuenta = 0, IdProveedor = idProveedor } };
                    }

                    cuenta.RFC = response.RFC;
                    cuenta.CatalogoDocumentoList = proveedorDocumento;

                    cuenta.ProveedorCuentaList = new List<ProveedorCuentaDTO> { new ProveedorCuentaDTO { Cuenta = null, IdBanco = 0, CLABE = null, IdTipoCuenta = 0, IdProveedor = idProveedor } };

                    cuenta.ProveedorCuentaListRegistro = new List<ProveedorCuentaDTO>();

                    return Json(new { success = true, responseText = "ok" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = "error" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = false, responseText = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}