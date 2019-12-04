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
        public decimal CantidadAdquirida { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal ImporteAdquirido { get; set; }
        public decimal CantidadFacturada { get; set; }
        public decimal CantidadFacturar { get; set; }
        public decimal ImporteFacturado { get; set; }
        public decimal ImporteFacturar { get; set; }
    }
}
