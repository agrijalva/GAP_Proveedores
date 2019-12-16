using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class AeropuertoListaResponseModel: ResponseBaseModel
    {
        /// <summary>
        /// Representa una lista de Aeropuerto
        /// </summary>
        public List<AeropuertoListaModel> AeropuertoLista { get; set; }
    }
}