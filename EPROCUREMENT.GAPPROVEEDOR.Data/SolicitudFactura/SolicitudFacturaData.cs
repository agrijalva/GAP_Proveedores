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
    public class SolicitudFacturaData 
    {
        public SolicitudFacturaResponseDTO GetSolicitudFacturaList(SolicitudFacturaRequestDTO request)
        {
            var response = new SolicitudFacturaResponseDTO()
            {
                SolicitudFacturaList = new List<SolicitudFacturaDTO>()
            };

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmdContacto = new SqlCommand("[dbo].[usp_EPROCUREMENT_SolicitudFactura_GETL]", conexion);
                    SolicitudFacturaDTO solicitudFactura = null;
                    cmdContacto.CommandType = CommandType.StoredProcedure;
                    cmdContacto.Parameters.Add(new SqlParameter("@IdProveedor", request.SolicitudFacturaFiltro.IdProveedor));
                    cmdContacto.Parameters.Add(new SqlParameter("@IdSolicitudFactura", request.SolicitudFacturaFiltro.IdSolicitudFactura));
                    cmdContacto.Parameters.Add(new SqlParameter("@IdEstatus", request.SolicitudFacturaFiltro.IdEstatus));
                    cmdContacto.Parameters.Add(new SqlParameter("@FechaInicio", request.SolicitudFacturaFiltro.FechaInicio));
                    cmdContacto.Parameters.Add(new SqlParameter("@FechaFin", request.SolicitudFacturaFiltro.FechaFin));
                    using (SqlDataReader reader = cmdContacto.ExecuteReader()) 
                    {
                        while (reader.Read())
                        {
                            solicitudFactura = new SolicitudFacturaDTO();
                            solicitudFactura.IdSolicitudFactura = Convert.ToInt32(reader["IdSolicitudFactura"]);
                            solicitudFactura.NumeroRecepcion = reader["NumeroRecepcion"].ToString();
                            solicitudFactura.FechaSolicitud = Convert.ToDateTime(reader["FechaSolicitud"].ToString());
                            solicitudFactura.Monto = Convert.ToDecimal(reader["Monto"]);
                            solicitudFactura.RutaPDF = reader["RutaPDF"].ToString();
                            solicitudFactura.RutaXML = reader["RutaXML"].ToString();
                            solicitudFactura.UltimoEstatus = Convert.ToInt32(reader["UltimoEstatus"]);
                            solicitudFactura.UltimoStatusLabel = reader["UltimoStatusLabel"].ToString();
                            solicitudFactura.IdEstatusSolicitud = Convert.ToInt32(reader["UltimoEstatus"]);
                            response.SolicitudFacturaList.Add(solicitudFactura);
                        }
                    }
                }
                response.Success = true;
            }
            catch (Exception exception)
            {
                response.Success = false;
            }

            return response;
        }

        public SolicitudFacturaDetalleResponseDTO GetSolicitudFacturaCabecero(SolicitudFacturaDetalleRequestDTO request)
        {
            var response = new SolicitudFacturaDetalleResponseDTO()
            {
                SolicitudFacturaCabecera = new SolicitudFacturaCabeceraDTO(),
                SolicitudFacturaDetalleList = new List<SolicitudFacturaDetalleDTO>()
            };

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    SolicitudFacturaCabeceraDTO solicitudFacturaCabecero = null;
                    SolicitudFacturaDetalleDTO solicitudFacturaDetalle = null;
                    conexion.Open();
                    var cmdCabecero = new SqlCommand("[dbo].[usp_EPROCUREMENT_SolicitudFacturaDetalleCabecera_GETL]", conexion);
                    cmdCabecero.CommandType = CommandType.StoredProcedure;
                    cmdCabecero.Parameters.Add(new SqlParameter("@IdSolicitudFactura", request.IdSolicitudFactura));
                    using (SqlDataReader reader = cmdCabecero.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            solicitudFacturaCabecero = new SolicitudFacturaCabeceraDTO();
                            solicitudFacturaCabecero.Descripcion = reader["Descripcion"].ToString();
                            solicitudFacturaCabecero.ImporteContratado = Convert.ToDecimal(reader["ImporteContratado"]);
                            solicitudFacturaCabecero.ImporteAdquirido = Convert.ToDecimal(reader["ImporteAdquirido"].ToString());
                            solicitudFacturaCabecero.ImporteFacturado = Convert.ToDecimal(reader["ImporteFacturado"]);
                            solicitudFacturaCabecero.ImporteFacturar = Convert.ToDecimal(reader["ImporteFacturar"]);
                            solicitudFacturaCabecero.IdEstatusSolicitud = Convert.ToInt32(reader["IdEstatusSolicitud"]);
                            
                            response.SolicitudFacturaCabecera = solicitudFacturaCabecero;
                        }
                    }

                    var cmdDetalle = new SqlCommand("[dbo].[usp_EPROCUREMENT_SolicitudFacturaDetalle_GETL]", conexion);
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.Parameters.Add(new SqlParameter("@IdSolicitudFactura", request.IdSolicitudFactura));
                    using (SqlDataReader reader = cmdDetalle.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            solicitudFacturaDetalle = new SolicitudFacturaDetalleDTO();
                            solicitudFacturaDetalle.Linea = Convert.ToInt32(reader["Linea"]);
                            solicitudFacturaDetalle.Descripcion = reader["Descripcion"].ToString();
                            solicitudFacturaDetalle.CantidadAdquirida = Convert.ToDecimal(reader["CantidadAdquirida"]);
                            solicitudFacturaDetalle.PrecioUnitario = Convert.ToDecimal(reader["PrecioUnitario"]);
                            solicitudFacturaDetalle.ImporteAdquirido = Convert.ToDecimal(reader["ImporteAdquirido"].ToString());
                            solicitudFacturaDetalle.CantidadFacturada = Convert.ToDecimal(reader["CantidadFacturada"]);
                            solicitudFacturaDetalle.CantidadFacturar = Convert.ToDecimal(reader["CantidadFacturar"]);
                            solicitudFacturaDetalle.ImporteFacturado = Convert.ToDecimal(reader["ImporteFacturado"]);
                            solicitudFacturaDetalle.ImporteFacturar = Convert.ToDecimal(reader["ImporteFacturar"]);
                            response.SolicitudFacturaDetalleList.Add(solicitudFacturaDetalle);
                        }
                    }
                    response.Success = true;
                }
            }
            catch (Exception exception)
            {
            }

            return response;
        }

        public FacturaResponseDTO GetFacturaList(FacturaRequestDTO request)
        {
            var response = new FacturaResponseDTO()
            {
                FacturaList = new List<FacturaDTO>()
            };

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmdContacto = new SqlCommand("[dbo].[usp_EPROCUREMENT_ProveedorFactura_GETL]", conexion);
                    FacturaDTO Factura = null;
                    cmdContacto.CommandType = CommandType.StoredProcedure;
                    cmdContacto.Parameters.Add(new SqlParameter("@IdProveedor", request.FacturaFiltro.IdProveedor));
                    cmdContacto.Parameters.Add(new SqlParameter("@IdAeropuerto", request.FacturaFiltro.IdAeropuerto));
                    cmdContacto.Parameters.Add(new SqlParameter("@OrdenCompra", request.FacturaFiltro.OrdenCompra));
                    cmdContacto.Parameters.Add(new SqlParameter("@Folio", request.FacturaFiltro.Folio));
                    cmdContacto.Parameters.Add(new SqlParameter("@FechaFacturaIni", request.FacturaFiltro.FechaFacInicio));
                    cmdContacto.Parameters.Add(new SqlParameter("@FechaFacturaFin", request.FacturaFiltro.FechaFacFin));
                    cmdContacto.Parameters.Add(new SqlParameter("@FechaPagoIni", request.FacturaFiltro.FechaPagoInicio));
                    cmdContacto.Parameters.Add(new SqlParameter("@FechaPagoFin", request.FacturaFiltro.FechaPagoFin));

                    //SqlDataReader reader = cmdContacto.ExecuteReader();

                    using (SqlDataReader reader = cmdContacto.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Factura = new FacturaDTO();
                            Factura.IdFactura = Convert.ToInt32(reader["IdFactura"]);
                            Factura.IdAeropuerto = Convert.ToInt32(reader["IdAeropuerto"]);
                            Factura.Aeropuerto = reader["Aeropuerto"].ToString();
                            Factura.OrdenCompra = reader["OrdenCompra"].ToString();
                            Factura.Folio = reader["Folio"].ToString();
                            Factura.Monto = Convert.ToDecimal(reader["Monto"]);
                            // Factura.FechaFactura = Convert.ToDateTime(reader["FechaFactura"].ToString());
                            Factura.FechaFactura = reader["FechaFactura"].ToString();
                            Factura.IdEstatus = Convert.ToInt32(reader["IdEstatus"]);
                            Factura.Estatus = reader["Estatus"].ToString();
                            Factura.FechaPago = reader["FechaPago"].ToString();
                            response.FacturaList.Add(Factura);
                        }
                    }
                }
                response.Success = true;
            }
            catch (Exception exception)
            {
                response.Success = false;
            }

            return response;
        }

        public AeropuertoListaResponseDTO GetAeropuertoLista()
        {
            var response = new AeropuertoListaResponseDTO()
            {
                AeropuertoLista = new List<AeropuertoListaDTO>()
            };

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmdContacto = new SqlCommand("[dbo].[usp_EPROCUREMENT_Aeropuerto_GETL]", conexion);
                    AeropuertoListaDTO Aeropuerto = null;
                    cmdContacto.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmdContacto.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Aeropuerto = new AeropuertoListaDTO();
                            Aeropuerto.Id = reader["Id"].ToString();
                            Aeropuerto.Nombre = reader["Nombre"].ToString();
                            response.AeropuertoLista.Add(Aeropuerto);
                        }
                    }
                }
                response.Success = true;
            }
            catch (Exception exception)
            {
                response.Success = false;
            }

            return response;
        }

        public EstatusSolicitudResponseDTO GuardarHistoricoEstatusSolicitud(EstatusSolicitudRequestDTO request)
        {
            var response = new EstatusSolicitudResponseDTO()
            {
                ErrorList = new List<ErrorDTO>()
            };

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmdUpdate = new SqlCommand("[dbo].[usp_EPROCUREMENT_HistoricoEstatusSolicitud_INS]", conexion);
                    cmdUpdate.CommandType = CommandType.StoredProcedure;
                    cmdUpdate.Parameters.Add(new SqlParameter("@IdSolicitudFactura", request.IdSolicitudFactura));
                    cmdUpdate.Parameters.Add(new SqlParameter("@IdEstatusSolicitud", request.IdEstatusSolicitud));
                    cmdUpdate.Parameters.Add(new SqlParameter("@RutaPDF", SqlDbType.NVarChar, 350)).Value = request.RutaPDF;
                    cmdUpdate.Parameters.Add(new SqlParameter("@RutaXML", SqlDbType.NVarChar, 350)).Value = request.RutaXML;
                    cmdUpdate.Parameters.Add(new SqlParameter("Result", SqlDbType.BigInt) { Direction = ParameterDirection.ReturnValue });
                    cmdUpdate.ExecuteNonQuery();
                    var resultDelete = Convert.ToInt32(cmdUpdate.Parameters["Result"].Value);

                    response.Success = true;
                }
            }
            catch (Exception exception)
            {
            }
            return response;
        }

        public FacturaDetalleResponseDTO GetFacturaDetalle(FacturaDetalleRequestDTO request)
        {
            var response = new FacturaDetalleResponseDTO()
            {
                FacturaDetalleDTO = new FacturaDetalleDTO()
            };

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    FacturaDetalleDTO FacturaDetalle = null;
                    conexion.Open();
                    var cmdContacto = new SqlCommand("[dbo].[usp_EPROCUREMENT_ProveedorFacturaDetalle_GETL]", conexion);
                    cmdContacto.CommandType = CommandType.StoredProcedure;
                    cmdContacto.Parameters.Add(new SqlParameter("@IdFactura", request.IdFactura));

                    using (SqlDataReader reader = cmdContacto.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            FacturaDetalle = new FacturaDetalleDTO();
                            FacturaDetalle.OrdenCompra = reader["OrdenCompra"].ToString();
                            FacturaDetalle.FechaRecepcion = reader["FechaRecepcion"].ToString();
                            FacturaDetalle.Subtotal = Convert.ToDecimal(reader["Subtotal"].ToString());
                            FacturaDetalle.Anticipo = Convert.ToDecimal(reader["Anticipo"].ToString());
                            FacturaDetalle.Impuestos = Convert.ToDecimal(reader["Impuestos"].ToString());
                            FacturaDetalle.Total = Convert.ToDecimal(reader["Total"].ToString());
                            FacturaDetalle.Pagado = reader["Pagado"].ToString();
                            FacturaDetalle.Facturado = reader["Facturado"].ToString();
                            FacturaDetalle.FechaFactura = reader["FechaFactura"].ToString();
                            FacturaDetalle.NoFactura = reader["NoFactura"].ToString();
                            FacturaDetalle.FechaPago = reader["FechaPago"].ToString();

                            response.FacturaDetalleDTO = FacturaDetalle;
                        }
                    }
                }
                response.Success = true;
            }
            catch (Exception exception)
            {
                response.Success = false;
            }

            return response;
        }

    }
}
