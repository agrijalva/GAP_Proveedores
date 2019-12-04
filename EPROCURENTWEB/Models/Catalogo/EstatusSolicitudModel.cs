using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class EstatusSolicitudModel
    {
        public int IdEstatus { get; set; }
        public string Descripcion { get; set; }
    }

    public class SolicitudFactura
    {
        public int IdSolicitudFactura { get; set; }
    }
}