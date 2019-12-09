using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class DocumentoModel
    {
            public int IdDetalle { get; set; }
            public string NombreDocumento { get; set; }
            public string RutaDocumento { get; set; }
            public string Extension { get; set; }
            public HttpPostedFileBase File { get; set; }
    }
}
