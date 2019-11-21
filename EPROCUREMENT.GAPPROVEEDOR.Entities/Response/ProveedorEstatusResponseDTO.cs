using System.Collections.Generic;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class ProveedorEstatusResponseDTO: ResponseBaseDTO
    {
        public List<ProveedorEstatusDTO> ProveedorList { get; set; }
    }
}
