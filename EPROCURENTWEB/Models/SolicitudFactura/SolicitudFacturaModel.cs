using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class SolicitudFacturaModel
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