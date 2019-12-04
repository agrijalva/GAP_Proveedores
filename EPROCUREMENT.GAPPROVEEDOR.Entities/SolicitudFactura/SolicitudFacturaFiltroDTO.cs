using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class SolicitudFacturaFiltroDTO
    {
        public int IdProveedor { get; set; }
        public int? IdSolicitudFactura { get; set; }
        public int? IdEstatus { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
