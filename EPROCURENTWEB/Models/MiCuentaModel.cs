using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class MiCuentaModel
    {
        public ActualizaPasswordModel ActualizaPassword { get; set; }
        public ProveedorModel Proveedor { get; set; }

        public List<ProveedorCuentaDTO> ProveedorCuentaList { get; set; }

        public List<CatalogoDocumentoDTO> CatalogoDocumentoList { get; set; }
    }
}