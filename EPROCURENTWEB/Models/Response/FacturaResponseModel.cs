﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class FacturaResponseModel : ResponseBaseModel
    {
        public List<FacturaModel> FacturaList { get; set; }
    }
}