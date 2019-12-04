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

    }
}
