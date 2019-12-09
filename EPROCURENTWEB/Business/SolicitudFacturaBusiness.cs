using EPROCUREMENT.GAPPROVEEDOR.Entities;
using EprocurementWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace EprocurementWeb.Business
{
    public class SolicitudFacturaBusiness
    {
        string urlApi = ConfigurationManager.AppSettings["urlApi"].ToString();
        public SolicitudFacturaResponseModel GetSolicitudFacturaList(SolicitudFacturaRequestModel request)
        {
            SolicitudFacturaResponseModel response = new SolicitudFacturaResponseModel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/SolicitudFactura/");
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("SolicitudFacturaGetList", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    response = JSSerializer.Deserialize<SolicitudFacturaResponseModel>(readTask.Result);                    
                }
            }
            return response;
        }

        public SolicitudFacturaDetalleResponseDTO GetSolicitudFacturaDetalle(SolicitudFacturaDetalleRequestDTO request)
        {
            SolicitudFacturaDetalleResponseDTO response = new SolicitudFacturaDetalleResponseDTO();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/SolicitudFactura/");
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("SolicitudFacturaDetalleGetList", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    response = JSSerializer.Deserialize<SolicitudFacturaDetalleResponseDTO>(readTask.Result);
                }
            }
            return response;
        }

        public FacturaResponseModel GetFacturaList(FacturaRequestModel request)
        {
            
            FacturaResponseModel response = new FacturaResponseModel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/SolicitudFactura/");
                var json = JsonConvert.SerializeObject(request);
                Console.WriteLine(json);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine(content);
                var responseTask = client.PostAsync("FacturaGetList", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    response = JSSerializer.Deserialize<FacturaResponseModel>(readTask.Result);
                }
            }
            return response;
        }

        public bool GuardarDocumentos(List<DocumentoModel> documentoList, string rfc, int idSolicitud)
        {
            bool respuesta = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/SolicitudFactura/");
                using (var content = new MultipartFormDataContent())
                {
                    foreach (var file in documentoList)
                    {
                        byte[] Bytes = new byte[file.File.InputStream.Length + 1];
                        file.File.InputStream.Read(Bytes, 0, Bytes.Length);
                        var fileContent = new ByteArrayContent(Bytes);
                        fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = (rfc + "_" + idSolicitud + "_" + file.NombreDocumento) };
                        content.Add(fileContent);
                    }

                    var result = client.PostAsync("Upload", content).Result;

                    if (result.StatusCode == System.Net.HttpStatusCode.Created) { respuesta = true; }
                    else { respuesta = false; }
                }
            }

            return respuesta;
        }
    }
}