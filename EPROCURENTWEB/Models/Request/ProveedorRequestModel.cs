using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class ProveedorRequestModel: RequestBaseModel
    {
        /// <summary>
        /// Representa la información del proveedor
        /// </summary>
        public ProveedorModel Proveedor { get; set; }
    }
}