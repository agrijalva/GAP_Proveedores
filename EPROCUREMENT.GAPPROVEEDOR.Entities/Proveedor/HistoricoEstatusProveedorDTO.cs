using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class HistoricoEstatusProveedorDTO
    {
        public int IdHistoricoEstatusProveedor { get; set; }
        public int IdEstatusProveedor { get; set; }
        public int IdProveedor { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Observaciones { get; set; }
        public int? IdUsuario { get; set; }
    }
}
