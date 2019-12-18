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
    }
}
