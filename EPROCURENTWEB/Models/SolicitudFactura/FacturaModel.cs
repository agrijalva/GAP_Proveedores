using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class FacturaModel
    {
        public int IdFactura { get; set; }
        public int IdAeropuerto { get; set; }
        public string Aeropuerto { get; set; }
        public string OrdenCompra { get; set; }
        public string Folio { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaFactura { get; set; }
        public int IdEstatus { get; set; }
        public string Estatus { get; set; }
        public DateTime FechaPago { get; set; }
    }
}