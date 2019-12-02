using System.Collections.Generic;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class ProveedorDocumentoResponseDTO: ResponseBaseDTO
    {
        public List<ProveedorDocumentoDTO> ProveedorDocumentoList { get; set; }
    }
}
