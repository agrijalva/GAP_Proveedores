using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class RequestBaseModel
    {
        /// <summary>
        /// Id del usuario en sesion
        /// </summary>
        public ulong IdUsuario { get; set; }
    }
}