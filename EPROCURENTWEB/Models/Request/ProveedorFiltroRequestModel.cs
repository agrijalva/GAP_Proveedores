using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class ProveedorFiltroRequestModel: RequestBaseModel
    {
        public string Filtro { get; set; }
        public TipoFiltro TipoFiltro { get; set; }
    }
}