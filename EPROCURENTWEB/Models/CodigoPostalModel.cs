using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class CodigoPostalModel
    {
        public string codigo_postal { get; set; }
        public string municipio { get; set; }
        public string estado { get; set; }
        public List<string> colonias { get; set; }
    }
}