using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class OrdenCompraModel
    {
        public string NumeroProveedor { get; set; }
        public string Moneda { get; set; }
        public string Empresa { get; set; }
        public string OrdenCompra { get; set; }
        public string Descripcion { get; set; }
        public decimal Importe { get; set; }
        public int IdEstatus { get; set; }
        public string Estatus { get; set; }
    }
}