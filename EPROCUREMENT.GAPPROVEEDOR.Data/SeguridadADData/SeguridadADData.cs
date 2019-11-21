using EPROCUREMENT.GAPPROVEEDOR.Entities;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EPROCUREMENT.GAPPROVEEDOR.Data.SeguridadADData
{
    public class SeguridadADData
    {
        private string GapActiveDirectory = "10.255.127.11";
        private string PagWebOficial = "https://www.aeropuertosgap.com.mx/es/";
        private string LoginWinUrl = "http://siap_webint2/gap2/?app=GapSkipaTemp";
        private string SitioWeb = "http://172.16.1.119/GapSkipaTemp/Home/Login?ReturnUrl=%2FGapSkipaTemp%2F";
        //public class AuthenticationResult
        //{
        //    public AuthenticationResult(string errorMessage = null)
        //    {
        //        S ErrorMessage = errorMessage;
        //    }

        //    public String ErrorMessage { get; private set; }
        //    public Boolean IsSuccess => String.IsNullOrEmpty(ErrorMessage);
        //}

        private readonly IAuthenticationManager authenticationManager;

        //public SeguridadADData(IAuthenticationManager authenticationManager)
        //{
        //    this.authenticationManager = authenticationManager;
        //}

        //Flujo de login comun, usado para usuarios que existen en el active director
        public LoginUsuarioResponseDTO SignIn(LoginUsuarioRequestDTO request)
        {
            LoginUsuarioResponseDTO response = new LoginUsuarioResponseDTO();


            string propiedadesUsuario = "";
            string rol = "";
            string val = "";

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, GapActiveDirectory))
            {
               

                if (request.Usuario.NombreUsuario == "Tesoreria1" || request.Usuario.NombreUsuario == "Tesoreria2")
                {
                    using (var conexion = new SqlConnection(Helper.Connection()))
                    {
                        conexion.Open();
                        var cmdlogin = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_Usuario_GETIByLogin, conexion);
                        var cmdUsuario = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_UsuarioInterno_INS, conexion);
                        var idUsuario = ExecuteComandUsuario(cmdUsuario, request.Usuario.NombreUsuario, request.Usuario.Password);

                        if (idUsuario >= 0)
                        {
                            response = ExecuteComandUsuarioLogin(cmdlogin, request.Usuario.NombreUsuario, request.Usuario.Password);

                        }
                    }

                }
                else
                {
                    bool local = HttpContext.Current.Request.IsLocal;
                    bool isValid = pc.ValidateCredentials(request.Usuario.NombreUsuario, request.Usuario.Password);
                    if (isValid)
                    {
                        //---------local = false;
                        if (local)
                        {
                            try
                            {
                                using (var conexion = new SqlConnection(Helper.Connection()))
                                {
                                    conexion.Open();
                                    var cmdlogin = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_Usuario_GETIByLogin, conexion);
                                    var cmdUsuario = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_UsuarioInterno_INS, conexion);
                                    var idUsuario = ExecuteComandUsuario(cmdUsuario, request.Usuario.NombreUsuario, request.Usuario.Password);

                                    if (idUsuario >= 0)
                                    {
                                        response = ExecuteComandUsuarioLogin(cmdlogin, request.Usuario.NombreUsuario, request.Usuario.Password);

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                val = "Acceso Invalido No se pudo crear su usuario 105: " + ex.Message;

                            }

                            return response;

                        }
                        response.Success = false;
                    }

                }

                
                return response;
            }
        }
        private int ExecuteComandUsuario(SqlCommand cmdUsuario, string usuario, string password)
        {
            try
            {
                cmdUsuario.CommandType = CommandType.StoredProcedure;
                cmdUsuario.Parameters.Add(new SqlParameter("@Usuario", usuario));
                cmdUsuario.Parameters.Add(new SqlParameter("@Password", password));
                cmdUsuario.Parameters.Add(new SqlParameter("Result", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
                cmdUsuario.ExecuteNonQuery();
                var resultado = Convert.ToInt32(cmdUsuario.Parameters["Result"].Value);
                return resultado;

            }
            catch (Exception ex)
            {
                return 0;
            }
            
        }

        private LoginUsuarioResponseDTO ExecuteComandUsuarioLogin(SqlCommand cmdLogin, string usuario, string password)
        {
            var response = new LoginUsuarioResponseDTO();

            cmdLogin.CommandType = CommandType.StoredProcedure;
            cmdLogin.Parameters.Add(new SqlParameter("@NombreUsuario", SqlDbType.NVarChar, 50)).Value = usuario;
            cmdLogin.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 50)).Value = password;
            using (SqlDataReader reader = cmdLogin.ExecuteReader())
            {
                if (reader.Read())
                {
                    response.Usuario = new UsuarioDTO();
                    response.Usuario.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                    response.Usuario.NombreUsuario = reader["NombreUsuario"].ToString();

                    response.Usuario.IdUsuarioRol = Convert.ToInt32(reader["IdUsuarioRol"]);
                    response.Success = true;
                }
            }

            return response;
        }

        //private ClaimsIdentity CreateIdentity(string userPrincipal, string rol, string propiedadesUsuario)
        //{
        //    //GestorOWS.Service1Client gestor = new GestorOWS.Service1Client();
        //    //GestorOWS.Usuarios u = gestor.getUsuario(userPrincipal);
        //    //string rol = gestor.getRol((int)u.IDRol);

        //    var identity = new ClaimsIdentity(LoginAuthentication.ApplicationCookie, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

        //    identity.AddClaim(new Claim(ClaimTypes.Name, userPrincipal));
        //    identity.AddClaim(new Claim(ClaimTypes.Role, rol));
        //    identity.AddClaim(new Claim(ClaimTypes.UserData, propiedadesUsuario));
        //    identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userPrincipal));
        //    identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", userPrincipal));

        //    // add your own claims if you need to add more information stored on the cookie

        //    return identity;
        //}
    }
}
