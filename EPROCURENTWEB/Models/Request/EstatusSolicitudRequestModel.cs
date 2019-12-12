using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class EstatusSolicitudRequestModel: RequestBaseModel
    {
        public int IdSolicitudFactura { get; set; }
        public int IdEstatusSolicitud { get; set; }
    }
}