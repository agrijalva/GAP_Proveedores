﻿using System;
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
            ViewBag.UsuarioInfo = usuarioInfo;
            var idProveedor = 0;
            if (usuarioInfo != null)
            {
                idProveedor = usuarioInfo.IdProveedor;
            }
            Session["IdProveedor"] = idProveedor;
            BusinessLogic business = new BusinessLogic();

            if (usuarioInfo.IdEstatus == 5 || usuarioInfo.IdEstatus == 7)
            {
                ProveedorCuentaRequestDTO proveedorCuentaRequest = new ProveedorCuentaRequestDTO
                {
                    IdProveedor = idProveedor
                };
                var proveedorCuentaResponse = business.GetProveedorCuentaList(proveedorCuentaRequest);
                //ProveedorCuentaRequestDTO proveedorCuentaAeropuertoRequest = new ProveedorCuentaRequestDTO
                //{
                //    ProveedorCuentaList = proveedorCuentaResponse.ProveedorCuentaList
                //};
                //var proveedorCuentaAeropuertoResponse = business.GetProveedorCuentaAeropuertoList(proveedorCuentaAeropuertoRequest);
                ViewBag.ProveedorCuentaList = proveedorCuentaResponse.ProveedorCuentaList;

                ProveedorDocumentoRequestDTO proveedorDocumentoRequest = new ProveedorDocumentoRequestDTO
                {
                    IdProveedor = idProveedor
                };

                var proveedorDocumentoResponse = business.GetProveedorDocumentoList(proveedorDocumentoRequest);
                ViewBag.ProveedorDocumentoList = proveedorDocumentoResponse.ProveedorDocumentoList;
            }

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
            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            // Logica anterior
            BusinessLogic business = new BusinessLogic();
            var proveedorDocumento = business.GetCatalogoDocumentoList();
            int idProveedor = new ValidaSession().RecuperaIdProveedorSession();
            var aeropuertos = business.GetAeropuertosList();
            ViewBag.BancoList = business.GetBancoList();
            ViewBag.TipoCuentaList = business.GetTipoCuentaList();
            List<ProveedorDocumentoDTO> provDoctos = new List<ProveedorDocumentoDTO>();

            string cuentaJson = Request["cuenta"];
            if(cuentaJson.Length > 0)
            {
                var cuenta = (ProveedorInformacionFinanciera)JsonConvert.DeserializeObject(cuentaJson, typeof(ProveedorInformacionFinanciera));

                for(int i = 0; i < cuenta.ProveedorCuentaListRegistro.Count; i++)
                {
                    cuenta.ProveedorCuentaListRegistro[i].IdProveedor = idProveedor;
                }

                int contador = Request.Files.Count;
                cuenta.CatalogoDocumentoList = new List<CatalogoDocumentoDTO>();
                bool extensionesValidas = true;

                if (cuenta.ProveedorDocumentoList != null)
                {
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
                            IdCatalogoDocumento = cuenta.ProveedorDocumentoList[j].IdCatalogoDocumento,
                            IdProveedor = idProveedor,
                            DescripcionDocumento = "NA",
                            DocumentoAutorizado = false,
                            NombreArchivo = nombreArchivo
                        };
                    }

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];
                        string idCatalogoDocumento = Request.Files.GetKey(i);
                        var division = file.FileName.Split('.');
                        var extension = division.Last();
                        string nombreArchivo = idProveedor + "-" + idCatalogoDocumento + '.' + extension;


                        for (int j = 0; j < cuenta.ProveedorDocumentoList.Count; j++)
                        {
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
                }
                else
                {
                    cuenta.ProveedorDocumentoList = new List<ProveedorDocumentoDTO>();
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];
                        string idCatalogoDocumento = Request.Files.GetKey(i);
                        var division = file.FileName.Split('.');
                        var extension = division.Last();

                        foreach(var doc in proveedorDocumento)
                        {
                            if(doc.IdCatalogoDocumento == Convert.ToInt32(idCatalogoDocumento))
                            {
                                if (!doc.Extensiones.Contains(extension))
                                {
                                    extensionesValidas = false;
                                }
                            }
                        }

                        string nombreArchivo = idProveedor + "-" + idCatalogoDocumento + '.' + extension;

                        cuenta.ProveedorDocumentoList.Add(new ProveedorDocumentoDTO
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
                }

                if (extensionesValidas)
                {
                    ProveedorDetalleRequestModel request = new ProveedorDetalleRequestModel();
                    var response = business.GetProveedorElemento(request).Proveedor;
                    cuenta.RFC = response.RFC;

                    ProveedorCuentaRequestDTO requestg = new ProveedorCuentaRequestDTO
                    {
                        IdUsuario = Convert.ToUInt64(usuarioInfo.IdUsuario),
                        ProveedorCuentaList = cuenta.ProveedorCuentaListRegistro
                    };
                    var responseg = business.GuardarProveedorCuenta(requestg);

                    ProveedorDocumentoRequestDTO requestPC = new ProveedorDocumentoRequestDTO
                    {
                        IdUsuario = Convert.ToUInt64(usuarioInfo.IdUsuario),
                        ProveedorDocumentoList = cuenta.ProveedorDocumentoList
                    };

                    var responsePC = business.GuardarProveedorCuenta(requestPC);
                    if (responsePC.Success)
                    {
                        bool respuestaDoc = business.GuardarDocumentos(cuenta.RFC, cuenta.CatalogoDocumentoList);
                        if (respuestaDoc)
                        {
                            ProveedorAprobarRequestDTO requestAprobador = new ProveedorAprobarRequestDTO { EstatusProveedor = new HistoricoEstatusProveedorDTO { IdEstatusProveedor = 5, IdProveedor = idProveedor, IdUsuario = usuarioInfo.IdUsuario } };
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
                    return Json(new { success = false, responseText = "error en extensiones" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = false, responseText = "Sin Cuenta" }, JsonRequestBehavior.AllowGet);
            }
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

    }
}