using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto de respuesta de estatus de proveedor
    /// </summary>
    public class EstatusProveedorResponseDTO: ResponseBaseDTO
    {
        public List<EstatusProveedorDTO> EstatusProveedorList { get; set; }
    }
}
