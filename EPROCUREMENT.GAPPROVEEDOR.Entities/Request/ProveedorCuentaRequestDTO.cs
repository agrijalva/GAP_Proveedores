using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class ProveedorCuentaRequestDTO: RequestBaseDTO
    {
        public List<ProveedorCuentaDTO> ProveedorCuentaList { get; set; }
        public int IdProveedor { get; set; }
    }
}
