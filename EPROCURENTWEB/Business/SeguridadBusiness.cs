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
    public class SeguridadBusiness
    {
        string urlApi = ConfigurationManager.AppSettings["urlApi"].ToString();
        public bool ResetPasswordUsuario(ActualizaPasswordModel passwordModel, int idUsuario)
        {
            UsuarioDTO usuarioDTO = new UsuarioDTO
            {
                IdUsuario = idUsuario,
                Password = passwordModel.Password
            };
            ActualizaPasswordRequestDTO loginUsuario = new ActualizaPasswordRequestDTO { Usuario = usuarioDTO, NuevaPassword = passwordModel.PasswordNueva };
            bool resultado = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Seguridad/");
                var json = JsonConvert.SerializeObject(loginUsuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("ActualizaPassword", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    var response = JSSerializer.Deserialize<ActualizaPasswordResponseDTO>(readTask.Result);
                    resultado = response.Success;
                }
            }
            return resultado;
        }
    }
}