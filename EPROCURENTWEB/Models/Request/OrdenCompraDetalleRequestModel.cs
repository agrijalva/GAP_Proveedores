using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class OrdenCompraDetalleRequestModel : ResponseBaseModel
    {
        public OrdenCompraDetalleFiltroModel OrdenCompraDetalleFiltro { get; set; }
    }
}