using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class EstatusSolicitudRequestDTO: RequestBaseDTO
    {
        public int IdSolicitudFactura { get; set; }
        public int IdEstatusSolicitud { get; set; }
    }
}
