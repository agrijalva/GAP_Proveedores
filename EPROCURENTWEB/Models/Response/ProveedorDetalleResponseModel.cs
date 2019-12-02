using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class ProveedorDetalleResponseModel: ResponseBaseModel
    {

        /// <summary>
        /// Representa el objeto del proveedor
        /// </summary>
        public ProveedorModel Proveedor { get; set; }
    }
}