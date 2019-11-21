using System.Collections.Generic;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class ProveedorDocumentoRequestDTO : RequestBaseDTO
    {
        public List<ProveedorDocumentoDTO> ProveedorDocumentoList { get; set; }
    }
}
