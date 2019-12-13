using EprocurementWeb.GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    /// <summary>
    /// Representa el objeto y propiedades de Aeropuerto
    /// </summary>
    public class AeropuertoListaModel
    {
        public string Id { get; set; }

        public string Nombre { get; set; }
    }
}