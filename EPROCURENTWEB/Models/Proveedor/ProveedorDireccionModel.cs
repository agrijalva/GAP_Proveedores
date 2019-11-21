using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models.Proveedor
{
    public class ProveedorDireccionModel
    {
        public int IdProveedorDireccion { get; set; }
        public string CodigoPostal { get; set; }
        public string Colonia { get; set; }
        public int IdMunicipio { get; set; }
        public string Calle { get; set; }
        public int IdPais { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public bool DireccionValidada { get; set; }
        public int IdProveedor { get; set; }
    }
}