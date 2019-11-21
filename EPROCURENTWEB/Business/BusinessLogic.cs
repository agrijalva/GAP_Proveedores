using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;
using EprocurementWeb.Models;
using System.Configuration;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using EprocurementWeb.Properties;
using System.IO;

namespace EprocurementWeb.Business
{
    public class BusinessLogic
    {
        string urlApi = ConfigurationManager.AppSettings["urlApi"].ToString();
        string urlApiCp = ConfigurationManager.AppSettings["urlApiCP"].ToString();

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

        public ProveedorResponseDTO PostProveedor(ProveedorRegistro proveedor)
        {
            ProveedorResponseDTO response = null;
            ProveedorRequesteDTO proveedorRequest = new ProveedorRequesteDTO { IdUsuario = 0, Proveedor = proveedor };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Proveedor/");
                var json = JsonConvert.SerializeObject(proveedorRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("Insertar", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    response = JSSerializer.Deserialize<ProveedorResponseDTO>(readTask.Result);
                }
            }
            return response;
        }

        public bool GuardarDocumentos(string rfc, List<CatalogoDocumentoDTO> files)
        {
            string ruta = Settings.Default["RutaDocumentos"].ToString();
            string rutaF = HttpContext.Current.Server.MapPath(ruta);
            string rutaP = rutaF + "\\" + rfc;
            bool respuesta = false;

            if (!Directory.Exists(rutaF))
            {
                Directory.CreateDirectory(rutaF);
            }

            if (!Directory.Exists(rutaP))
            {
                Directory.CreateDirectory(rutaP);
            }
            try
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        //var extension = Path.GetExtension(file.File.FileName);
                        var InputFileName = Path.GetFileName(file.File.FileName);
                        InputFileName = file.IdCatalogoDocumento + "_" + InputFileName;
                        var ServerSavePath = Path.Combine(rutaP + "\\" + InputFileName);
                        file.File.SaveAs(ServerSavePath);
                    }
                }
                respuesta = true;
            }
            catch (Exception excep)
            {
                respuesta = false;
            }
            return respuesta;
        }

        public List<CatalogoDocumentoDTO> ObtenerDocumentos(string rfc, List<CatalogoDocumentoDTO> files)
        {
            string ruta = Settings.Default["RutaDocumentos"].ToString();
            string rutaF = HttpContext.Current.Server.MapPath(ruta);
            string rutaP = rutaF + "\\" + rfc;
            List<string> fnDocumentos = new List<string>();
            try
            {
                foreach (string file in Directory.EnumerateFiles(rutaP))
                {
                    string fn = Path.GetFileName(file);
                    string[] idDoc = fn.Split('_');
                    int id = idDoc.Length > 0 ? Convert.ToInt32(idDoc[0]) : 0;
                    foreach(var doc in files)
                    {
                        if (doc.IdCatalogoDocumento == id)
                        {
                            doc.NombreDocumento = fn;
                            doc.RutaDocumento = file;
                        }
                    }
                    //string contents = File.ReadAllText(file);
                }
            }
            catch (Exception excep)
            {
                throw excep;
            }
            return files;
        }

        public List<TipoCuentaDTO> GetTipoCuentaList()
        {
            var lstTipoCuenta = new List<TipoCuentaDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Catalogo/");
                var responseTask = client.GetAsync("TipoCuentaGetList");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();

                    var response = JSserializer.Deserialize<TipoCuentaResponseDTO>(readTask.Result);
                    lstTipoCuenta = response.TipoCuentaList;
                }
            }
            return lstTipoCuenta;
        }

        public List<BancoDTO> GetBancoList()
        {
            var lstBanco = new List<BancoDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Catalogo/");
                var responseTask = client.GetAsync("BancoGetList");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();

                    var response = JSserializer.Deserialize<BancoResponseDTO>(readTask.Result);
                    lstBanco = response.BancoList;
                }
            }
            return lstBanco;
        }

        public List<CatalogoDocumentoDTO> GetCatalogoDocumentoList()
        {
            var lstDocumento = new List<CatalogoDocumentoDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Catalogo/");
                var responseTask = client.GetAsync("CatalogoDocumentoList");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();

                    var response = JSserializer.Deserialize<CatalogoDocumentoResponseDTO>(readTask.Result);
                    lstDocumento = response.CatalogoDocumentoList;
                }
            }
            return lstDocumento;
        }

        public List<FormatoArchivoDTO> GetFormatoArchivoList()
        {
            var lstFormato = new List<FormatoArchivoDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Catalogo/");
                var responseTask = client.GetAsync("FormatoArchivoList");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();

                    var response = JSserializer.Deserialize<FormatoArchivoResponseDTO>(readTask.Result);
                    lstFormato = response.FormatoArchivoList;
                }
            }
            return lstFormato;
        }

        public ProveedorCuentaResponseDTO GuardarProveedorCuenta(ProveedorCuentaRequestDTO proveedorCuenta)
        {
            ProveedorCuentaResponseDTO response = null;
            ProveedorCuentaRequestDTO proveedorRequest = proveedorCuenta;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Proveedor/");
                var json = JsonConvert.SerializeObject(proveedorRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("GuardarProveedorCuenta", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    response = JSSerializer.Deserialize<ProveedorCuentaResponseDTO>(readTask.Result);
                }
            }
            return response;
        }

        public ProveedorDocumentoResponseDTO GuardarProveedorCuenta(ProveedorDocumentoRequestDTO proveedorDocumento)
        {
            ProveedorDocumentoResponseDTO response = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Proveedor/");
                var json = JsonConvert.SerializeObject(proveedorDocumento);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("GuardarProveedorDocumento", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    response = JSSerializer.Deserialize<ProveedorDocumentoResponseDTO>(readTask.Result);
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

        public UsuarioDTO LoginUsuarioItem(string usuario, string password)
        {
            UsuarioDTO usuarioDTO = null;
            LoginUsuarioRequestDTO loginUsuario = new LoginUsuarioRequestDTO { Usuario = new UsuarioDTO { NombreUsuario = usuario, Password = password } };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Seguridad/");
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

        public string RecuperarPasswordUsuario(string email, bool esSolicitud)
        {
            UsuarioDTO usuarioDTO = new UsuarioDTO
            {
                Email = email
            };
            ResetPasswordRequestDTO loginUsuario = new ResetPasswordRequestDTO { Usuario = usuarioDTO, EsSolicitud = esSolicitud };
            string token = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Seguridad/");
                var json = JsonConvert.SerializeObject(loginUsuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("IniciarRecovery", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    var response = JSSerializer.Deserialize<ResetPasswordResponseDTO>(readTask.Result);
                    if (response.Success)
                    {
                        token = response.TokenRecovery;
                    }
                }
            }
            return token;
        }

        public bool RecuperarPasswordUsuario(UsuarioModel usuario, bool esSolicitud)
        {
            UsuarioDTO usuarioDTO = new UsuarioDTO
            {
                Token = usuario.Token,
                Password = usuario.PasswordNueva

            };
            ResetPasswordRequestDTO loginUsuario = new ResetPasswordRequestDTO { Usuario = usuarioDTO, EsSolicitud = esSolicitud };
            bool resultado = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Seguridad/");
                var json = JsonConvert.SerializeObject(loginUsuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("IniciarRecovery", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    var response = JSSerializer.Deserialize<ResetPasswordResponseDTO>(readTask.Result);
                    if (response.Success)
                    {
                        resultado = response.Success;
                    }
                }
            }
            return resultado;
        }

        public CodigoPostalModel RecuperaCodigoPostalInfo(string codigoPostal)
        {
            var codigoPostalItem = new CodigoPostalModel();
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri(urlApi + "codigo_postal/" + codigoPostal);
                ////var json = JsonConvert.SerializeObject(loginUsuario);
                //var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.GetAsync(urlApiCp + "codigo_postal/" + codigoPostal);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    codigoPostalItem = JSSerializer.Deserialize<CodigoPostalModel>(readTask.Result);
                    //if (response.Success)
                    //{
                    //    resultado = response.Success;
                    //}
                }
            }
            return codigoPostalItem;
        }

        public bool PostTempProveedor(ProveedorRegistro proveedor)
        {
            ProveedorResponseDTO response = null;
            ProveedorRequesteDTO proveedorRequest = new ProveedorRequesteDTO { IdUsuario = 0, Proveedor = proveedor };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi + "api/Proveedor/");
                var json = JsonConvert.SerializeObject(proveedorRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("InsertarTempProveedor", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JavaScriptSerializer JSSerializer = new JavaScriptSerializer();
                    response = JSSerializer.Deserialize<ProveedorResponseDTO>(readTask.Result);
                }
            }
            return response.Success;
        }
    }
}