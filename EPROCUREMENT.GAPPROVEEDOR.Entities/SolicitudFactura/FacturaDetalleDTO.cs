using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class FacturaDetalleDTO
    {
        public string OrdenCompra { get; set; }
        public string FechaRecepcion { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Anticipo { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Total { get; set; }
        public string Pagado { get; set; }
        public string Facturado { get; set; }
        public string FechaFactura { get; set; }
        public string NoFactura { get; set; }
        public string FechaPago { get; set; }
    }
}
