using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class TipoCuentaResponseDTO : ResponseBaseDTO
    {
        public List<TipoCuentaDTO> TipoCuentaList { get; set; }
    }
}
