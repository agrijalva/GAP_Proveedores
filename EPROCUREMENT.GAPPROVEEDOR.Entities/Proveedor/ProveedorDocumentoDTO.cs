using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class ProveedorDocumentoDTO
    {
        public int IdProveedorDocumento { get; set; }
        public int IdProveedor { get; set; }
        public int IdCatalogoDocumento { get; set; }
        public DateTime FechaAlta { get; set; }
        public string DescripcionDocumento { get; set; }
        public bool DocumentoAutorizado { get; set; }
        public string NombreArchivo { get; set; }
    }
}
