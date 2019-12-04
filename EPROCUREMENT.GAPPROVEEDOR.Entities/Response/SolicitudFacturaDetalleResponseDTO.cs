using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class SolicitudFacturaDetalleResponseDTO: ResponseBaseDTO
    {
        public SolicitudFacturaCabeceraDTO SolicitudFacturaCabecera { get; set; }
        public List<SolicitudFacturaDetalleDTO> SolicitudFacturaDetalleList { get; set; }
    }
}
