using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class FacturaFiltroModel
    {
        public int IdProveedor { get; set; }
        public string IdAeropuerto { get; set; }
        public string OrdenCompra { get; set; }
        public string Folio { get; set; }
        public DateTime? FechaFacInicio { get; set; }
        public DateTime? FechaFacFin { get; set; }
        public DateTime? FechaPagoInicio { get; set; }
        public DateTime? FechaPagoFin { get; set; }
    }
}