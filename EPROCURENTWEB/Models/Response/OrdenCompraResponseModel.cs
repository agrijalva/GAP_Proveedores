using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class OrdenCompraResponseModel : ResponseBaseModel
    {
        public List<OrdenCompraModel> OrdenCompraList { get; set; }
    }
}