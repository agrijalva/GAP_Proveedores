using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class SolicitudFacturaFiltroModel
    {
        public int IdProveedor { get; set; }
        public int? IdSolicitudFactura { get; set; }
        public int? IdEstatus { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}