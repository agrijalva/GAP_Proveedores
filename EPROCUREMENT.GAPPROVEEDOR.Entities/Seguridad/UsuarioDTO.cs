using System;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompania { get; set; }
        public int IdUsuarioRol { get; set; }
        public int IdEstatus { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Password { get; set; }
        public bool EsActivo { get; set; }
        public int IdProveedor { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
