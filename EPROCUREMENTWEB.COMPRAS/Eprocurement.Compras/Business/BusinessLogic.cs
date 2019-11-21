using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using EPROCUREMENT.GAPPROVEEDOR.Entities.Proveedor;
using Newtonsoft.Json;

namespace Eprocurement.Compras.Business
{
    public class BusinessLogic
    {
        string urlApi = ConfigurationManager.AppSettings["urlApi"].ToString();
        public ProveedorEstatusResponseDTO GetProveedorEstatusList(ProveedorEstatusRequestDTO request)
        {
            ProveedorEstatusResponseDTO response = new ProveedorEstatusResponseDTO();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Proveedor/");
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("ProveedorEstatusList", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    response = JSSerializer.Deserialize<ProveedorEstatusResponseDTO>(readTask.Result);

                }
            }
            return response;
        }

        public ProveedorDetalleResponseDTO GetProveedorElemento(ProveedorDetalleRequestDTO request)
        {
            ProveedorDetalleResponseDTO response = new ProveedorDetalleResponseDTO();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Proveedor/");
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("ProveedorElemento", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    response = JSSerializer.Deserialize<ProveedorDetalleResponseDTO>(readTask.Result);

                }
            }
            return response;
        }

        public ProveedorEstatusResponseDTO SetProveedorEstatus(ProveedorAprobarRequestDTO request)
        {
            ProveedorEstatusResponseDTO response = new ProveedorEstatusResponseDTO();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Proveedor/");
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("AprobarProveedor", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    response = JSSerializer.Deserialize<ProveedorEstatusResponseDTO>(readTask.Result);

                }
            }
            return response;
        }

        public List<PaisDTO> GetPaisesList()
        {
            var lstPaises = new List<PaisDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Catalogo/");
                var responseTask = client.GetAsync("PaisGetList");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();

                    var response = JSserializer.Deserialize<PaisResponseDTO>(readTask.Result);
                    lstPaises = response.PaisList;
                }
            }
            return lstPaises;
        }
        public List<AeropuertoDTO> GetAeropuertosList()
        {
            var lstAeropuertos = new List<AeropuertoDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Catalogo/");
                var responseTask = client.GetAsync("AeropuertoGetList");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();

                    var response = JSserializer.Deserialize<AeropuertoResponseDTO>(readTask.Result);
                    lstAeropuertos = response.AeropuertoList;
                }
            }
            return lstAeropuertos;
        }
        public List<GiroDTO> GetGirosList()
        {
            var lstGiros = new List<GiroDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Catalogo/");
                var responseTask = client.GetAsync("GiroGetList");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();

                    var response = JSserializer.Deserialize<GiroResponseDTO>(readTask.Result);
                    lstGiros = response.GiroList;
                }
            }
            return lstGiros;
        }
        public List<NacionalidadDTO> GetNacionalidadList()
        {
            var lstNacionalidad = new List<NacionalidadDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Catalogo/");
                var responseTask = client.GetAsync("NacionalidadGetList");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();

                    var response = JSserializer.Deserialize<NacionalidadResponseDTO>(readTask.Result);
                    lstNacionalidad = response.NacionalidadList;
                }
            }
            return lstNacionalidad;
        }

        public List<ZonaHorariaDTO> GetZonaHorariaList()
        {
            var lstZonaHoraria = new List<ZonaHorariaDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Catalogo/");
                var responseTask = client.GetAsync("ZonaHorariaGetList");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();

                    var response = JSserializer.Deserialize<ZonaHorariaResponseDTO>(readTask.Result);
                    lstZonaHoraria = response.ZonaHorariaList;
                }
            }
            return lstZonaHoraria;
        }

        public List<IdiomaDTO> GetIdiomaList()
        {
            var lstIdioma = new List<IdiomaDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Catalogo/");
                var responseTask = client.GetAsync("IdiomaGetList");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();

                    var response = JSserializer.Deserialize<IdiomaResponseDTO>(readTask.Result);
                    lstIdioma = response.IdiomaList;
                }
            }
            return lstIdioma;
        }

        public List<EstadoDTO> GetEstadoList(int idPais)
        {
            List<EstadoDTO> lstEstado = new List<EstadoDTO>();
            EstadoRequesteDTO estado = new EstadoRequesteDTO { idPais = idPais };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Catalogo/");
                var json = JsonConvert.SerializeObject(estado);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("EstadoGetList", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    var response = JSSerializer.Deserialize<EstadoResponseDTO>(readTask.Result);
                    lstEstado = response.EstadoList;
                }
            }
            return lstEstado;
        }

        public List<MunicipioDTO> GetMunicipioList(int idEstado)
        {
            List<MunicipioDTO> lstMunicipio = new List<MunicipioDTO>();
            MunicipioRequesteDTO estado = new MunicipioRequesteDTO { idEstado = idEstado };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Catalogo/");
                var json = JsonConvert.SerializeObject(estado);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("MunicipioGetList", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    var response = JSSerializer.Deserialize<MunicipioResponseDTO>(readTask.Result);
                    lstMunicipio = response.MunicipioList;
                }
            }
            return lstMunicipio;
        }

        public List<TipoProveedorDTO> GetTipoProveedorList()
        {
            var lstTipoProveedor = new List<TipoProveedorDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Catalogo/");
                var responseTask = client.GetAsync("TipoProveedorGetList");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();

                    var response = JSserializer.Deserialize<TipoProveedorResponseDTO>(readTask.Result);
                    lstTipoProveedor = response.TipoProveedorList;
                }
            }
            return lstTipoProveedor;
        }
        public UsuarioDTO LoginUsuarioItem(string usuario, string password)
        {
            UsuarioDTO usuarioDTO = null;
            LoginUsuarioRequestDTO loginUsuario = new LoginUsuarioRequestDTO { Usuario = new UsuarioDTO { NombreUsuario = usuario, Password = password } };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/SeguridadAD/");
                var json = JsonConvert.SerializeObject(loginUsuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("Login", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    var response = JSSerializer.Deserialize<LoginUsuarioResponseDTO>(readTask.Result);
                    if (response.Success)
                    {
                        usuarioDTO = response.Usuario;
                    }
                    else
                    {
                        usuarioDTO = null;
                    }
                }
            }
            return usuarioDTO;
        }

        public ProveedorInformacionFinanciera GetProveedorInfoFinanciera(int idProveedor)
        {
            List<EstadoDTO> lstEstado = new List<EstadoDTO>();
            ProveedorInformacionFinancieraRequestDTO estado = new ProveedorInformacionFinancieraRequestDTO { IdProveedor = idProveedor };
            ProveedorInformacionFinanciera proveedor = new ProveedorInformacionFinanciera();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Proveedor/");
                var json = JsonConvert.SerializeObject(estado);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("ProveedorInfoFinanciera", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    var response = JSSerializer.Deserialize<ProveedorInformacionFinanciera>(readTask.Result);
                    proveedor = response;
                }
            }
            return proveedor;
        }
    }
}
