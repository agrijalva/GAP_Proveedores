using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class SolicitudFacturaDTO
    {
        public int IdSolicitudFactura { get; set; }
        public string NumeroRecepcion { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public decimal Monto { get; set; }
        public string RutaPDF { get; set; }
        public string RutaXML { get; set; }
        public int UltimoEstatus { get; set; }
        public string UltimoStatusLabel { get; set; }
    }
}
