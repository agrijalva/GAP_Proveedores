using System.Configuration;

namespace EPROCUREMENT.GAPPROVEEDOR.Data
{
    public static class Helper
    {
        /// <summary>
        /// Regresa la conexion a base de datos
        /// </summary>
        /// <returns></returns>
        public static string Connection()
        {
            return ConfigurationManager.ConnectionStrings["GAPProveedoresConnectionString"].ToString();
        }
    }
}
