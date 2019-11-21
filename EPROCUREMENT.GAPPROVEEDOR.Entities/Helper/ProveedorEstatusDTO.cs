using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class ProveedorEstatusDTO
    {        
        public int IdProveedor { get; set; }
        public string RFC { get; set; }
        public string NombreEmpresa { get; set; }
        public string Email { get; set; }
        public string Estatus { get; set; }
        public int IdEstatus { get; set; }
        public string AXNumeroProveedor { get; set; }
    }
}
