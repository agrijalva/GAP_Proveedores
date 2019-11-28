using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class ProveedorFiltroDTO
    {
        public int? IdTipoProveedor { get; set; }
        public int? IdGiroProveedor { get; set; }
        public string IdAeropuerto { get; set; }
        public string NombreEmpresa { get; set; }
        public string RFC { get; set; }
        public string Email { get; set; }
        public int? IdEstatus { get; set; }
    }
}
