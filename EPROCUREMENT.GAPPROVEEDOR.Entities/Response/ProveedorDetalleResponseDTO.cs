using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto de respuesta de consulta de proveedor
    /// </summary>
    public class ProveedorDetalleResponseDTO: ResponseBaseDTO
    {
        /// <summary>
        /// Representa el objeto del proveedor
        /// </summary>
        public ProveedorDTO Proveedor { get; set; }
    }
}
