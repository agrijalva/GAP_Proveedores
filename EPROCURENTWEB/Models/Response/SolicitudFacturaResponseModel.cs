﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class SolicitudFacturaResponseModel : ResponseBaseModel
    {
        public List<SolicitudFacturaModel> SolicitudFacturaList { get; set; }
    }
}