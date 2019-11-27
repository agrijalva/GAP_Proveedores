using Eprocurement.Compras.Business;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eprocurement.Compras.Models;
using EPROCUREMENT.GAPPROVEEDOR.Entities.Proveedor;
using Eprocurement.Compras.Filters;

namespace Eprocurement.Compras.Controllers
{

    public class HomeController : Controller
    {
        public List<AeropuertoDTO> aeropuertoList;
        public List<ZonaHorariaDTO> zonaHorariaList;
        public List<NacionalidadDTO> nacionalidadList;
        public List<GiroDTO> giroList;
        public List<PaisDTO> paisList;
        public List<IdiomaDTO> idiomaList;
        public List<EstadoDTO> estadoList;
        public List<MunicipioDTO> municipioList;
        public List<TipoProveedorDTO> tipoProveedorList;
        public List<EstatusProveedorDTO> estatusProveedorGetList;

        private void CargarCatalogos()
        {
            BusinessLogic businessLogic = new BusinessLogic();
            aeropuertoList = businessLogic.GetAeropuertosList();
            giroList = businessLogic.GetGirosList();
            tipoProveedorList = businessLogic.GetTipoProveedorList();
            estatusProveedorGetList = businessLogic.GetEstatusProveedorList();
        }

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
            estatusProveedorGetList = businessLogic.GetEstatusProveedorList();
        }

        public ActionResult Index()
        {
            CargarCatalogos();

            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            ViewBag.IdUsuarioRol = usuarioInfo.IdUsuarioRol;
            ViewBag.AeropuertoList = aeropuertoList;
            ViewBag.GiroList = giroList;
            ViewBag.TipoProveedorList = tipoProveedorList;
            ViewBag.EstatusProveedorGetList = estatusProveedorGetList;
            return View();
        }


        public ActionResult AceptarProveedor(int idProvider)
        {
            CargarCatalogosAceptar();
            ProveedorRegistro proveedor;
            ViewBag.GiroList = giroList;
            ViewBag.ZonaHorariaList = zonaHorariaList;
            ViewBag.NacionalidadList = nacionalidadList;
            ViewBag.PaisList = paisList;
            ViewBag.IdiomaList = idiomaList;
            ViewBag.EstadoList = estadoList;
            ViewBag.MunicipioList = municipioList;
            ViewBag.TipoProveedorList = tipoProveedorList;
            ViewBag.idProveedor = idProvider;
            ViewBag.EstatusProveedorGetList = estatusProveedorGetList;
            try
            {
                BusinessLogic businessLogic = new BusinessLogic();
                ProveedorDetalleRequestDTO request = new ProveedorDetalleRequestDTO();
                request.IdProveedor = idProvider;

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
                    RFC = response.RFC     ,
                    Mexicana = response.Mexicana,
                    Extranjera = response.Extranjera
                };
                proveedor.AeropuertoList = proveedor.AeropuertoList.Where(x => x.Checado).ToList();
                ViewBag.EstadoList = estadoList;
                ViewBag.MunicipioList = municipioList;
                ViewBag.idEstado = proveedor.Direccion.IdEstado;
                ViewBag.idMunicipio = proveedor.Direccion.IdMunicipio;
                if (proveedor.Direccion.IdPais == 1)
                {
                    ViewBag.EstadoList = businessLogic.GetEstadoList(proveedor.Direccion.IdPais);

                    if (proveedor.Direccion.IdEstado > 0)
                    {
                        ViewBag.idEstado = proveedor.Direccion.IdEstado;
                        ViewBag.MunicipioList = businessLogic.GetMunicipioList(proveedor.Direccion.IdEstado);
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }

        public JsonResult GetProveedorEstatusList(int? idTipoProveedor, int? idGiroProveedor, string idAeropuerto, string nombreEmpresa, string rfc, string email, int? idEstatus)
        {
            try
            {
                var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
                var proveedorEstatus = new List<ProveedorEstatusDTO>();
                BusinessLogic businessLogic = new BusinessLogic();
                ProveedorEstatusRequestDTO request = new ProveedorEstatusRequestDTO();
                request.ProveedorFiltro = new ProveedorFiltroDTO { IdTipoProveedor = idTipoProveedor, IdGiroProveedor = idGiroProveedor, IdAeropuerto = idAeropuerto, NombreEmpresa = nombreEmpresa, RFC = rfc, Email = email, IdEstatus = idEstatus };
                //string[] estatus = { "1", "2", "3", "4" };
                if (usuarioInfo.IdUsuarioRol == 3)
                {
                    string[] estatus = { "5", "6", "7", "8" };
                    var response = businessLogic.GetProveedorEstatusList(request);
                    proveedorEstatus = (from t in response.ProveedorList
                                            where estatus.Contains(t.IdEstatus.ToString())
                                            select t).ToList();
                } else
                {
                    var response = businessLogic.GetProveedorEstatusList(request);
                    if (idEstatus == null)
                    {
                        string[] estatus = { "1", "2", "3", "4", "5", "6", "7", "8" };
                        proveedorEstatus = (from t in response.ProveedorList
                                            where estatus.Contains(t.IdEstatus.ToString())
                                            select t).ToList();
                    }
                    else
                    {
                        string[] estatus = { idEstatus.ToString() };
                        proveedorEstatus = (from t in response.ProveedorList
                                            where estatus.Contains(t.IdEstatus.ToString())
                                            select t).ToList();
                    }
                    
                    
                }
                   


                //from person in people
                //where names.Contains(person.Firstname)
                //select person;

                //(from aeropuerto in aeropuertos
                // join aeropuertoA in aeropuertosAsignados on aeropuerto.Id equals aeropuertoA.IdCatalogoAeropuerto
                // select new AeropuertoDTO { Id = aeropuerto.Id, Nombre = aeropuerto.Nombre, Checado = false }).ToList();



                return Json(proveedorEstatus, JsonRequestBehavior.AllowGet);


              
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public JsonResult SetProveedorEstatus(int idProveedor, int estatus, string observaciones )
        {
            try
            {
                BusinessLogic businessLogic = new BusinessLogic();
                ProveedorAprobarRequestDTO request = new ProveedorAprobarRequestDTO();
                request.EstatusProveedor = new HistoricoEstatusProveedorDTO { IdProveedor = idProveedor, IdEstatusProveedor = estatus, IdUsuario = 3, Observaciones = observaciones };

                var response = businessLogic.SetProveedorEstatus(request);
                return Json(response.Success, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return null;
            }
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

        public JsonResult GetAeropuertos(int idProvider)
        {
            ProveedorRegistro proveedor;
            try
            {
                BusinessLogic businessLogic = new BusinessLogic();
                ProveedorDetalleRequestDTO request = new ProveedorDetalleRequestDTO();
                request.IdProveedor = idProvider;

                var response = businessLogic.GetProveedorElemento(request).Proveedor;
                var empresaList = response.EmpresaList;
                aeropuertoList = businessLogic.GetAeropuertosList();
                proveedor = new ProveedorRegistro
                {
                    AeropuertoList = aeropuertoList.Select(a => new AeropuertoDTO { Id = a.Id, Nombre = a.Nombre, Checado = empresaList.Where(el => el.IdCatalogoAeropuerto == a.Id).Count() > 0 ? true : false }).ToList(),
                };
                proveedor.AeropuertoList = proveedor.AeropuertoList.Where(x => x.Checado).ToList();
    
            }
            catch (Exception ex)
            {
                return Json(new List<AeropuertoDTO>(), JsonRequestBehavior.AllowGet);
            }
            return Json(proveedor.AeropuertoList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGiros(int idProvider)
        {
            var girosProveedor = new List<GiroDTO>();
            try
            {
                BusinessLogic businessLogic = new BusinessLogic();
                ProveedorDetalleRequestDTO request = new ProveedorDetalleRequestDTO();
                request.IdProveedor = idProvider;
                var giroList = businessLogic.GetGirosList();
                if (giroList != null && giroList.Count > 0)
                {
                    var response = businessLogic.GetProveedorElemento(request).Proveedor;
                    if (response != null && response.ProveedorGiroList != null && response.ProveedorGiroList.Count > 0)
                    {
                        girosProveedor = (from a in giroList
                                         join b in response.ProveedorGiroList on a.IdGiro equals b.IdCatalogoGiro
                                         select  a).ToList();
                    }                    
                }

            }
            catch (Exception ex)
            {
                return Json(new List<GiroDTO>(), JsonRequestBehavior.AllowGet);
            }
            return Json(girosProveedor, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About(int idProvider)
        {
            try
            {
                BusinessLogic businessLogic = new BusinessLogic();
                ProveedorDetalleRequestDTO request = new ProveedorDetalleRequestDTO();
                request.IdProveedor = idProvider;

                var response = businessLogic.GetProveedorElemento(request);
                return View(response.Proveedor);

            }
            catch (Exception ex)
            {

                return View();
            }


            //ViewBag.Message = "Your application description page.";

            //return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AprobarInformacionBF(int idProveedor, int idEstatus)
        {
            ProveedorInformacionFinanciera informacionFinanciera = new ProveedorInformacionFinanciera();
            ViewBag.IdProveedor = idProveedor;
            ViewBag.IdEstatus = idEstatus;
            return View(informacionFinanciera);
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
    }
}