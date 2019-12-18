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
    public class OrdenCompraData
    {
        public OrdenCompraResponseDTO GetOrdenCompraList(OrdenCompraRequestDTO request)
        {
            var response = new OrdenCompraResponseDTO()
            {
                OrdenCompraList = new List<OrdenCompraDTO>()
            };

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmdContacto = new SqlCommand("[dbo].[usp_EPROCUREMENT_OrdenCompra_GETL]", conexion);
                    OrdenCompraDTO OrdenCompra = null;
                    cmdContacto.CommandType = CommandType.StoredProcedure;
                    cmdContacto.Parameters.Add(new SqlParameter("@IdProveedor", request.OrdenCompraFiltro.IdProveedor));
                    cmdContacto.Parameters.Add(new SqlParameter("@OrdenCompra", request.OrdenCompraFiltro.OrdenCompra));
                    cmdContacto.Parameters.Add(new SqlParameter("@IdEstatus", request.OrdenCompraFiltro.IdEstatus));
                    using (SqlDataReader reader = cmdContacto.ExecuteReader()) 
                    {
                        while (reader.Read())
                        {
                            OrdenCompra = new OrdenCompraDTO();
                            OrdenCompra.NumeroProveedor = reader["NumeroProveedor"].ToString();
                            OrdenCompra.Moneda = reader["Moneda"].ToString();
                            OrdenCompra.Empresa = reader["Empresa"].ToString();
                            OrdenCompra.OrdenCompra = reader["OrdenCompra"].ToString();
                            OrdenCompra.Descripcion = reader["Descripcion"].ToString();
                            OrdenCompra.Importe = Convert.ToDecimal(reader["Importe"].ToString());
                            OrdenCompra.IdEstatus = Convert.ToInt32(reader["IdEstatus"].ToString());
                            OrdenCompra.Estatus = reader["Estatus"].ToString();
                            response.OrdenCompraList.Add(OrdenCompra);
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

        public OrdenCompraDetalleResponseDTO GetOrdenCompraDetalleList(OrdenCompraDetalleRequestDTO request)
        {
            var response = new OrdenCompraDetalleResponseDTO()
            {
                OrdenCompraDetalleList = new List<OrdenCompraDetalleDTO>()
            };

            try
            {
                using (var conexion = new SqlConnection(Helper.Connection()))
                {
                    conexion.Open();
                    var cmdContacto = new SqlCommand("[dbo].[usp_EPROCUREMENT_OrdenCompraLineas_GETL]", conexion);
                    OrdenCompraDetalleDTO OrdenCompraDetalle = null;
                    cmdContacto.CommandType = CommandType.StoredProcedure;
                    cmdContacto.Parameters.Add(new SqlParameter("@OrdenCompra", request.OrdenCompraDetalleFiltro.OrdenCompra));
                    using (SqlDataReader reader = cmdContacto.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrdenCompraDetalle = new OrdenCompraDetalleDTO();
                            OrdenCompraDetalle.Linea = Convert.ToInt32(reader["Linea"].ToString());
                            OrdenCompraDetalle.OrdenCompra = reader["OrdenCompra"].ToString();
                            OrdenCompraDetalle.Empresa = reader["Empresa"].ToString();
                            OrdenCompraDetalle.Agrupador = reader["Agrupador"].ToString();
                            OrdenCompraDetalle.CentroCosto = reader["CentroCosto"].ToString();
                            OrdenCompraDetalle.TipoPresupuesto = reader["TipoPresupuesto"].ToString();
                            OrdenCompraDetalle.Cantidad = Convert.ToDecimal(reader["Cantidad"].ToString());
                            OrdenCompraDetalle.Recibido = Convert.ToDecimal(reader["Recibido"].ToString());
                            OrdenCompraDetalle.Facturado = Convert.ToDecimal(reader["Facturado"].ToString());
                            OrdenCompraDetalle.Solicitud = reader["Solicitud"].ToString();
                            OrdenCompraDetalle.Precio = Convert.ToDecimal(reader["Precio"].ToString());
                            OrdenCompraDetalle.Monto = Convert.ToDecimal(reader["Monto"].ToString());
                            OrdenCompraDetalle.Categoria = reader["Categoria"].ToString();
                            OrdenCompraDetalle.Producto = reader["Producto"].ToString();
                            OrdenCompraDetalle.Unidad = reader["Unidad"].ToString();
                            OrdenCompraDetalle.Moneda = reader["Moneda"].ToString();
                            OrdenCompraDetalle.ImpuestosxVenta = reader["ImpuestosxVenta"].ToString();
                            OrdenCompraDetalle.CuentaProveedor = reader["CuentaProveedor"].ToString();
                            OrdenCompraDetalle.RECIDLinSol = reader["RECIDLinSol"].ToString();
                            OrdenCompraDetalle.EsConvenio = Convert.ToInt32(reader["EsConvenio"].ToString());
                            response.OrdenCompraDetalleList.Add(OrdenCompraDetalle);
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
