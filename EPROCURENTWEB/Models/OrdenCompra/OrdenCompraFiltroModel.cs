using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class OrdenCompraFiltroModel
    {
        public int? IdProveedor { get; set; }
        public string OrdenCompra { get; set; }
        public int? IdEstatus { get; set; }
    }
}