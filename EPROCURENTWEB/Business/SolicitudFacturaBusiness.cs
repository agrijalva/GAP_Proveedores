﻿using EPROCUREMENT.GAPPROVEEDOR.Entities;
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
    }
}