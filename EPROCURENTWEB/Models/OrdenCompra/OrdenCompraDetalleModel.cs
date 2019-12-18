using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class OrdenCompraDetalleModel
    {
        public int Linea { get; set; }
        public string OrdenCompra { get; set; }
        public string Empresa { get; set; }
        public string Agrupador { get; set; }
        public string CentroCosto { get; set; }
        public string TipoPresupuesto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Recibido { get; set; }
        public decimal Facturado { get; set; }
        public string Solicitud { get; set; }
        public decimal Precio { get; set; }
        public decimal Monto { get; set; }
        public string Categoria { get; set; }
        public string Producto { get; set; }
        public string Unidad { get; set; }
        public string Moneda { get; set; }
        public string ImpuestosxVenta { get; set; }
        public string CuentaProveedor { get; set; }
        public string RECIDLinSol { get; set; }
        public int EsConvenio { get; set; }
    }
}