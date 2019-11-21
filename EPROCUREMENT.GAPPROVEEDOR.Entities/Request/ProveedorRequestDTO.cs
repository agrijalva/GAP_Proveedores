using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto de solicitud de proveedor
    /// </summary>
    public class ProveedorRequesteDTO: RequestBaseDTO
    {
        /// <summary>
        /// Representa la información del proveedor
        /// </summary>
        public ProveedorDTO Proveedor { get; set; }
    }
}
