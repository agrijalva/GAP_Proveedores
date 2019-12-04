using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class SolicitudFacturaRequestModel : ResponseBaseModel
    {
        public SolicitudFacturaFiltroModel SolicitudFacturaFiltro { get; set; }
    }
}