using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EprocurementWeb.Business;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using EPROCUREMENT.GAPPROVEEDOR.Entities.Proveedor;
using System.IO;
using EprocurementWeb.Filters;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using EprocurementWeb.Models;

namespace EprocurementWeb.Controllers
{
    public class AltaProveedorController : Controller
    {
        public ProveedorInformacionFinanciera cuenta = null;
        public List<ProveedorCuentaDTO>  ProveedorCuentaListRegistro = new List<ProveedorCuentaDTO>();
        public ActionResult InformacionBF()
        {
            ViewBag.Respuesta = "";
            ViewBag.MenssageError = EprocurementWeb.GlobalResources.RHome.Message_Error_Required_Generic;
            ViewBag.MenssageErrorAirPort = EprocurementWeb.GlobalResources.RHome.Message_Error_Required_Airport;
            ViewBag.MenssageErrorDocument = EprocurementWeb.GlobalResources.RHome.Message_Error_Required_Document;
            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            var idProveedor = 0;
            if (usuarioInfo != null)
            {
                idProveedor = usuarioInfo.IdProveedor;
            }
            Session["IdProveedor"] = idProveedor;
            BusinessLogic business = new BusinessLogic();
            Session["ProveedorCuentaListRegistro"] = new List<ProveedorCuentaDTO>();
            var aeropuertos = business.GetAeropuertosList();
            ViewBag.BancoList = business.GetBancoList();
            ViewBag.TipoCuentaList = business.GetTipoCuentaList();
            ProveedorDetalleRequestModel request = new ProveedorDetalleRequestModel();
            request.IdProveedor = idProveedor;
            var response = business.GetProveedorElemento(request).Proveedor;
            var aeropuertosAsignados = response.EmpresaList;
            var proveedorDocumento = business.GetCatalogoDocumentoList();
            var formatoDocumento = business.GetFormatoArchivoList();
            if (cuenta == null)
            {
                cuenta = new ProveedorInformacionFinanciera();
                cuenta.ProveedorCuentaList = new List<ProveedorCuentaDTO> { new ProveedorCuentaDTO { Cuenta = null, IdBanco = 0, CLABE = null, IdTipoCuenta = 0, IdProveedor = idProveedor } };
            }
            cuenta.RFC = response.RFC;
            cuenta.ProveedorCuentaList[0].AeropuertoList = (from aeropuerto in aeropuertos
                                                            join aeropuertoA in aeropuertosAsignados on aeropuerto.Id equals aeropuertoA.IdCatalogoAeropuerto
                                                            select new AeropuertoDTO { Id = aeropuerto.Id, Nombre = aeropuerto.Nombre, Checado = false }).ToList();
            cuenta.CatalogoDocumentoList = proveedorDocumento;
            cuenta.ProveedorCuentaListRegistro = new List<ProveedorCuentaDTO>();
            cuenta.CuentaBancaria = new ProveedorCuentaDTO();
            cuenta.CuentaBancaria.AeropuertoList = cuenta.ProveedorCuentaList[0].AeropuertoList;
            Session["ProveedorCuentaListRegistro"] = new List<ProveedorCuentaDTO>();
            return View(cuenta);
        }

        public ActionResult AgregarCuenta(ProveedorInformacionFinanciera cuenta)
        {
            ViewBag.Respuesta = "";
            BusinessLogic business = new BusinessLogic();
            var bancoList = business.GetBancoList();
            var tipoCuentaList = business.GetTipoCuentaList();
            ProveedorCuentaListRegistro = new ValidaSession().RecuperaRegistrosSession();
            if (cuenta.ProveedorCuentaListRegistro == null)
            {
                cuenta.ProveedorCuentaListRegistro = new List<ProveedorCuentaDTO>();
            }
            if (ProveedorCuentaListRegistro == null)
            {
                ProveedorCuentaListRegistro = new List<ProveedorCuentaDTO>();
            }
            if (cuenta.ProveedorCuentaList != null && cuenta.ProveedorCuentaList.Count > 0)
            {
                cuenta.ProveedorCuentaList.First().AeropuertoList = cuenta.ProveedorCuentaList.First().AeropuertoList.Where(x => x.Checado).ToList();
                ProveedorCuentaListRegistro.Add(cuenta.ProveedorCuentaList.First());
            }
            Session["ProveedorCuentaListRegistro"] = ProveedorCuentaListRegistro;
            foreach(var registro in ProveedorCuentaListRegistro)
            {
                registro.NombreBanco = bancoList.FirstOrDefault(x => x.IdBanco == registro.IdBanco).Nombre;
                registro.TipoCuenta = tipoCuentaList.FirstOrDefault(x => x.IdTipoCuenta == registro.IdTipoCuenta).Tipo;
            }
            cuenta.ProveedorCuentaListRegistro = ProveedorCuentaListRegistro;
            var aeropuertos = business.GetAeropuertosList();
            ViewBag.BancoList = bancoList;
            ViewBag.TipoCuentaList = tipoCuentaList;
            ProveedorDetalleRequestModel request = new ProveedorDetalleRequestModel();
            if (cuenta.ProveedorCuentaList != null)
            {
                request.IdProveedor = cuenta.ProveedorCuentaList.First().IdProveedor;
            }
            var response = business.GetProveedorElemento(request).Proveedor;
            var aeropuertosAsignados = response.EmpresaList;
            var proveedorDocumento = business.GetCatalogoDocumentoList();
            var formatoDocumento = business.GetFormatoArchivoList();
            if (cuenta == null)
            {
                cuenta = new ProveedorInformacionFinanciera();                
            }
            cuenta.ProveedorCuentaList = new List<ProveedorCuentaDTO> { new ProveedorCuentaDTO { Cuenta = null, IdBanco = 0, CLABE = null, IdTipoCuenta = 0, IdProveedor = cuenta.ProveedorCuentaList.First().IdProveedor } };
            cuenta.RFC = response.RFC;
            cuenta.ProveedorCuentaList[0].AeropuertoList = (from aeropuerto in aeropuertos
                                                            join aeropuertoA in aeropuertosAsignados on aeropuerto.Id equals aeropuertoA.IdCatalogoAeropuerto
                                                            select new AeropuertoDTO { Id = aeropuerto.Id, Nombre = aeropuerto.Nombre, Checado = false }).ToList();
            foreach(var aeropuerto in cuenta.ProveedorCuentaList[0].AeropuertoList)
            {
                foreach(var reg in ProveedorCuentaListRegistro)
                {
                    if(reg.AeropuertoList.Exists(x => x.Id == aeropuerto.Id && x.Checado))
                    {
                        aeropuerto.Agregado = true;
                        break;
                    }
                }
                aeropuerto.Checado = false;
            }
            
            cuenta.CatalogoDocumentoList = proveedorDocumento;
            //cuenta.ProveedorCuentaListRegistro = new List<ProveedorCuentaDTO>();
            cuenta.CuentaBancaria = new ProveedorCuentaDTO();
            cuenta.CuentaBancaria.AeropuertoList = cuenta.ProveedorCuentaList[0].AeropuertoList;

            return View("InformacionBF", cuenta);
        }

        [HttpPost]
        public ActionResult Upload()
        {
            // Logica anterior
            BusinessLogic business = new BusinessLogic();
            int idProveedor = new ValidaSession().RecuperaIdProveedorSession();
            var aeropuertos = business.GetAeropuertosList();
            ViewBag.BancoList = business.GetBancoList();
            ViewBag.TipoCuentaList = business.GetTipoCuentaList();
            List<ProveedorDocumentoDTO> provDoctos = new List<ProveedorDocumentoDTO>();

            string cuentaJson = Request["cuenta"];
            if(cuentaJson.Length > 0)
            {
                var cuenta = (ProveedorInformacionFinanciera)JsonConvert.DeserializeObject(cuentaJson, typeof(ProveedorInformacionFinanciera));

                if (cuenta.ProveedorCuentaListRegistro.Count > 0)
                {
                    foreach (var registro in cuenta.ProveedorCuentaListRegistro)
                    {
                        registro.IdProveedor = idProveedor;
                        cuenta.ProveedorCuentaList = new List<ProveedorCuentaDTO>();
                        cuenta.ProveedorCuentaList.Add(registro);
                        ProveedorCuentaRequestDTO requestg = new ProveedorCuentaRequestDTO { IdUsuario = 3, ProveedorCuentaList = cuenta.ProveedorCuentaList };
                        var responseg = business.GuardarProveedorCuenta(requestg);
                    }
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

                    provDoctos.Add(new ProveedorDocumentoDTO
                    {
                        IdCatalogoDocumento = Convert.ToInt32(idCatalogoDocumento),
                        IdProveedor = idProveedor,
                        DescripcionDocumento = "NA",
                        DocumentoAutorizado = false,
                        NombreArchivo = nombreArchivo
                    });

                    cuenta.CatalogoDocumentoList.Add(new CatalogoDocumentoDTO
                    {
                        IdCatalogoDocumento = Convert.ToInt32(idCatalogoDocumento),
                        NombreDocumento = nombreArchivo,
                        File = file
                    });
                }

                ProveedorDetalleRequestModel request = new ProveedorDetalleRequestModel();
                var response = business.GetProveedorElemento(request).Proveedor;
                cuenta.RFC = response.RFC;

                ProveedorDocumentoRequestDTO requestPC = new ProveedorDocumentoRequestDTO { IdUsuario = 3, ProveedorDocumentoList = provDoctos };
                var responsePC = business.GuardarProveedorCuenta(requestPC);
                if (responsePC.Success)
                {
                    bool respuestaDoc = business.GuardarDocumentos(cuenta.RFC, cuenta.CatalogoDocumentoList);
                    if (respuestaDoc)
                    {
                        ProveedorAprobarRequestDTO requestAprobador = new ProveedorAprobarRequestDTO { EstatusProveedor = new HistoricoEstatusProveedorDTO { IdEstatusProveedor = 5, IdProveedor = idProveedor, IdUsuario = 3 } };
                        var responseAprobar = business.SetProveedorEstatus(requestAprobador);
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

            }
            return Json(new { success = true, responseText = "ok" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargarCuentas(ProveedorInformacionFinanciera proveedor)
        {
            BusinessLogic business = new BusinessLogic();
            ProveedorCuentaListRegistro = new ValidaSession().RecuperaRegistrosSession();
            int idProveedor = new ValidaSession().RecuperaIdProveedorSession();
          
            var aeropuertos = business.GetAeropuertosList();
            ViewBag.BancoList = business.GetBancoList();
            ViewBag.TipoCuentaList = business.GetTipoCuentaList();
            proveedor.ProveedorCuentaListRegistro = ProveedorCuentaListRegistro;
            if (ProveedorCuentaListRegistro.Count > 0)
            {
                foreach (var registro in ProveedorCuentaListRegistro)
                {
                    proveedor.ProveedorCuentaList = new List<ProveedorCuentaDTO>();
                    proveedor.ProveedorCuentaList.Add(registro);
                    ProveedorCuentaRequestDTO requestg = new ProveedorCuentaRequestDTO { IdUsuario = 3, ProveedorCuentaList = proveedor.ProveedorCuentaList };
                    var responseg = business.GuardarProveedorCuenta(requestg);
                    ViewBag.Respuesta = "Se guardaron las cuentas correctamente";
                }
            }
            //if (response.Success)
            //{
            //    bool respuestaDoc = business.GuardarDocumentos(proveedor.RFC, proveedor.CatalogoDocumentoList);
            //    if (respuestaDoc)
            //    {
            //        ProveedorAprobarRequestDTO requestAprobador = new ProveedorAprobarRequestDTO { EstatusProveedor = new HistoricoEstatusProveedorDTO { IdEstatusProveedor = 5, IdProveedor = proveedor.ProveedorCuentaList[0].IdProveedor, IdUsuario = 3 } };
            //        var responseAprobar = business.SetProveedorEstatus(requestAprobador);
            //        if (responseAprobar.Success)
            //        {
            //            return View(proveedor);
            //        }
            // }
            //}
            ProveedorDetalleRequestModel request = new ProveedorDetalleRequestModel();
            request.IdProveedor = idProveedor;
            var response = business.GetProveedorElemento(request).Proveedor;
            var aeropuertosAsignados = response.EmpresaList;
            var proveedorDocumento = business.GetCatalogoDocumentoList();
            var formatoDocumento = business.GetFormatoArchivoList();
            if (cuenta == null)
            {
                cuenta = new ProveedorInformacionFinanciera();
                cuenta.ProveedorCuentaList = new List<ProveedorCuentaDTO> { new ProveedorCuentaDTO { Cuenta = null, IdBanco = 0, CLABE = null, IdTipoCuenta = 0, IdProveedor = idProveedor } };
            }
            cuenta.RFC = response.RFC;
            cuenta.ProveedorCuentaList[0].AeropuertoList = (from aeropuerto in aeropuertos
                                                            join aeropuertoA in aeropuertosAsignados on aeropuerto.Id equals aeropuertoA.IdCatalogoAeropuerto
                                                            select new AeropuertoDTO { Id = aeropuerto.Id, Nombre = aeropuerto.Nombre, Checado = false }).ToList();
            cuenta.CatalogoDocumentoList = proveedorDocumento;
            //cuenta.ProveedorCuentaListRegistro = new List<ProveedorCuentaDTO>();
            cuenta.CuentaBancaria = new ProveedorCuentaDTO();
            cuenta.CuentaBancaria.AeropuertoList = cuenta.ProveedorCuentaList[0].AeropuertoList;
            cuenta.ProveedorCuentaListRegistro = proveedor.ProveedorCuentaListRegistro;
            //cuenta.ProveedorCuentaList = new List<ProveedorCuentaDTO> { new ProveedorCuentaDTO { Cuenta = null, IdBanco = 0, CLABE = null, IdTipoCuenta = 0, IdProveedor = cuenta.ProveedorCuentaList.First().IdProveedor } };
            Session["ProveedorCuentaListRegistro"] = new List<ProveedorCuentaDTO>();
            return View("InformacionBF", cuenta);
        }


        [HttpPost, ActionName("InformacionBF")]
        public ActionResult CargarArchivo(ProveedorInformacionFinanciera proveedor)
        {
            BusinessLogic business = new BusinessLogic();
            int idProveedor = new ValidaSession().RecuperaIdProveedorSession();
            var aeropuertos = business.GetAeropuertosList();
            ViewBag.BancoList = business.GetBancoList();
            ViewBag.TipoCuentaList = business.GetTipoCuentaList();
            List<ProveedorDocumentoDTO> provDoctos = new List<ProveedorDocumentoDTO>();
            foreach(var item in proveedor.CatalogoDocumentoList)
            {
                provDoctos.Add(new ProveedorDocumentoDTO
                {
                    IdCatalogoDocumento = item.IdCatalogoDocumento,
                    IdProveedor = idProveedor,
                    DescripcionDocumento = "NA",
                    DocumentoAutorizado = false
                });
            }
            ProveedorDocumentoRequestDTO requestPC = new ProveedorDocumentoRequestDTO { IdUsuario = 3, ProveedorDocumentoList = provDoctos };
            var responsePC = business.GuardarProveedorCuenta(requestPC);
            if (responsePC.Success)
            {
                bool respuestaDoc = business.GuardarDocumentos(proveedor.RFC, proveedor.CatalogoDocumentoList);
                if (respuestaDoc)
                {
                    ProveedorAprobarRequestDTO requestAprobador = new ProveedorAprobarRequestDTO { EstatusProveedor = new HistoricoEstatusProveedorDTO { IdEstatusProveedor = 5, IdProveedor = idProveedor, IdUsuario = 3 } };
                    var responseAprobar = business.SetProveedorEstatus(requestAprobador);
                    //if (responseAprobar.Success)
                    //{
                    //    return View(proveedor);
                    //}
                }
            }
            ProveedorDetalleRequestModel request = new ProveedorDetalleRequestModel();
            request.IdProveedor = idProveedor;
            var response = business.GetProveedorElemento(request).Proveedor;
            var aeropuertosAsignados = response.EmpresaList;
            var proveedorDocumento = business.GetCatalogoDocumentoList();
            var formatoDocumento = business.GetFormatoArchivoList();
            if (cuenta == null)
            {
                cuenta = new ProveedorInformacionFinanciera();
                cuenta.ProveedorCuentaList = new List<ProveedorCuentaDTO> { new ProveedorCuentaDTO { Cuenta = null, IdBanco = 0, CLABE = null, IdTipoCuenta = 0, IdProveedor = idProveedor } };
            }
            cuenta.RFC = response.RFC;
            cuenta.ProveedorCuentaList[0].AeropuertoList = (from aeropuerto in aeropuertos
                                                            join aeropuertoA in aeropuertosAsignados on aeropuerto.Id equals aeropuertoA.IdCatalogoAeropuerto
                                                            select new AeropuertoDTO { Id = aeropuerto.Id, Nombre = aeropuerto.Nombre, Checado = false }).ToList();
            cuenta.CatalogoDocumentoList = proveedorDocumento;
            //cuenta.ProveedorCuentaListRegistro = new List<ProveedorCuentaDTO>();
            //cuenta.CuentaBancaria = new ProveedorCuentaDTO();
            //cuenta.ProveedorCuentaList[0].AeropuertoList = cuenta.ProveedorCuentaList[0].AeropuertoList;
            cuenta.ProveedorCuentaListRegistro = new List<ProveedorCuentaDTO>();

            return View(cuenta);
        }

        public ActionResult DocumentacionBF(int idProveedor)
        {
            BusinessLogic business = new BusinessLogic();
            var proveedorDocumento = business.GetCatalogoDocumentoList();
            ProveedorDetalleRequestModel request = new ProveedorDetalleRequestModel();
            request.IdProveedor = idProveedor;
            var response = business.GetProveedorElemento(request).Proveedor;
            ProveedorInformacionFinanciera cuenta = new ProveedorInformacionFinanciera();
            cuenta.RFC = response.RFC;
            cuenta.CatalogoDocumentoList = business.ObtenerDocumentos(cuenta.RFC, proveedorDocumento);
            return View(cuenta);
        }

        public FileResult DescargarArchivo(string ruta, string nombre)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@ruta);
            string fileName = nombre;

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

    }
}