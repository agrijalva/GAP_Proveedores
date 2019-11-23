using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class ProveedorGiroModel
    {
        public int IdProveedorGiro { get; set; }
        public int IdProveedor { get; set; }
        public int IdCatalogoGiro { get; set; }
    }
}