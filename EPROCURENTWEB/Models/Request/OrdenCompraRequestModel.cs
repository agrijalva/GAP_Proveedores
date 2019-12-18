using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class OrdenCompraRequestModel : ResponseBaseModel
    {
        public OrdenCompraFiltroModel OrdenCompraFiltro { get; set; }
    }
}