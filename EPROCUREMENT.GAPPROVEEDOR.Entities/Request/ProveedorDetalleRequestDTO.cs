using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto de solicitud de consulta de proveedor
    /// </summary>
    public class ProveedorDetalleRequestDTO: RequestBaseDTO
    {
        /// <summary>
        /// Representa el identificador del proveedor
        /// </summary>
        public int IdProveedor { get; set; }
    }
}
