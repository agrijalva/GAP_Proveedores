using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class FacturaRequestDTO: ResponseBaseDTO
    {
        public FacturaFiltroDTO FacturaFiltro { get; set; }
    }
}
