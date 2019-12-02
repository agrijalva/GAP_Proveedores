using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class AeropuertoResponseModel: ResponseBaseModel
    {
        /// <summary>
        /// Representa una lista de Aeropuerto
        /// </summary>
        public List<AeropuertoModel> AeropuertoList { get; set; }
    }
}