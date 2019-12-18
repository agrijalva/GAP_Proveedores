using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class OrdenCompraFiltroDTO
    {
        public int IdProveedor { get; set; }
        public string OrdenCompra { get; set; }
        public int IdEstatus { get; set; }
    }
}
