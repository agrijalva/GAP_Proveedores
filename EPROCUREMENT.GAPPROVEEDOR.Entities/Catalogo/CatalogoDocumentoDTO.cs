using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class CatalogoDocumentoDTO
    {
        public int IdCatalogoDocumento { get; set; }
        public string NombreDocumento { get; set; }
        public string RutaDocumento { get; set; }
        public int IdFormatoArchivo { get; set; }
        public bool EsRequerido { get; set; }
        public int IdFormulario { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}
