using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System.Collections.Generic;

namespace Eprocurement.Compras.Models
{
    public class ProveedorRegistro : ProveedorDTO
    {
        public List<AeropuertoDTO> AeropuertoList { get; set; }
        public int IdEstatus { get; set; }
    }
}