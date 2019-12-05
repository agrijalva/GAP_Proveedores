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

namespace EprocurementWeb.Controllers
{
    public class GestionController : Controller
    {
        public string nav_factura;
        public string nav_sFactura;
        public string nav_sCotizacion;
        public string nav_sOC;
        // GET: Gestion
        public ActionResult Index()
        {
            return View();
        }


        // GET: SolicitudFacturacion
        public ActionResult SolicitudFacturacion()
        {
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

        // GET: SolicitudCotizacion
        public ActionResult SolicitudCotizacion()
        {
            return View();
        }

        // GET: OrdenCompra
        public ActionResult OrdenCompra()
        {
            return View();
        }
    }
}