using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class SolicitudFacturaCabeceraDTO
    {
        public string Descripcion { get; set; }
        public decimal ImporteContratado { get; set; }
        public decimal ImporteAdquirido { get; set; }
        public decimal ImporteFacturado { get; set; }
        public decimal ImporteFacturar { get; set; }
        public int IdEstatusSolicitud { get; set; }
    }
}
