using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EPROCUREMENT.GAPPROVEEDOR.Data
{
    public class ProveedorData
    {
        private readonly TryCatchExecutor tryCatch;

        public ProveedorData()
        {
            tryCatch = new TryCatchExecutor();
        }

        /// <summary>
        /// Registra la información del proveedor
        /// </summary>
        /// <returns>Un objeto de tipo ProveedorResponseDTO</returns>
        public ProveedorResponseDTO ProveedorInsertar(ProveedorRequesteDTO request)
        {
            ProveedorResponseDTO response = new ProveedorResponseDTO()
            {
                ErrorList = new List<ErrorDTO>()
            };

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmdProveedor = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_Proveedor_INS, conexion);

                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        var idProveedor = ExecuteComandProveedor(cmdProveedor, request.Proveedor);
                        if (idProveedor > 0)
                        {
                            foreach (var proveedorGiro in request.Proveedor.ProveedorGiroList.Where(x => x.IdCatalogoGiro != 0).ToList())
                            {
                                var cmdGiro = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_ProveedorGiro_INS, conexion);
                                if (ExecuteComandGiro(cmdGiro, proveedorGiro.IdCatalogoGiro, idProveedor) < 1) { return response; }
                            }

                            foreach (var empresa in request.Proveedor.EmpresaList)
                            {
                                var cmdEmpresa = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_ProveedorEmpresa_INS, conexion);
                                empresa.IdProveedor = idProveedor;
                                if (ExecuteComandEmpresa(cmdEmpresa, empresa) < 1) { return response; }
                            }

                            var cmdContacto = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_ProveedorContacto_INS, conexion);
                            request.Proveedor.Contacto.IdProveedor = idProveedor;
                            if (ExecuteComandContacto(cmdContacto, request.Proveedor.Contacto) < 1) { return response; }

                            var cmdDireccion = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_ProveedorDireccion_INS, conexion);
                            request.Proveedor.Direccion.IdProveedor = idProveedor;
                            if (ExecuteComandDireccion(cmdDireccion, request.Proveedor.Direccion) < 1) { return response; }

                            var cmdEstatus = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_EstatusProveedor_INS, conexion);
                            var estatusProveedor = new HistoricoEstatusProveedorDTO
                            {
                                IdEstatusProveedor = 1,
                                IdProveedor = idProveedor,
                                IdUsuario = null,
                                Observaciones = null
                            };
                            if (ExecuteComandEstatus(cmdEstatus, estatusProveedor) < 1) { return response; }
                            transactionScope.Complete();
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

        public ProveedorCuentaResponseDTO GuardarProveedorCuenta(ProveedorCuentaRequestDTO request)
        {
            ProveedorCuentaResponseDTO response = new ProveedorCuentaResponseDTO()
            {
                ErrorList = new List<ErrorDTO>()
            };

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();

                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        foreach (var proveedorCuenta in request.ProveedorCuentaList)
                        {
                            var cmdCuenta = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_ProveedorCuenta_INS, conexion);
                            cmdCuenta.CommandType = CommandType.StoredProcedure;
                            cmdCuenta.Parameters.Add(new SqlParameter("@Cuenta", proveedorCuenta.Cuenta));
                            cmdCuenta.Parameters.Add(new SqlParameter("@IdBanco", proveedorCuenta.IdBanco));
                            cmdCuenta.Parameters.Add(new SqlParameter("@CLABE", proveedorCuenta.CLABE));
                            cmdCuenta.Parameters.Add(new SqlParameter("@IdTipoCuenta", proveedorCuenta.IdTipoCuenta));
                            cmdCuenta.Parameters.Add(new SqlParameter("@IdProveedor", proveedorCuenta.IdProveedor));
                            cmdCuenta.Parameters.Add(new SqlParameter("Result", SqlDbType.BigInt) { Direction = ParameterDirection.ReturnValue });
                            cmdCuenta.ExecuteNonQuery();
                            var idProveedorCuenta = Convert.ToInt32(cmdCuenta.Parameters["Result"].Value);
                            if (idProveedorCuenta > 0)
                            {
                                foreach (var aeropuerto in proveedorCuenta.AeropuertoList)
                                {

                                    var cmdAeropuerto = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_CuentaEmpresa_INS, conexion);
                                    cmdAeropuerto.CommandType = CommandType.StoredProcedure;
                                    cmdAeropuerto.Parameters.Add(new SqlParameter("@IdProveedorCuenta", idProveedorCuenta));
                                    cmdAeropuerto.Parameters.Add(new SqlParameter("@IdCatalogoAeropuerto", SqlDbType.NVarChar, 50)).Value = aeropuerto.Id;
                                    cmdAeropuerto.Parameters.Add(new SqlParameter("Result", SqlDbType.BigInt) { Direction = ParameterDirection.ReturnValue });
                                    cmdAeropuerto.ExecuteNonQuery();
                                    if (Convert.ToInt32(cmdAeropuerto.Parameters["Result"].Value) < 1)
                                    {
                                        return response;
                                    }
                                }
                            }
                            else { return response; }
                        }

                        transactionScope.Complete();
                        response.Success = true;

                    }
                }
            }
            catch (Exception exception)
            {
            }
            return response;
        }

        public ProveedorDocumentoResponseDTO GuardarProveedorDocumento(ProveedorDocumentoRequestDTO request)
        {
            ProveedorDocumentoResponseDTO response = new ProveedorDocumentoResponseDTO()
            {
                ErrorList = new List<ErrorDTO>()
            };

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();

                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        foreach (var proveedorDocumento in request.ProveedorDocumentoList)
                        {
                            var cmdDocto = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_ProveedorDocumento_INS, conexion);
                            cmdDocto.CommandType = CommandType.StoredProcedure;
                            cmdDocto.Parameters.Add(new SqlParameter("@IdProveedor", proveedorDocumento.IdProveedor));
                            cmdDocto.Parameters.Add(new SqlParameter("@IdCatalogoDocumento", proveedorDocumento.IdCatalogoDocumento));
                            cmdDocto.Parameters.Add(new SqlParameter("@DescripcionDocumento", SqlDbType.NVarChar, 560)).Value = proveedorDocumento.DescripcionDocumento;
                            cmdDocto.Parameters.Add(new SqlParameter("@DocumentoAutorizado", proveedorDocumento.DocumentoAutorizado));
                            cmdDocto.Parameters.Add(new SqlParameter("Result", SqlDbType.BigInt) { Direction = ParameterDirection.ReturnValue });
                            cmdDocto.ExecuteNonQuery();
                            if (Convert.ToInt32(cmdDocto.Parameters["Result"].Value) < 1)
                            {
                                return response;
                            }
                        }

                        transactionScope.Complete();
                        response.Success = true;

                    }
                }
            }
            catch (Exception exception)
            {
            }
            return response;
        }

        /// <summary>
        /// Obtiene un listado de provedores por filtro
        /// </summary>
        /// <param name="request">Un objeto de tipo ProveedorEstatusRequestDTO con los filtros</param>
        /// <returns>Un obejeto de tipo ProveedorEstatusResponseDTO</returns>
        public ProveedorEstatusResponseDTO GetProveedorEstatusList(ProveedorEstatusRequestDTO request)
        {
            ProveedorEstatusResponseDTO response = new ProveedorEstatusResponseDTO()
            {
                ProveedorList = new List<ProveedorEstatusDTO>()
            };

            ProveedorEstatusDTO proveedorEstatus = null;

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmd = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_Proveedor_GETLByFilter, conexion)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add(new SqlParameter("@nombreEmpresa", SqlDbType.NVarChar, 50)).Value = request.ProveedorFiltro.NombreEmpresa;
                    cmd.Parameters.Add(new SqlParameter("@idTipoProveedor", request.ProveedorFiltro.IdTipoProveedor));
                    cmd.Parameters.Add(new SqlParameter("@idGiro", request.ProveedorFiltro.IdGiroProveedor));
                    cmd.Parameters.Add(new SqlParameter("@idAeropuerto", SqlDbType.NVarChar, 100)).Value = request.ProveedorFiltro.IdAeropuerto;
                    cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 50)).Value = request.ProveedorFiltro.Email;
                    cmd.Parameters.Add(new SqlParameter("@rfc", SqlDbType.NVarChar, 50)).Value = request.ProveedorFiltro.RFC;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            proveedorEstatus = new ProveedorEstatusDTO();
                            proveedorEstatus.IdProveedor = Convert.ToInt32(reader["IdProveedor"]);
                            proveedorEstatus.RFC = reader["RFC"].ToString();
                            proveedorEstatus.NombreEmpresa = reader["NombreEmpresa"].ToString();
                            proveedorEstatus.Email = reader["Email"].ToString();
                            proveedorEstatus.Estatus = reader["Estatus"].ToString();
                            proveedorEstatus.IdEstatus = Convert.ToInt32(reader["IdEstatus"]);
                            proveedorEstatus.AXNumeroProveedor = reader["AXNumeroProveedor"].ToString();
                            response.ProveedorList.Add(proveedorEstatus);
                            
                        }
                    }
                }
                response.Success = true;
                return response;
            }
            catch (Exception exception)
            {
                response.Success = false;
            }

            return response;
        }

        /// <summary>
        /// Obtiene el detalle de un proveedor
        /// </summary>
        /// <param name="request">Un objeto de tipo ProveedorEstatusRequestDTO con los filtros</param>
        /// <returns>Un obejeto de tipo ProveedorEstatusResponseDTO</returns>
        public ProveedorDetalleResponseDTO GetProveedorElemento(ProveedorDetalleRequestDTO request)
        {
            ProveedorDetalleResponseDTO response = new ProveedorDetalleResponseDTO()
            {
                Proveedor = null
            };

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmdProveedor = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_Proveedor_GETIById, conexion);
                    response.Proveedor = ExecuteQueryProveedor(cmdProveedor, request.IdProveedor);

                    var cmdDireccion = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_ProveedorDireccion_GETIByIdProveedor, conexion);
                    response.Proveedor.Direccion = ExecuteQueryDireccion(cmdDireccion, request.IdProveedor);

                    var cmdContacto = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_ProveedorContacto_GETIByIdProveedor, conexion);
                    response.Proveedor.Contacto = ExecuteQueryContacto(cmdContacto, request.IdProveedor);
                    
                    var cmdEmpresa = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_ProveedorEmpresa_GETLByIdProveedor, conexion);
                    response.Proveedor.EmpresaList = ExecuteQueryEmpresa(cmdEmpresa, request.IdProveedor);
                    
                    var cmdProveedorGiro = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_ProveedorGiro_GETLByIdProveedor, conexion);
                    response.Proveedor.ProveedorGiroList = ExecuteQueryProveedorGiroList(cmdProveedorGiro, request.IdProveedor);
                }
                response.Success = true;
                return response;
            }
            catch (Exception exception)
            {
                response.Success = false;
            }

            return response;
        }

        /// <summary>
        /// Registra el estatus del proveedor
        /// </summary>
        /// <returns>Un objeto de tipo ProveedorEstatusResponseDTO con la respuesta</returns>
        public ProveedorEstatusResponseDTO EstatusProveedorInsertar(ProveedorAprobarRequestDTO request)
        {
            ProveedorEstatusResponseDTO response = new ProveedorEstatusResponseDTO()
            {
                ErrorList = new List<ErrorDTO>()
            };

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    //var valorValido = 0;
                    //var password = "";
                    //while (valorValido != 0)
                    //{
                    //    password = GenerarPassword(10);
                    //    var cmdPassword = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_EstatusProveedor_INS, conexion);
                    //    cmdPassword.CommandType = CommandType.StoredProcedure;
                    //    cmdPassword.Parameters.Add(new SqlParameter("@NombreContacto", SqlDbType.NVarChar, 300)).Value = contacto.NombreContacto;
                    //    cmdPassword.Parameters.Add(new SqlParameter("Result", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
                    //    cmdPassword.ExecuteNonQuery();
                    //    var resultado = Convert.ToInt32(cmdPassword.Parameters["Result"].Value);
                    //}

                    var cmdEstatus = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_EstatusProveedor_INS, conexion);
                    if (request.EstatusProveedor.IdEstatusProveedor == 4)
                    {
                        var userPassword = GenerarPassword(10);
                        var cmdUsuario = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_UsuarioProveedor_IN, conexion);
                        var idUsuario = ExecuteComandUsuario(cmdUsuario, request.EstatusProveedor.IdProveedor, userPassword);
                        request.EstatusProveedor.IdUsuario = idUsuario;
                        if (idUsuario > 0)
                        {
                            if (ExecuteComandEstatus(cmdEstatus, request.EstatusProveedor) > 0)
                            {

                                new EmailData().EnviarEmailRegistro(request.EstatusProveedor.IdProveedor, idUsuario, request.EstatusProveedor.IdEstatusProveedor, request.EstatusProveedor.Observaciones);
                                response.Success = true;
                            }
                        }
                    }
                    else if (request.EstatusProveedor.IdEstatusProveedor == 2)
                    {
                        if (ExecuteComandEstatus(cmdEstatus, request.EstatusProveedor) > 0)
                        {
                            new EmailData().EnviarEmailRegistro(request.EstatusProveedor.IdProveedor, 0, request.EstatusProveedor.IdEstatusProveedor, request.EstatusProveedor.Observaciones);
                            response.Success = true;
                        }
                    }
                    else if (request.EstatusProveedor.IdEstatusProveedor == 5 || request.EstatusProveedor.IdEstatusProveedor == 8 || request.EstatusProveedor.IdEstatusProveedor == 6)
                    {
                        if (ExecuteComandEstatus(cmdEstatus, request.EstatusProveedor) > 0)
                        {
                            response.Success = true;
                        }
                    }
                    else
                    {
                        response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("Estatus No valido") } };
                    }
                }
            }
            catch (Exception exception)
            {
            }
            return response;
        }

        public ProveedorUsuarioDTO GetProvedorUsuarioItem(int idProveedor, int idUsuario)
        {
            ProveedorUsuarioDTO response = new ProveedorUsuarioDTO();

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmd = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_ProveedorUsuario_GETIById, conexion)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add(new SqlParameter("@idProveedor", idProveedor));
                    cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            response = new ProveedorUsuarioDTO();
                            response.RFC = reader["RFC"].ToString();
                            response.NombreEmpresa = reader["NombreEmpresa"].ToString();
                            response.Email = reader["Email"].ToString();
                            response.Password = reader["Password"].ToString();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
            }

            return response;
        }

        public ProveedorUsuarioDTO GetProvedorUsuarioPorRFC(string email)
        {
            ProveedorUsuarioDTO response = new ProveedorUsuarioDTO();

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmdReset = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_ProveedorUsuario_GETIByNombreUsuario, conexion)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmdReset.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 300)).Value = email;
                    using (SqlDataReader reader = cmdReset.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            response = new ProveedorUsuarioDTO();
                            response.RFC = reader["RFC"].ToString();
                            response.NombreEmpresa = reader["NombreEmpresa"].ToString();
                            response.Email = reader["Email"].ToString();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
            }

            return response;
        }

        /// <summary>
        /// Recupera la información del proveedor
        /// </summary>
        /// <param name="cmdProveedor">Información del comand</param>
        /// <param name="idProveedor">Identificador del proveedor</param>
        /// <returns>La respuesta de la consulta</returns>
        private ProveedorDTO ExecuteQueryProveedor(SqlCommand cmdProveedor, int idProveedor)
        {
            var proveedor = new ProveedorDTO();
            cmdProveedor.CommandType = CommandType.StoredProcedure;
            cmdProveedor.Parameters.Add(new SqlParameter("@idProveedor", idProveedor));
            using (SqlDataReader reader = cmdProveedor.ExecuteReader())
            {
                if (reader.Read())
                {
                    proveedor.NombreEmpresa = reader["NombreEmpresa"].ToString();
                    proveedor.RazonSocial = reader["RazonSocial"].ToString();
                    proveedor.RFC = reader["RFC"].ToString();
                    proveedor.NIF = reader["NIF"].ToString();
                    proveedor.ProvTelefono = reader["ProvTelefono"].ToString();
                    proveedor.ProvFax = reader["ProvFax"].ToString();
                    proveedor.PaginaWeb = reader["PaginaWeb"].ToString();
                    proveedor.IdZonaHoraria = Convert.ToInt32(reader["IdZonaHoraria"]);
                    proveedor.IdTipoProveedor = Convert.ToInt32(reader["IdTipoProveedor"]);
                    proveedor.AXNumeroProveedor = reader["AXNumeroProveedor"].ToString();
                    proveedor.AXFechaRegistro = Convert.ToDateTime(reader["AXFechaRegistro"]);
                    proveedor.IdNacionalidad = Convert.ToInt32(reader["IdNacionalidad"]);
                    proveedor.Mexicana = Convert.ToInt32(reader["TipoEmpresa"]) == 1;
                    proveedor.Extranjera = Convert.ToInt32(reader["TipoEmpresa"]) != 1;
                }
            }
            return proveedor;
        }

        /// <summary>
        /// Recupera la información de la dirección
        /// </summary>
        /// <param name="cmdDireccion">Información del comand</param>
        /// <param name="idProveedor">Identificador del proveedor</param>
        /// <returns>Retorna la dirección del proveedor</returns>
        private ProveedorDireccionDTO ExecuteQueryDireccion(SqlCommand cmdDireccion, int idProveedor)
        {
            var direccion = new ProveedorDireccionDTO();
            cmdDireccion.CommandType = CommandType.StoredProcedure;
            cmdDireccion.Parameters.Add(new SqlParameter("@idProveedor", idProveedor));
            using (SqlDataReader reader = cmdDireccion.ExecuteReader())
            {
                if (reader.Read())
                {
                    direccion.IdProveedorDireccion = Convert.ToInt32(reader["IdProveedorDireccion"]);
                    direccion.CodigoPostal = reader["CodigoPostal"].ToString();
                    direccion.Colonia = reader["Colonia"].ToString();
                    direccion.IdMunicipio = Convert.ToInt32(reader["IdMunicipio"]);
                    direccion.Calle = reader["Calle"].ToString();
                    direccion.IdPais = Convert.ToInt32(reader["IdPais"]);
                    direccion.IdEstado = Convert.ToInt32(reader["IdEstado"]);
                    direccion.Estado = reader["Estado"].ToString();
                    direccion.Municipio = reader["Municipio"].ToString();
                    direccion.DireccionValidada = Convert.ToBoolean(reader["DireccionValidada"]);
                    direccion.IdProveedor = idProveedor;
                }
            }

            return direccion;
        }

        /// <summary>
        /// Recupera la información del contacto
        /// </summary>
        /// <param name="cmdContacto">Información del comand</param>
        /// <param name="idProveedor">Identificador del proveedor</param>
        /// <returns>Retorna el contacto del proveedor</returns>
        private ProveedorContactoDTO ExecuteQueryContacto(SqlCommand cmdContacto, int idProveedor)
        {
            var contacto = new ProveedorContactoDTO();
            cmdContacto.CommandType = CommandType.StoredProcedure;
            cmdContacto.Parameters.Add(new SqlParameter("@idProveedor", idProveedor));
            using (SqlDataReader reader = cmdContacto.ExecuteReader())
            {
                if (reader.Read())
                {
                    contacto.IdContacto = Convert.ToInt32(reader["IdContacto"]);
                    contacto.IdProveedor = idProveedor;
                    contacto.NombreContacto = reader["NombreContacto"].ToString();
                    contacto.Cargo = reader["Cargo"].ToString();
                    contacto.IdNacionalidad = Convert.ToInt32(reader["IdNacionalidad"]);
                    contacto.TelefonoDirecto = reader["TelefonoDirecto"].ToString();
                    contacto.TelefonoMovil = reader["TelefonoMovil"].ToString();
                    contacto.Fax = reader["Fax"].ToString();
                    contacto.Email = reader["Email"].ToString();
                    contacto.IdZonaHoraria = Convert.ToInt32(reader["IdZonaHoraria"]);
                    contacto.IdPais = Convert.ToInt32(reader["IdPais"]);
                    contacto.IdIdioma = Convert.ToInt32(reader["IdIdioma"]);
                }
            }

            return contacto;
        }

        /// <summary>
        /// Recupera la información del aeropuerto
        /// </summary>
        /// <param name="cmdAeropuerto">Información del comand</param>
        /// <param name="idProveedor">Identificador del proveedor</param>
        /// <returns>Retorna los aeropuertos del proveedor</returns>
        private List<ProveedorEmpresaDTO> ExecuteQueryEmpresa(SqlCommand cmdEmpresa, int idProveedor)
        {
            ProveedorEmpresaDTO empresa = null;
            var empresaList = new List<ProveedorEmpresaDTO>();
            cmdEmpresa.CommandType = CommandType.StoredProcedure;
            cmdEmpresa.Parameters.Add(new SqlParameter("@idProveedor", idProveedor));
            using (SqlDataReader reader = cmdEmpresa.ExecuteReader())
            {
                while (reader.Read())
                {
                    empresa = new ProveedorEmpresaDTO();
                    empresa.IdProveedorAeropuerto = Convert.ToInt32(reader["IdProveedorAeropuerto"]);
                    empresa.IdProveedor = idProveedor;
                    empresa.IdCatalogoAeropuerto = reader["IdCatalogoAeropuerto"].ToString();
                    empresaList.Add(empresa);
                }
            }
            return empresaList;
        }

        /// <summary>
        /// Recupera la información de los giros del proveedor
        /// </summary>
        /// <param name="cmdProveedorGiro">Información del comand</param>
        /// <param name="idProveedor">Identificador del proveedor</param>
        /// <returns>Retorna los giros del proveedor</returns>
        private List<ProveedorGiroDTO> ExecuteQueryProveedorGiroList(SqlCommand cmdProveedorGiro, int idProveedor)
        {
            ProveedorGiroDTO proveedorGiro = null;
            var proveedorGiroList = new List<ProveedorGiroDTO>();
            cmdProveedorGiro.CommandType = CommandType.StoredProcedure;
            cmdProveedorGiro.Parameters.Add(new SqlParameter("@idProveedor", idProveedor));
            using (SqlDataReader reader = cmdProveedorGiro.ExecuteReader())
            {
                while (reader.Read())
                {
                    proveedorGiro = new ProveedorGiroDTO();
                    proveedorGiro.IdProveedorGiro = Convert.ToInt32(reader["IdProveedorGiro"]);
                    proveedorGiro.IdProveedor = idProveedor;
                    proveedorGiro.IdCatalogoGiro = Convert.ToInt32(reader["IdCatalogoGiro"]);
                    proveedorGiroList.Add(proveedorGiro);
                }
            }
            return proveedorGiroList;
        }

        /// <summary>
        /// Recupera los parametros para el registro del proveedor
        /// </summary>
        /// <param name="cmdProveedor">Información del comand</param>
        /// <param name="proveedor">Información del proveedor</param>
        /// <returns>La respuesta de la inserción</returns>
        private int ExecuteComandProveedor(SqlCommand cmdProveedor, ProveedorDTO proveedor)
        {
            proveedor.AXFechaRegistro = DateTime.Now;
            cmdProveedor.CommandType = CommandType.StoredProcedure;
            cmdProveedor.Parameters.Add(new SqlParameter("@NombreEmpresa", SqlDbType.NVarChar, 500)).Value = proveedor.NombreEmpresa;
            cmdProveedor.Parameters.Add(new SqlParameter("@RazonSocial", SqlDbType.NVarChar, 500)).Value = proveedor.RazonSocial;
            cmdProveedor.Parameters.Add(new SqlParameter("@RFC", SqlDbType.NVarChar, 40)).Value = proveedor.RFC;
            cmdProveedor.Parameters.Add(new SqlParameter("@NIF", SqlDbType.NVarChar, 30)).Value = proveedor.NIF;
            cmdProveedor.Parameters.Add(new SqlParameter("@ProvTelefono", SqlDbType.NVarChar, 50)).Value = proveedor.ProvTelefono;
            cmdProveedor.Parameters.Add(new SqlParameter("@ProvFax", SqlDbType.NVarChar, 50)).Value = proveedor.ProvFax;
            cmdProveedor.Parameters.Add(new SqlParameter("@PaginaWeb", SqlDbType.NVarChar, 500)).Value = proveedor.PaginaWeb;
            cmdProveedor.Parameters.Add(new SqlParameter("@IdZonaHoraria", proveedor.IdZonaHoraria));
            cmdProveedor.Parameters.Add(new SqlParameter("@IdTipoProveedor", proveedor.IdTipoProveedor));
            cmdProveedor.Parameters.Add(new SqlParameter("@AXNumeroProveedor", SqlDbType.NVarChar, 30)).Value = proveedor.AXNumeroProveedor;
            cmdProveedor.Parameters.Add(new SqlParameter("@AXFechaRegistro", proveedor.AXFechaRegistro));
            cmdProveedor.Parameters.Add(new SqlParameter("@IdNacionalidad", proveedor.IdNacionalidad));
            cmdProveedor.Parameters.Add(new SqlParameter("@TipoEmpresa", proveedor.Mexicana ? 1 : 2 ));
            
            cmdProveedor.Parameters.Add(new SqlParameter("Result", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
            cmdProveedor.ExecuteNonQuery();
            var idProveedor = Convert.ToInt32(cmdProveedor.Parameters["Result"].Value);
            return idProveedor;
        }

        /// <summary>
        /// Recupera los parametros para el registro del contacto
        /// </summary>
        /// <param name="cmdContacto">Información del comand</param>
        /// <param name="contacto">Información del contacto</param>
        /// <returns>La respuesta de la inserción</returns>
        private int ExecuteComandContacto(SqlCommand cmdContacto, ProveedorContactoDTO contacto)
        {
            cmdContacto.CommandType = CommandType.StoredProcedure;
            cmdContacto.Parameters.Add(new SqlParameter("@IdProveedor", contacto.IdProveedor));
            cmdContacto.Parameters.Add(new SqlParameter("@NombreContacto", SqlDbType.NVarChar, 300)).Value = contacto.NombreContacto;
            cmdContacto.Parameters.Add(new SqlParameter("@Cargo", SqlDbType.NVarChar, 250)).Value = contacto.Cargo;
            cmdContacto.Parameters.Add(new SqlParameter("@IdNacionalidad", contacto.IdNacionalidad));
            cmdContacto.Parameters.Add(new SqlParameter("@TelefonoDirecto", SqlDbType.NVarChar, 50)).Value = contacto.TelefonoDirecto;
            cmdContacto.Parameters.Add(new SqlParameter("@TelefonoMovil", SqlDbType.NVarChar, 50)).Value = contacto.TelefonoMovil;
            cmdContacto.Parameters.Add(new SqlParameter("@Fax", SqlDbType.NVarChar, 50)).Value = contacto.Fax;
            cmdContacto.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 250)).Value = contacto.Email;
            cmdContacto.Parameters.Add(new SqlParameter("@IdZonaHoraria", contacto.IdZonaHoraria));
            cmdContacto.Parameters.Add(new SqlParameter("@IdPais", contacto.IdPais));
            cmdContacto.Parameters.Add(new SqlParameter("@IdIdioma", contacto.IdIdioma));
            cmdContacto.Parameters.Add(new SqlParameter("Result", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
            cmdContacto.ExecuteNonQuery();
            var resultado = Convert.ToInt32(cmdContacto.Parameters["Result"].Value);
            return resultado;
        }

        /// <summary>
        /// Recupera los parametros para el registro de la dirección
        /// </summary>
        /// <param name="cmdDireccion">Información del comand</param>
        /// <param name="direccion">Información de la dirección</param>
        /// <returns>La respuesta de la inserción</returns>
        private int ExecuteComandDireccion(SqlCommand cmdDireccion, ProveedorDireccionDTO direccion)
        {
            cmdDireccion.CommandType = CommandType.StoredProcedure;
            cmdDireccion.Parameters.Add(new SqlParameter("@IdProveedor", direccion.IdProveedor));
            cmdDireccion.Parameters.Add(new SqlParameter("@CodigoPostal", SqlDbType.NVarChar, 50)).Value = direccion.CodigoPostal;
            cmdDireccion.Parameters.Add(new SqlParameter("@Colonia", SqlDbType.NVarChar, 150)).Value = direccion.Colonia;
            cmdDireccion.Parameters.Add(new SqlParameter("@IdMunicipio", direccion.IdMunicipio));
            cmdDireccion.Parameters.Add(new SqlParameter("@Calle", SqlDbType.NVarChar, 150)).Value = direccion.Calle;
            cmdDireccion.Parameters.Add(new SqlParameter("@IdPais", direccion.IdPais));
            cmdDireccion.Parameters.Add(new SqlParameter("@Estado", SqlDbType.NVarChar, 150)).Value = direccion.Estado;
            cmdDireccion.Parameters.Add(new SqlParameter("@Municipio", SqlDbType.NVarChar, 250)).Value = direccion.Municipio;
            cmdDireccion.Parameters.Add(new SqlParameter("@DireccionValidada", direccion.DireccionValidada));
            cmdDireccion.Parameters.Add(new SqlParameter("Result", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
            cmdDireccion.ExecuteNonQuery();
            var resultado = Convert.ToInt32(cmdDireccion.Parameters["Result"].Value);
            return resultado;
        }

        /// <summary>
        /// Recupera los parametros para el registro de la empresa
        /// </summary>
        /// <param name="cmdEmpresa">Información del comand</param>
        /// <param name="empresa">Información de la empresa</param>
        /// <returns>La respuesta de la inserción</returns>
        private int ExecuteComandEmpresa(SqlCommand cmdEmpresa, ProveedorEmpresaDTO empresa)
        {
            cmdEmpresa.CommandType = CommandType.StoredProcedure;
            cmdEmpresa.Parameters.Add(new SqlParameter("@IdProveedor", empresa.IdProveedor));
            cmdEmpresa.Parameters.Add(new SqlParameter("@IdCatalogoAeropuerto", SqlDbType.NVarChar, 50)).Value = empresa.IdCatalogoAeropuerto;
            cmdEmpresa.Parameters.Add(new SqlParameter("Result", SqlDbType.BigInt) { Direction = ParameterDirection.ReturnValue });
            cmdEmpresa.ExecuteNonQuery();
            var resultado = Convert.ToInt32(cmdEmpresa.Parameters["Result"].Value);
            return resultado;
        }

        /// <summary>
        /// Recupera los parametros para el registro del giro
        /// </summary>
        /// <param name="cmdGiro">Información del comand</param>
        /// <param name="idAeropuerto">Identificador del aeropuerto</param>
        /// <param name="idProveedor">Identificador del proovedor</param>
        /// <returns>La respuesta de la inserción</returns>
        private int ExecuteComandGiro(SqlCommand cmdGiro, int idGiro, int idProveedor)
        {
            cmdGiro.CommandType = CommandType.StoredProcedure;
            cmdGiro.Parameters.Add(new SqlParameter("@IdProveedor", idProveedor));
            cmdGiro.Parameters.Add(new SqlParameter("@IdCatalogoGiro", SqlDbType.NVarChar, 50)).Value = idGiro;
            cmdGiro.Parameters.Add(new SqlParameter("Result", SqlDbType.BigInt) { Direction = ParameterDirection.ReturnValue });
            cmdGiro.ExecuteNonQuery();
            var resultado = Convert.ToInt32(cmdGiro.Parameters["Result"].Value);
            return resultado;
        }

        private int ExecuteComandDelete(SqlCommand cmdGiro, int idProveedor)
        {
            cmdGiro.CommandType = CommandType.StoredProcedure;
            cmdGiro.Parameters.Add(new SqlParameter("@IdProveedor", idProveedor));
            cmdGiro.Parameters.Add(new SqlParameter("Result", SqlDbType.BigInt) { Direction = ParameterDirection.ReturnValue });
            cmdGiro.ExecuteNonQuery();
            var resultado = Convert.ToInt32(cmdGiro.Parameters["Result"].Value);
            return resultado;
        }

        /// <summary>
        /// Recupera los parametros para el registro del giro
        /// </summary>
        /// <param name="cmdEstatus">Información del comand</param>
        /// <param name="estatusProveedor">Información del estatus</param>
        /// <returns>La respuesta de la inserción</returns>
        private int ExecuteComandEstatus(SqlCommand cmdEstatus, HistoricoEstatusProveedorDTO estatusProveedor)
        {
            cmdEstatus.CommandType = CommandType.StoredProcedure;
            cmdEstatus.Parameters.Add(new SqlParameter("@IdEstatusProveedor", estatusProveedor.IdEstatusProveedor));
            cmdEstatus.Parameters.Add(new SqlParameter("@IdProveedor", estatusProveedor.IdProveedor));
            cmdEstatus.Parameters.Add(new SqlParameter("@Observaciones", SqlDbType.NVarChar, 50)).Value = estatusProveedor.Observaciones;
            cmdEstatus.Parameters.Add(new SqlParameter("@IdUsuario", estatusProveedor.IdUsuario));
            cmdEstatus.Parameters.Add(new SqlParameter("Result", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
            cmdEstatus.ExecuteNonQuery();
            var resultado = Convert.ToInt32(cmdEstatus.Parameters["Result"].Value);
            return resultado;
        }
        private int ExecuteComandUsuario(SqlCommand cmdUsuario, int idProveedor, string password)
        {
            cmdUsuario.CommandType = CommandType.StoredProcedure;
            cmdUsuario.Parameters.Add(new SqlParameter("@IdProveedor", idProveedor));
            cmdUsuario.Parameters.Add(new SqlParameter("@Password", password));
            cmdUsuario.Parameters.Add(new SqlParameter("Result", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
            cmdUsuario.ExecuteNonQuery();
            var resultado = Convert.ToInt32(cmdUsuario.Parameters["Result"].Value);
            return resultado;
        }

        public static string GenerarPassword(int longitud)
        {
            string contrasenia = string.Empty;
            string[] letras = { "_", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "ñ", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
            Random EleccionAleatoria = new Random();

            for (int i = 0; i < longitud; i++)
            {
                int LetraAleatoria = EleccionAleatoria.Next(0, 100);
                int NumeroAleatorio = EleccionAleatoria.Next(0, 9);

                if (LetraAleatoria < letras.Length)
                {
                    contrasenia += letras[LetraAleatoria];
                }
                else
                {
                    contrasenia += NumeroAleatorio.ToString();
                }
            }
            return contrasenia;
        }

        public ProveedorResponseDTO TempProveedorInsertar(ProveedorRequesteDTO request)
        {
            ProveedorResponseDTO response = new ProveedorResponseDTO()
            {
                ErrorList = new List<ErrorDTO>()
            };

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmdProveedor = new SqlCommand("[dbo].[usp_EPROCUREMENT_TmpProveedor_INS]", conexion);

                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        var resultado = ExecuteComandTempProveedor(cmdProveedor, request.Proveedor);
                        if (resultado > 0)
                        {
                            var idProveedor = request.Proveedor.IdProveedor;
                            //var cmdGiroDelete = new SqlCommand("[dbo].[usp_EPROCUREMENT_ProveedorGiro_DEL]", conexion);
                            //if (ExecuteComandDelete(cmdGiroDelete, idProveedor) < 1) { return response; }
                            var cmdEmpresaDelete = new SqlCommand("[dbo].[usp_EPROCUREMENT_ProveedorEmpresa_DEL]", conexion);
                            if (ExecuteComandDelete(cmdEmpresaDelete, idProveedor) < 1) { return response; }

                            //foreach (var proveedorGiro in request.Proveedor.ProveedorGiroList)
                            //{
                            //    var cmdGiro = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_ProveedorGiro_INS, conexion);
                            //    if (ExecuteComandGiro(cmdGiro, proveedorGiro.IdCatalogoGiro, idProveedor) < 1) { return response; }
                            //}

                            foreach (var empresa in request.Proveedor.EmpresaList)
                            {
                                var cmdEmpresa = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_ProveedorEmpresa_INS, conexion);
                                empresa.IdProveedor = idProveedor;
                                if (ExecuteComandEmpresa(cmdEmpresa, empresa) < 1) { return response; }
                            }

                            var cmdContacto = new SqlCommand("[dbo].[usp_EPROCUREMENT_ProveedorContacto_UPD]", conexion);
                            request.Proveedor.Contacto.IdProveedor = idProveedor;
                            if (ExecuteComandContacto(cmdContacto, request.Proveedor.Contacto) < 1) { return response; }

                            var cmdDireccion = new SqlCommand("[dbo].[usp_EPROCUREMENT_ProveedorDireccion_UPD]", conexion);
                            request.Proveedor.Direccion.IdProveedor = idProveedor;
                            if (ExecuteComandDireccion(cmdDireccion, request.Proveedor.Direccion) < 1) { return response; }

                            var cmdEstatus = new SqlCommand(App_GlobalResources.StoredProcedures.usp_EPROCUREMENT_EstatusProveedor_INS, conexion);
                            var estatusProveedor = new HistoricoEstatusProveedorDTO
                            {
                                IdEstatusProveedor = 1,
                                IdProveedor = idProveedor,
                                IdUsuario = null,
                                Observaciones = null
                            };
                            if (ExecuteComandEstatus(cmdEstatus, estatusProveedor) < 1) { return response; }
                            transactionScope.Complete();
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

        private int ExecuteComandTempProveedor(SqlCommand cmdProveedor, ProveedorDTO proveedor)
        {
            proveedor.AXFechaRegistro = DateTime.Now;
            cmdProveedor.CommandType = CommandType.StoredProcedure;
            cmdProveedor.Parameters.Add(new SqlParameter("@IdProveedor", proveedor.IdProveedor));
            cmdProveedor.Parameters.Add(new SqlParameter("@NombreEmpresa", SqlDbType.NVarChar, 500)).Value = proveedor.NombreEmpresa;
            cmdProveedor.Parameters.Add(new SqlParameter("@RazonSocial", SqlDbType.NVarChar, 500)).Value = proveedor.RazonSocial;
            cmdProveedor.Parameters.Add(new SqlParameter("@ProvTelefono", SqlDbType.NVarChar, 50)).Value = proveedor.ProvTelefono;
            cmdProveedor.Parameters.Add(new SqlParameter("@PaginaWeb", SqlDbType.NVarChar, 500)).Value = proveedor.PaginaWeb;
            cmdProveedor.Parameters.Add(new SqlParameter("@IdZonaHoraria", proveedor.IdZonaHoraria));
            cmdProveedor.Parameters.Add(new SqlParameter("Result", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
            cmdProveedor.ExecuteNonQuery();
            var idProveedor = Convert.ToInt32(cmdProveedor.Parameters["Result"].Value);
            return idProveedor;
        }

        public List<ProveedorCuentaDTO> GetProveedorCuentaList(int idProveedor)
        {
            List<ProveedorCuentaDTO> proveedorCuentaList = new List<ProveedorCuentaDTO>();
            ProveedorCuentaDTO proveedorCuenta = null;

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("[dbo].[usp_EPROCUREMENT_ProveedorCuenta_GETLByIdProveedor]", conexion)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add(new SqlParameter("@IdProveedor", idProveedor));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            proveedorCuenta = new ProveedorCuentaDTO();
                            proveedorCuenta.Cuenta = reader["Cuenta"].ToString();
                            proveedorCuenta.CLABE = reader["CLABE"].ToString();
                            proveedorCuenta.NombreBanco = reader["NombreBanco"].ToString();
                            proveedorCuenta.TipoCuenta = reader["Tipo"].ToString();
                            proveedorCuentaList.Add(proveedorCuenta);

                        }
                    }
                }
                return proveedorCuentaList;
            }
            catch (Exception exception)
            {
            }

            return proveedorCuentaList;
        }

        public List<CatalogoDocumentoDTO> GetProveedorDocumentoList(int idProveedor)
        {
            List<CatalogoDocumentoDTO> catalogoDocumentoList = new List<CatalogoDocumentoDTO>();
            CatalogoDocumentoDTO proveedorDocumento = null;

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("[dbo].[usp_EPROCUREMENT_ProveedorDocumento_GETLByIdProveedor]", conexion)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add(new SqlParameter("@IdProveedor", idProveedor));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            proveedorDocumento = new CatalogoDocumentoDTO();
                            proveedorDocumento.NombreDocumento = reader["NombreDocumento"].ToString();
                            proveedorDocumento.RutaDocumento = reader["RutaArchivo"].ToString();
                            catalogoDocumentoList.Add(proveedorDocumento);

                        }
                    }
                }
                return catalogoDocumentoList;
            }
            catch (Exception exception)
            {
            }

            return catalogoDocumentoList;
        }
    }
}
