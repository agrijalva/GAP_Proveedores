using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class SolicitudFacturaDetalleDTO
    {
        public int Linea { get; set; }
        public string Descripcion { get; set; }
        public decimal CantidadTotal { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal CantidadPendiente { get; set; }
        public decimal MontoPendiente { get; set; }
        public decimal CantidadSolicitada { get; set; }
        public decimal MontoSolicitada { get; set; }
    }
}
