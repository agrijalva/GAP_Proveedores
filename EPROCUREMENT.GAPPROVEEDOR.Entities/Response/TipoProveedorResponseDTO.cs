using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto de respuesta de TipoProveedor
    /// </summary>
    public class TipoProveedorResponseDTO : ResponseBaseDTO
    {
        /// <summary>
        /// Representa una lista de Idiomas
        /// </summary>
        public List<TipoProveedorDTO> TipoProveedorList { get; set; }
    }
}
