using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class FacturaRequestModel : ResponseBaseModel
    {
        public FacturaFiltroModel FacturaFiltro { get; set; }
    }
}