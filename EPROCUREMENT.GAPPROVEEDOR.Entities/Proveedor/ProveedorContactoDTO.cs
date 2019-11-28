using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class ProveedorContactoDTO
    {
        public int IdContacto { get; set; }
        public int IdProveedor { get; set; }
        public string NombreContacto { get; set; }
        public string Cargo { get; set; }
        public int IdNacionalidad { get; set; }
        public string TelefonoDirecto { get; set; }
        public string TelefonoMovil { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int IdZonaHoraria { get; set; }
        public int IdPais { get; set; }
        public int IdIdioma { get; set; }
        public string Tipo { get; set; }
        public int ContactoPrincipal { get; set; }

        
    }
}
