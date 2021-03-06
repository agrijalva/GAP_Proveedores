﻿using EPROCUREMENT.GAPPROVEEDOR.Business.Proveedor;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using EPROCUREMENT.GAPPROVEEDOR.Entities.Proveedor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Linq;

namespace EPROCUREMENT.GAPPROVEEDOR.Host.Http.Controllers
{
    [RoutePrefix("api/Proveedor")]
    public class ProveedorController : ApiController
    {
        /// <summary>
        /// Operacion para insertar un proveedor
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Insertar")]
        public ProveedorResponseDTO Process([FromBody]ProveedorRequesteDTO request)
        {
            var response = new HandlerProveedor().GuardarProveedor(request);
            return response;
        }

        [HttpPost]
        [Route("GuardarProveedorCuenta")]
        public ProveedorCuentaResponseDTO GuardarProveedorCuenta([FromBody]ProveedorCuentaRequestDTO request)
        {
            var response = new HandlerProveedor().GuardarProveedorCuenta(request);
            return response;
        }
        
        [HttpPost]
        [Route("ProveedorCuentaList")]
        public ProveedorCuentaResponseDTO GetProveedorCuentaList([FromBody]ProveedorCuentaRequestDTO request)
        {
            var proveedorCuenta = new HandlerProveedor().GetProveedorCuentaList(request);
            return proveedorCuenta;
        }

        //[HttpPost]
        //[Route("ProveedorCuentaAeropuertoList")]
        //public ProveedorCuentaResponseDTO GetProveedorCuentaAeropuertoList([FromBody]ProveedorCuentaRequestDTO request)
        //{
        //    var proveedorCuentaAeropuerto = new HandlerProveedor().GetProveedorCuentaAeropuertoList(request);
        //    return proveedorCuentaAeropuerto;
        //}

        [HttpPost]
        [Route("GetProveedorDocumentoList")]
        public ProveedorDocumentoResponseDTO GetProveedorDocumentoList([FromBody]ProveedorDocumentoRequestDTO request)
        {
            var proveedorDocumento = new HandlerProveedor().GetProveedorDocumentoList(request);
            return proveedorDocumento;
        }

        [HttpPost]
        [Route("GuardarProveedorDocumento")]
        public ProveedorDocumentoResponseDTO GuardarProveedorDocumento([FromBody]ProveedorDocumentoRequestDTO request)
        {
            var response = new HandlerProveedor().GuardarProveedorDocumento(request);
            return response;
        }

        // GET: api/ProveedorEstatusList
        [HttpPost]
        [Route("ProveedorEstatusList")]
        public ProveedorEstatusResponseDTO GetProveedorEstatusList([FromBody]ProveedorEstatusRequestDTO request)
        {
            var proveedorEstatus = new HandlerProveedor().GetProveedorEstatusList(request);

            return proveedorEstatus;
        }

        // GET: api/ProveedorElemento
        [HttpPost]
        [Route("ProveedorElemento")]
        public ProveedorDetalleResponseDTO GetProveedorElemento([FromBody]ProveedorDetalleRequestDTO request)
        {
            var proveedorElemento = new HandlerProveedor().GetProveedorElemento(request);

            return proveedorElemento;
        }

        [HttpPost]
        [Route("AprobarProveedor")]
        public ProveedorEstatusResponseDTO EstatusProveedorInsertar([FromBody]ProveedorAprobarRequestDTO request)
        {
            var response = new HandlerProveedor().EstatusProveedorInsertar(request);
            return response;
        }

        [HttpPost]
        [Route("AprobarProveedorPorCompras")]
        public ProveedorEstatusResponseDTO AprobarProveedorPorCompras(int idProveedor = 0)
        {
            ProveedorEstatusResponseDTO response = new ProveedorEstatusResponseDTO
            {
                Success = false,
                ErrorList = new List<ErrorDTO>()
            };
            response.ErrorList.Add(new ErrorDTO { Mensaje = "idProveedor invalido", Codigo = "401" });

            if (idProveedor > 0)
            {
                ProveedorAprobarRequestDTO request = new ProveedorAprobarRequestDTO
                {
                    EstatusProveedor = new HistoricoEstatusProveedorDTO
                    {
                        IdEstatusProveedor = 4,
                        IdProveedor = idProveedor
                    }
                };
                response = new HandlerProveedor().EstatusProveedorInsertar(request);
            }
            
            return response;
        }

        [HttpPost]
        [Route("ProveedorFiltro")]
        public ProveedorFiltroResponseDTO GetProvedorPorFiltro([FromBody]ProveedorFiltroRequestDTO request)
        {
            var response = new HandlerProveedor().GetProvedorPorFiltro(request);
            return response;
        }

        [HttpPost]
        [Route("InsertarTempProveedor")]
        public ProveedorResponseDTO GuardarTempProveedor([FromBody]ProveedorRequesteDTO request)
        {
            var response = new HandlerProveedor().TempProveedorInsertar(request);
            return response;
        }

        [HttpPost]
        [Route("ProveedorInfoFinanciera")]
        public ProveedorInformacionFinanciera GetProveedorInfoFinanciera([FromBody]ProveedorInformacionFinancieraRequestDTO request)
        {
            var response = new HandlerProveedor().GetProveedorInfoFinanciera(request);
            return response;
        }

        [HttpPost]
        [Route("ContactoProveedorList")]
        public ContactoResponseDTO GetContactoProveedorList([FromBody]ContactoRequestDTO request)
        {
            var response = new HandlerProveedor().GetContactoProveedorList(request);
            return response;
        }

        [HttpPost]
        [Route("UpdateContacto")]
        public ContactoResponseDTO UpdateContacto([FromBody]ContactoRequestDTO request)
        {
            var response = new HandlerProveedor().UpdateContacto(request);
            return response;
        }

        [HttpPost]
        [Route("DeleteContacto")]
        public ContactoResponseDTO DeleteContacto([FromBody]ContactoRequestDTO request)
        {
            var response = new HandlerProveedor().DeleteContacto(request);
            return response;
        }

        [HttpGet]
        [Route("Documento")]
        public HttpResponseMessage Documento(string image)
        {
            string ruta = ConfigurationManager.AppSettings["rutaDocuments"];
            string rutaF = HttpContext.Current.Server.MapPath(ruta);
            if (!Directory.Exists(rutaF))
            {
                Directory.CreateDirectory(rutaF);
            }

            string r = rutaF + "\\" + image;

            if (!File.Exists(r))
            {
                r = rutaF + "\\notfound.jpg";
            }

            Byte[] b = File.ReadAllBytes(r);   // You can use your own method over here.         
            MemoryStream ms = new MemoryStream(b);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(ms);
            var division = image.Split('.');
            if(division.Last() == "pdf")
            {
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            }
            else if(division.Last() == "xls" || division.Last() == "xlsx" || division.Last() == "doc" || division.Last() == "docx")
            {
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = image
                };
            }
            else if(division.Last() == "jpg")
            {
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            }
            else if (division.Last() == "jpeg")
            {
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            }
            else
            {
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            }

            return response;
        }
        
        [HttpPost]
        [Route("Upload")]
        public HttpResponseMessage Upload()
        {
            HttpResponseMessage result = null;

            if (Request.Content.IsMimeMultipartContent("form-data"))
            {
                var request = HttpContext.Current.Request;
                bool SubmittedFile = (request.Files.Count != 0);

                if (request.Files.Count > 0)
                {
                    string ruta = ConfigurationManager.AppSettings["rutaDocuments"];
                    string rutaF = HttpContext.Current.Server.MapPath(ruta);
                    if (!Directory.Exists(rutaF))
                    {
                        Directory.CreateDirectory(rutaF);
                    }

                    var docfiles = new List<string>();
                    foreach (string file in request.Files)
                    {
                        var postedFile = request.Files[file];
                        var filePath = Path.Combine(rutaF + "\\" + postedFile.FileName);
                        postedFile.SaveAs(filePath);
                        docfiles.Add(filePath);
                    }
                    result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return result;
        }

        /// <summary>
        /// Registra la información financiera
        /// </summary>
        /// <param name="request">Un objeto de tipo InformacionFinancieraRequestDTO</param>
        /// <returns>Retorna la respuesta true/false y un listado de errores segun aplique</returns>
        [HttpPost]
        [Route("InsertarInformacionFinanciera")]
        public InformacionFinancieraResponseDTO InsertarInformacionFinanciera([FromBody]InformacionFinancieraRequestDTO request)
        {
            var response = new HandlerProveedor().InsertarInformacionFinanciera(request); 

            return response;
        }

    }
}