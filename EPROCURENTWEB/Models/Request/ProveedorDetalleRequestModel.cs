using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class ProveedorDetalleRequestModel: RequestBaseModel
    {
        /// <summary>
        /// Representa el identificador del proveedor
        /// </summary>
        public int IdProveedor { get; set; }
    }
}