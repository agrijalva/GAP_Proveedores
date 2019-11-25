using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class ProveedorCuentaResponseDTO: ResponseBaseDTO
    {
        public List<ProveedorCuentaDTO> ProveedorCuentaList { get; set; }
    }
}
