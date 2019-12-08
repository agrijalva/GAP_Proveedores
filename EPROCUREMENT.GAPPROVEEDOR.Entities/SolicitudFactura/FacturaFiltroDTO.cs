using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class FacturaFiltroDTO
    {
        public int IdProveedor { get; set; }
        public int? IdAeropuerto { get; set; }
        public string OrdenCompra { get; set; }
        public string Folio { get; set; }
        public DateTime? FechaFacInicio { get; set; }
        public DateTime? FechaFacFin { get; set; }
        public DateTime? FechaPagoInicio { get; set; }
        public DateTime? FechaPagoFin { get; set; }
    }
}
