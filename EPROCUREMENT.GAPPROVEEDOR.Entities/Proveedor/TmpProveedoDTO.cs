using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities.Proveedor
{
    public class TmpProveedoDTO
    {
         public int TmpProveedor { get; set; }
        public int IdProveedor { get; set; }
        public string NombreEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public string ProvTelefono { get; set; }
        public string PaginaWeb { get; set; }
        public int IdZonaHoraria { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaRevision { get; set; }
        public int IdUsuario { get; set; }
        public string Observaciones { get; set; }
        public int IdEstatusEdicion { get; set; }
    }
}
