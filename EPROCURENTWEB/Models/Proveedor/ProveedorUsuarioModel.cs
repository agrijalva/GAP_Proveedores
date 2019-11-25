using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class ProveedorUsuarioModel
    {
        public string RFC { get; set; }
        public string NombreEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public string Contacto { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string NumeroProveedorAX { get; set; }
        public string ExtensionesConfiguradas { get; set; }
        public string Observaciones { get; set; }
    }
}