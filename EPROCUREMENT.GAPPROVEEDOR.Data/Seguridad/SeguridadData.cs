using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Data
{
    public class SeguridadData
    {
        public LoginUsuarioResponseDTO LoginUsuario(LoginUsuarioRequestDTO request)
        {
            LoginUsuarioResponseDTO response = new LoginUsuarioResponseDTO();

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmdUsuario = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_Usuario_GETIByLogin, conexion);
                    cmdUsuario.CommandType = CommandType.StoredProcedure;
                    cmdUsuario.Parameters.Add(new SqlParameter("@NombreUsuario", SqlDbType.NVarChar, 50)).Value = request.Usuario.NombreUsuario;
                    cmdUsuario.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 50)).Value = request.Usuario.Password;
                    using (SqlDataReader reader = cmdUsuario.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            response.Usuario = new UsuarioDTO();
                            response.Usuario.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                            response.Usuario.NombreUsuario = reader["NombreUsuario"].ToString();
                            response.Usuario.IdProveedor = Convert.ToInt32(reader["IdProveedor"]);
                            response.Usuario.NombreCompania = reader["NombreCompania"].ToString();
                            response.Usuario.IdUsuarioRol = Convert.ToInt32(reader["IdUsuarioRol"]);
                            response.Success = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
            }
            return response;
        }

        public ResetPasswordResponseDTO RecuperarPasswordUsuario(ResetPasswordRequestDTO request)
        {
            ResetPasswordResponseDTO response = new ResetPasswordResponseDTO();
            response.ErrorList = new List<ErrorDTO>();

            try
            {
                if (request.EsSolicitud)
                {
                    request.Usuario.Token = new UtileriaData().ObtenerTokenSha256();
                    if (!string.IsNullOrEmpty(request.Usuario.Token))
                    {
                        using (var conexion = new SqlConnection(Helper.Connection()))
                        {
                            conexion.Open();
                            var cmdReset = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_RecoveryPassword_UPD, conexion);
                            cmdReset.CommandType = CommandType.StoredProcedure;
                            cmdReset.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 300)).Value = request.Usuario.Email;
                            cmdReset.Parameters.Add(new SqlParameter("@TokenRecovery", SqlDbType.NVarChar, 1000)).Value = request.Usuario.Token;
                            cmdReset.Parameters.Add(new SqlParameter("Result", SqlDbType.BigInt) { Direction = ParameterDirection.ReturnValue });
                            cmdReset.ExecuteNonQuery();
                            if (Convert.ToInt32(cmdReset.Parameters["Result"].Value) > 0)
                            {
                                new EmailData().EnviarEmailRecuperarPassword(request.Usuario);
                                response.TokenRecovery = request.Usuario.Token;
                                response.Success = true;
                            }
                        }
                    }
                }
                else
                {
                    using (var conexion = new SqlConnection(Helper.Connection()))
                    {
                        conexion.Open();
                        var cmdReset = new SqlCommand("[dbo].[usp_EPROCUREMENT_UsuarioProveedor_Password_UPD]", conexion);
                        cmdReset.CommandType = CommandType.StoredProcedure;
                        cmdReset.Parameters.Add(new SqlParameter("@TokenRecovery", SqlDbType.NVarChar, 1000)).Value = request.Usuario.Token;
                        cmdReset.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 1000)).Value = request.Usuario.Password;
                        cmdReset.Parameters.Add(new SqlParameter("Result", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
                        cmdReset.ExecuteNonQuery();
                        if (Convert.ToInt32(cmdReset.Parameters["Result"].Value) > 0)
                        {
                            response.Success = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
            }
            return response;
        }

        public ActualizaPasswordResponseDTO ActualizaPasswordUsuario(ActualizaPasswordRequestDTO request)
        {
            ActualizaPasswordResponseDTO response = new ActualizaPasswordResponseDTO();
            response.ErrorList = new List<ErrorDTO>();

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmdUpdatePassword = new SqlCommand("[dbo].[usp_EPROCUREMENT_UsuarioResetPassword_UPD]", conexion);
                    cmdUpdatePassword.CommandType = CommandType.StoredProcedure;
                    cmdUpdatePassword.Parameters.Add(new SqlParameter("@IdUsuario", request.Usuario.IdUsuario));
                    cmdUpdatePassword.Parameters.Add(new SqlParameter("@PasswordAnterior", SqlDbType.NVarChar, 300)).Value = request.Usuario.Password;
                    cmdUpdatePassword.Parameters.Add(new SqlParameter("@PasswordNueva", SqlDbType.NVarChar, 1000)).Value = request.NuevaPassword;
                    cmdUpdatePassword.Parameters.Add(new SqlParameter("Result", SqlDbType.BigInt) { Direction = ParameterDirection.ReturnValue });
                    cmdUpdatePassword.ExecuteNonQuery();
                    var resultado = Convert.ToInt32(cmdUpdatePassword.Parameters["Result"].Value);
                    if (resultado == 1)
                    {
                        response.Success = true;
                    }
                    else if(resultado == -4)
                    {
                        response.ErrorList.Add(new ErrorDTO { Codigo = "SDRPDP" });
                    }
                    else
                    {
                        response.ErrorList.Add(new ErrorDTO { Codigo = "GE" });
                    }
                }
            }
            catch (Exception exception)
            {
                response.ErrorList.Add(new ErrorDTO { Codigo = "GE" });
            }
            return response;
        }
    }
}
