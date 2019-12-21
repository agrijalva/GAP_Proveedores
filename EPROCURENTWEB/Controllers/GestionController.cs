using EprocurementWeb.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EprocurementWeb.Models;
using EprocurementWeb.Filters;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using OfficeOpenXml;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Xml.Schema;
using System.Configuration;
using System.Net.Http;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using EprocurementWeb.SatServiceReference;

namespace EprocurementWeb.Controllers
{
    public class GestionController : Controller
    {
        public List<AeropuertoListaDTO> AeropuertoList;
        public string nav_factura;
        public string nav_sFactura;
        public string nav_sCotizacion;
        public string nav_sOC;
        // GET: Gestion
        public ActionResult Index()
        {
            GetAeropuertoList();
            ViewBag.AeropuertoList = AeropuertoList;
            
            //var aa = GetFacturaList(null, null, null, null, null, null, null);
            return View();
        }


        // GET: SolicitudFacturacion
        public ActionResult SolicitudFacturacion()
        {
            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            ViewBag.TipoEmpresa = usuarioInfo.TipoEmpresa;
            List<EstatusSolicitudModel> estatusList = new List<EstatusSolicitudModel>();
            estatusList.Add(new EstatusSolicitudModel { IdEstatus = 1, Descripcion = "Pendiente" });
            estatusList.Add(new EstatusSolicitudModel { IdEstatus = 2, Descripcion = "En Proceso" });
            estatusList.Add(new EstatusSolicitudModel { IdEstatus = 3, Descripcion = "Aceptada" });
            estatusList.Add(new EstatusSolicitudModel { IdEstatus = 4, Descripcion = "Cancelada por AX" });
            estatusList.Add(new EstatusSolicitudModel { IdEstatus = 5, Descripcion = "Pagada" });
            ViewBag.EstatusSolicitudList = estatusList;

            List<SolicitudFactura> numeroSolicitudList = new List<SolicitudFactura>();
            numeroSolicitudList.Add(new SolicitudFactura { IdSolicitudFactura = 4 });
            numeroSolicitudList.Add(new SolicitudFactura { IdSolicitudFactura = 5 });
            numeroSolicitudList.Add(new SolicitudFactura { IdSolicitudFactura = 6 });
            numeroSolicitudList.Add(new SolicitudFactura { IdSolicitudFactura = 7 });
            ViewBag.NumeroSolicitudList = numeroSolicitudList;


            return View();
        }

        // GET: SolicitudFacturacion
        public ActionResult SolicitudDetalle(int idSolicitudFactura)
        {
            ViewBag.IdSolicitudFactura = idSolicitudFactura;
            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            ViewBag.TipoEmpresa = usuarioInfo.TipoEmpresa;
            return View();
        }

        public JsonResult GetSolicitudFacturaList(int? idSolicitudFactura, int? idEstatus, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            SolicitudFacturaBusiness businessLogic = new SolicitudFacturaBusiness();
            var request = new SolicitudFacturaRequestModel
            {
                SolicitudFacturaFiltro = new SolicitudFacturaFiltroModel
                {
                    IdProveedor = usuarioInfo.IdProveedor,
                    IdSolicitudFactura = idSolicitudFactura,
                    IdEstatus = idEstatus,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin
                }
            };

            var solicitudFacturaResponse = businessLogic.GetSolicitudFacturaList(request);
          
            foreach(var solicitud in solicitudFacturaResponse.SolicitudFacturaList)
            {
                solicitud.Fecha = solicitud.FechaSolicitud.ToShortDateString();
                solicitud.RutaPDF = ConfigurationManager.AppSettings["urlApi"] + "api/SolicitudFactura/Documento?image=" + solicitud.RutaPDF;//"TRL891222ST7_4_d7e720c8-bdfd-4597-bbab-e9aa40d8413a.pdf";//documento.NombreArchivo;
                solicitud.RutaXML = usuarioInfo.TipoEmpresa == 1 ? ConfigurationManager.AppSettings["urlApi"] + "api/SolicitudFactura/Documento?image=" + solicitud.RutaXML : "";//"SQP981128289_29_d7e720c8-bdfd-4597-bbab-e9aa40d8413a.xml";//documento.NombreArchivo;
            }
            
            return Json(solicitudFacturaResponse.SolicitudFacturaList, JsonRequestBehavior.AllowGet);           

        }

        public JsonResult GetSolicitudDetalleList(int idSolicitudFactura)
        {
            SolicitudFacturaBusiness businessLogic = new SolicitudFacturaBusiness();
            var request = new SolicitudFacturaDetalleRequestDTO
            {
                IdSolicitudFactura = idSolicitudFactura
            };

            var solicitudDetalleResponse = businessLogic.GetSolicitudFacturaDetalle(request);

            return Json(solicitudDetalleResponse, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ExportToExcel(int idSolicitudFactura)
        {
            try
            {
                SolicitudFacturaBusiness businessLogic = new SolicitudFacturaBusiness();
                var request = new SolicitudFacturaDetalleRequestDTO
                {
                    IdSolicitudFactura = idSolicitudFactura
                };
                var solicitudDetalleResponse = businessLogic.GetSolicitudFacturaDetalle(request);
                if (solicitudDetalleResponse.Success)
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("ID Lineas", typeof(int));
                    dataTable.Columns.Add("Descripción", typeof(string));
                    dataTable.Columns.Add("Cantidad Adquirida", typeof(decimal));
                    dataTable.Columns.Add("Precio Unitario", typeof(decimal));
                    dataTable.Columns.Add("Importe Adquirido", typeof(decimal));
                    dataTable.Columns.Add("Cantidad Facturada", typeof(decimal));
                    dataTable.Columns.Add("Cantidad a Facturar", typeof(decimal));
                    dataTable.Columns.Add("Importe Facturado sin I.V.A.", typeof(decimal));
                    dataTable.Columns.Add("Importe a Facturar sin I.V.A.", typeof(decimal));

                    foreach (var solicitud in solicitudDetalleResponse.SolicitudFacturaDetalleList)
                    {
                        DataRow row = dataTable.NewRow();
                        row[0] = solicitud.Linea;
                        row[1] = solicitud.Descripcion;
                        row[2] = solicitud.CantidadAdquirida;
                        row[3] = solicitud.PrecioUnitario;
                        row[4] = solicitud.ImporteAdquirido;
                        row[5] = solicitud.CantidadFacturada;
                        row[6] = solicitud.CantidadFacturar;
                        row[7] = solicitud.ImporteFacturado;
                        row[8] = solicitud.ImporteFacturar;
                        dataTable.Rows.Add(row);
                    }

                    var memoryStream = new MemoryStream();
                    using (var excelPackage = new ExcelPackage(memoryStream))
                    {
                        var worksheet = excelPackage.Workbook.Worksheets.Add("Solicitud");
                        worksheet.Cells["A1"].LoadFromDataTable(dataTable, true, OfficeOpenXml.Table.TableStyles.None);
                        worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                        //worksheet.DefaultRowHeight = 18;

                        //worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        //worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        //worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.DefaultColWidth = 20;
                        worksheet.Column(2).AutoFit();

                        byte[] data = excelPackage.GetAsByteArray();
                        var date = DateTime.Now;
                        return File(data, "application/octet-stream", "SolicitudDetalle_" + date.ToShortDateString() + ".xlsx");
                    }
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult GetFacturaList(
            string IdAeropuerto,
            string OrdenCompra,
            string Folio,
            DateTime? FechaFacInicio,
            DateTime? FechaFacFin,
            DateTime? FechaPagoInicio,
            DateTime? FechaPagoFin
            )
        {
            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            SolicitudFacturaBusiness businessLogic = new SolicitudFacturaBusiness();
            var request = new FacturaRequestModel
            {
                FacturaFiltro = new FacturaFiltroModel
                {
                    IdProveedor = usuarioInfo.IdProveedor,
                    IdAeropuerto = IdAeropuerto,
                    OrdenCompra = OrdenCompra,
                    Folio = Folio,
                    FechaFacInicio = FechaFacInicio,
                    FechaFacFin = FechaFacFin,
                    FechaPagoInicio = FechaPagoInicio,
                    FechaPagoFin = FechaPagoFin
                }
            };

            var FacturaResponse = businessLogic.GetFacturaList(request);
            foreach (var solicitud in FacturaResponse.FacturaList)
            {
                //solicitud.FechaFactura = solicitud.FechaFactura.ToShortDateString();
                //solicitud.FechaPago = solicitud.FechaPago.ToShortDateString();
            }
            return Json(FacturaResponse.FacturaList, JsonRequestBehavior.AllowGet);

        }

        public void GetAeropuertoList()
        {
            SolicitudFacturaBusiness businessLogic = new SolicitudFacturaBusiness();
            AeropuertoList = businessLogic.GetAeropuertoList();
            //var AeropuertoListaResponse = businessLogic.GetAeropuertoList();
        }

        // GET: SolicitudCotizacion
        public ActionResult SolicitudCotizacion()
        {
            return View();
        }

        // GET: OrdenCompra
        public ActionResult OrdenCompra()
        {
            List<EstatusOrdenCompraModel> estatusList = new List<EstatusOrdenCompraModel>();
            estatusList.Add(new EstatusOrdenCompraModel { IdEstatus = 1, EstatusNombre = "Pendiente" });
            estatusList.Add(new EstatusOrdenCompraModel { IdEstatus = 2, EstatusNombre = "En Proceso" });
            estatusList.Add(new EstatusOrdenCompraModel { IdEstatus = 3, EstatusNombre = "Recibida" });
            ViewBag.EstatusOrdenCompraList = estatusList;

            return View();
        }

        public ActionResult OrdenDetalle(string OrdenCompra)
        {
            ViewBag.OrdenCompra = OrdenCompra;
            return View();
        }

        [HttpPost]
        public JsonResult Upload() //string descripcion, HttpPostedFileBase fichero
        {
            try
            {
                var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();

                var idSolicitudFactura = 0;
                if (!string.IsNullOrEmpty(Request.Form.Get("idSolicitudFactura").ToString()))
                {
                    idSolicitudFactura = Convert.ToInt32(Request.Form.Get("idSolicitudFactura"));
                }
                HttpFileCollectionBase files = Request.Files;
                if (usuarioInfo.TipoEmpresa == 1 && files.Count < 2)
                {
                    return Json(new { success = false, responseText = "Debe agregar archivo xml y pdf" }, JsonRequestBehavior.AllowGet);
                }
                else if (usuarioInfo.TipoEmpresa != 1 && files.Count < 1)
                {
                    return Json(new { success = false, responseText = "Debe agregar archivo pdf" }, JsonRequestBehavior.AllowGet);
                }
                HttpPostedFileBase ficheroXml = null;
                HttpPostedFileBase ficheroPdf = null;
                ficheroPdf = files.Count == 1 ? files[0] : files[1];
                
                if (ficheroPdf.ContentType != "application/pdf")
                {
                    return Json(new { success = false, responseText = "Debe agregar un archivo con extensión PDF" }, JsonRequestBehavior.AllowGet);
                }

                List<DocumentoModel> documentoList = new List<DocumentoModel>();
                var nombreXML = string.Empty;
                if (usuarioInfo.TipoEmpresa == 1)
                {
                    ficheroXml = files[0];
                    if (ficheroXml.ContentType != "text/xml")
                    {
                        return Json(new { success = false, responseText = "Debe agregar un archivo con extensión XML" }, JsonRequestBehavior.AllowGet);
                    }
                    documentoList.Add(new DocumentoModel { IdDetalle = 8, NombreDocumento = Path.GetFileName(ficheroXml.FileName), Extension = "xml", File = ficheroXml });
                    var strNombreXml = Path.GetFileName(ficheroXml.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content"), Path.GetFileName(ficheroXml.FileName));
                    
                    ficheroXml.SaveAs(path);
                    BinaryReader b = new BinaryReader(ficheroXml.InputStream);
                    byte[] binData = b.ReadBytes(ficheroXml.ContentLength);
                    string result = System.Text.Encoding.UTF8.GetString(binData);
                    var document = XDocument.Load(path);
                    var respuestaValidacion = ValidarXml(path, idSolicitudFactura);
                    //if (Directory.Exists(Path.GetDirectoryName(path)))
                    //{
                    //    System.IO.File.Delete(path);
                    //}
                    if (!string.IsNullOrEmpty(respuestaValidacion))
                    {
                        return Json(new { success = false, responseText = respuestaValidacion }, JsonRequestBehavior.AllowGet);
                    }
                    
                    nombreXML = Path.GetFileName(ficheroXml.FileName);
                }

                documentoList.Add(new DocumentoModel { IdDetalle = 8, NombreDocumento = Path.GetFileName(ficheroPdf.FileName), Extension = "pdf", File = ficheroPdf });
                var respuestaArchivo = new SolicitudFacturaBusiness().GuardarDocumentos(documentoList, usuarioInfo.NombreUsuario, idSolicitudFactura);
                
                if (respuestaArchivo)
                {
                    var request = new EstatusSolicitudRequestModel
                    {
                        IdSolicitudFactura = idSolicitudFactura,
                        IdEstatusSolicitud = 2,
                        RutaPDF = usuarioInfo.NombreUsuario + "_" + idSolicitudFactura +  "_" + Path.GetFileName(ficheroPdf.FileName),
                        RutaXML = usuarioInfo.NombreUsuario + "_" + idSolicitudFactura + "_" + nombreXML
                    };
                    var respuestaEstatus = new SolicitudFacturaBusiness().GuardarHistoricoEstatusSolicitud(request);
                    if (respuestaEstatus.Success)
                    {
                        return Json(new { success = true, responseText = "Archivos almacenados correctamente" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, responseText = "Ocurrio un error al guardar los archivos" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = "Ocurrio un error al guardar los archivos" }, JsonRequestBehavior.AllowGet);
                }




            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al procesar la información del documento " + ex.Message);
            }

            return Json(new { success = false, responseText = "Ocurrio un error al procesar los archivos" }, JsonRequestBehavior.AllowGet);
            //using (var content = new MultipartFormDataContent())
            //{
            //    byte[] Bytes = new byte[fichero.InputStream.Length + 1];
            //    fichero.InputStream.Read(Bytes, 0, Bytes.Length);
            //    var fileContent = new ByteArrayContent(Bytes);
            //    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };
            //    content.Add(fileContent);

            //}


            //SolicitudFacturaBusiness businessLogic = new SolicitudFacturaBusiness();
            //var request = new SolicitudFacturaDetalleRequestDTO
            //{
            //    IdSolicitudFactura = 8
            //};
            //var solicitudDetalleResponse = businessLogic.GetSolicitudFacturaDetalle(request);

            //return View();
        }
        
        public string ValidarXml(string path, int idSolicitudFactura)
        {
            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            XDocument xmlInput = null;
            XNamespace df;
            var respuesta = "";
            try
            {
                xmlInput = XDocument.Load(path);
                df = xmlInput.Root.Name.Namespace;
                XNamespace tfd = @"http://www.sat.gob.mx/TimbreFiscalDigital";
                XDocument timbre = XDocument.Parse(xmlInput.Root.Element(xmlInput.Root.Name.Namespace + "Complemento").ToString());

                var rfcProveedor = xmlInput.Root.Element(df + "Emisor").Attribute("rfc") != null ? xmlInput.Root.Element(df + "Emisor").Attribute("rfc").Value : xmlInput.Root.Element(df + "Emisor").Attribute("Rfc").Value;
                //var rfcProveedor = xmlInput.Root.Element(df + "Emisor").Attributes().First().Value;
                var rfcRecpetor = xmlInput.Root.Element(df + "Receptor").Attribute("rfc") != null ? xmlInput.Root.Element(df + "Receptor").Attribute("rfc").Value : xmlInput.Root.Element(df + "Receptor").Attribute("Rfc").Value;
                //var rfcRecpetor = xmlInput.Root.Element(df + "Receptor").Attributes().First().Value;
                var total = xmlInput.Root.Attribute("total") != null ? Convert.ToDecimal(xmlInput.Root.Attribute("total").Value) : Convert.ToDecimal(xmlInput.Root.Attribute("Total").Value);
                //var total = Convert.ToDecimal(xmlInput.Root.Attributes().ElementAt(11).Value);
                var foliFiscal = timbre.Root.Element(tfd + "TimbreFiscalDigital").Attribute("UUID").Value;
                if (!string.IsNullOrEmpty(rfcProveedor) && !string.IsNullOrEmpty(rfcRecpetor) && !string.IsNullOrEmpty(foliFiscal) && total != 0)
                {
                    if (usuarioInfo.NombreUsuario != rfcProveedor)
                    {
                        return "El RFC del emisor es diferente al RFC del proveedor.";
                    }

                    SolicitudFacturaBusiness businessLogic = new SolicitudFacturaBusiness();
                    var request = new SolicitudFacturaDetalleRequestDTO
                    {
                        IdSolicitudFactura = idSolicitudFactura
                    };

                    var solicitudDetalleResponse = businessLogic.GetSolicitudFacturaDetalle(request);
                    var montoTolerancia = Convert.ToDecimal(ConfigurationManager.AppSettings["parametroTolerancia"]);
                    var importeValidar = solicitudDetalleResponse.SolicitudFacturaCabecera.ImporteContratado;
                    if (total != importeValidar && ((total - importeValidar) > montoTolerancia) || ((total - importeValidar) < -montoTolerancia))
                    {
                        return "El total de la factura es diferente al total de los registros.";
                    }
                    respuesta = ValidarCFDISat(rfcProveedor, rfcRecpetor, total, foliFiscal) ? "" : "La información de la factura no es válida ante el SAT.";
                }
                //var serie = xmlInput.Root.Attribute("serie").Value;
                //var folio = xmlInput.Root.Attribute("folio").Value;    
                //foreach (XElement xe in timbre.DescendantNodes())
                //{
                //    if (xe.Name.LocalName == "TimbreFiscalDigital" || xe.Name.LocalName == "Complemento")
                //    {
                //        var noCertificadoSAT = xe.Attribute("noCertificadoSAT").Value;
                //        var folioFiscal = xe.Attribute("UUID").Value;
                //        var fechaCertificacion = Convert.ToDateTime(xe.Attribute("FechaTimbrado").Value);
                //    }
                //}
            }
            catch (Exception ex)
            {
                respuesta = "Se generó un error al validar la factura ante el SAT.";
            }
            return respuesta;
        }

        private bool ValidarCFDISat(string rfcEmisor, string rfcReceptor, decimal total, string folioFidcal)
        {
            var proxy = new ConsultaCFDIServiceClient();//SatCFDIServiceClient();
            var query = $"?re={rfcEmisor}&rr={rfcReceptor}&tt={total}&id={folioFidcal}";
            var response = proxy.Consulta(query);
            var resultado = false;           
            if (response.Estado == "Vigente" && response.CodigoEstatus == "S - Comprobante obtenido satisfactoriamente.")
            {
                resultado = true;
            }

            return resultado;
        }

        private void CheckXmlFile(string sPathXML)
        {
            try
            {
                //string sPathXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalResources.EtiquetaForm.content, "FolioFiscala8cc94dca26e42899149e749e2d1ca1a.xml");

                //crear un objeto el cual tendrá el resultado final, este objeto es el principal
                Comprobante oComprobante;
                //pon la ruta donde tienes tu archivo XML Timbrado
                string path = sPathXML;//@"C:\miXML.xml";

                //creamos un objeto XMLSerializer para deserializar
                XmlSerializer oSerializer = new XmlSerializer(typeof(Comprobante));

                //creamos un flujo el cual recibe nuestro xml
                using (StreamReader reader = new StreamReader(path))
                {
                    //aqui deserializamos
                    oComprobante = (Comprobante)oSerializer.Deserialize(reader);

                    //Deserializamos el complemento timbre fiscal
                    foreach (var oComplemento in oComprobante.Complemento)
                    {
                        foreach (var oComplementoInterior in oComplemento.Any)
                        {
                            //si el complemento es TimbreFiscalDigital lo deserializamos
                            if (oComplementoInterior.Name.Contains("TimbreFiscalDigital"))
                            {

                                //Objeto para aplicar ahora la deserialización del complemento timbre
                                XmlSerializer oSerializerComplemento = new XmlSerializer(typeof(TimbreFiscalDigital));
                                //creamos otro flujo para el complemento
                                using (var readerComplemento = new StringReader(oComplementoInterior.OuterXml))
                                {
                                    //y por ultimo deserializamos el complemento
                                    oComprobante.TimbreFiscalDigital =
                                        (TimbreFiscalDigital)oSerializerComplemento.Deserialize(readerComplemento);
                                }

                            }
                        }
                    }
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(sPathXML);
                var xmlText = Encoding.UTF8.GetString(fileBytes);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlText);
                string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);


                var byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                if (xmlText.StartsWith(byteOrderMarkUtf8))
                {
                    xmlText = xmlText.Remove(0, byteOrderMarkUtf8.Length);
                }

                if (!xmlText.StartsWith("<"))
                {
                    xmlText = "<" + xmlText;
                }

                var xDocument = XDocument.Parse(xmlText);
                var xElement = xDocument.Root;
                var satSchema = GetSatSchema();
                List<string> errors = new List<string>();
                List<object> er = new List<object>();
                xDocument.Validate(satSchema, (o, e) =>
                {
                    er.Add(o);
                    errors.Add(e.Message);
                });

            }
            catch (Exception exception)
            {
                throw (exception);
            }
        }

        private XmlSchemaSet GetSatSchema()
        {
            XmlSchemaSet schema = null;
            var urlCfdiSchema = ConfigurationManager.AppSettings["UrlCfdiSchema"];//  Helpers.Configuration.GetAppString("UrlCfdiSchema");
            var urlTimbreSchema = ConfigurationManager.AppSettings["UrlTimbreFiscalSchema"]; //Helpers.Configuration.GetAppString("UrlTimbreFiscalSchema");
            var cfdiNamespace = ConfigurationManager.AppSettings["CfdiTargetNamespace"];//Helpers.Configuration.GetAppString("CfdiTargetNamespace");
            var timbreNamespace = ConfigurationManager.AppSettings["TimbreFiscalNamespace"];//Helpers.Configuration.GetAppString("TimbreFiscalNamespace");

            var client = new HttpClient();
            try
            {
                schema = new XmlSchemaSet();
                schema.Add(cfdiNamespace, XmlReader.Create(urlCfdiSchema));
                schema.Add(timbreNamespace, XmlReader.Create(urlTimbreSchema));
            }
            catch (Exception exception)
            {
                throw (exception);
            }

            return schema;
        }

        public ActionResult DetalleFactura(int idFactura)
        {
            ViewBag.idFactura = idFactura;
            return View();
        }

        public JsonResult GetFacturaDetalle(int idFactura)
        {
            SolicitudFacturaBusiness businessLogic = new SolicitudFacturaBusiness();
            var request = new FacturaDetalleRequestDTO
            {
                IdFactura = idFactura
            };

            var solicitudDetalleResponse = businessLogic.GetFacturaDetalle(request);

            return Json(solicitudDetalleResponse, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetOrdenCompraList(string OrdenCompra, int? idEstatus)
        {
            var usuarioInfo = new ValidaSession().ObtenerUsuarioSession();
            OrdenCompraBusiness businessLogic = new OrdenCompraBusiness();
            var request = new OrdenCompraRequestModel
            {
                OrdenCompraFiltro = new OrdenCompraFiltroModel
                {
                    IdProveedor = usuarioInfo.IdProveedor,
                    OrdenCompra = OrdenCompra,
                    IdEstatus = idEstatus
                }
            };

            var ordenCompraResponse = businessLogic.GetOrdenCompraList(request);
            return Json(ordenCompraResponse.OrdenCompraList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetOrdenCompraDetalleList(string OrdenCompra)
        {
            OrdenCompraBusiness businessLogic = new OrdenCompraBusiness();
            var request = new OrdenCompraDetalleRequestModel
            {
                OrdenCompraDetalleFiltro = new OrdenCompraDetalleFiltroModel
                {
                    OrdenCompra = OrdenCompra
                }
            };

            var ordenCompraResponse = businessLogic.GetOrdenCompraDetalleList(request);
            return Json(ordenCompraResponse.OrdenCompraDetalleList, JsonRequestBehavior.AllowGet);

        }
    }
}