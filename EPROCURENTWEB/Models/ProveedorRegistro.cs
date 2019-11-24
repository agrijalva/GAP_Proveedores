using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPROCUREMENT.GAPPROVEEDOR.Entities;

namespace EprocurementWeb.Models
{
    public class ProveedorRegistro : ProveedorModel
    {
        public List<AeropuertoModel> AeropuertoList { get; set; }
    }
}