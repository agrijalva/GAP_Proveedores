using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class ProveedorCuentaDTO
    {
        public int IdProveedorCuenta { get; set; }
        public string Cuenta { get; set; }
        public int IdBanco { get; set; }
        public string NombreBanco { get; set; }
        public string CLABE { get; set; }
        public int IdTipoCuenta { get; set; }
        public int IdProveedor { get; set; }
        public string TipoCuenta { get; set; }
        public List<AeropuertoDTO> AeropuertoList { get; set; }
    }
}
