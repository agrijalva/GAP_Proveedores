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
    public class AeropuertoModel
    {
        /// <summary>
        /// Representa el Id del aeropuerto
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Representa El nombre del aeropuerto
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Representa El nombre del aeropuerto
        /// </summary>
        public bool Checado { get; set; }
        public bool Agregado { get; set; }
    }
}