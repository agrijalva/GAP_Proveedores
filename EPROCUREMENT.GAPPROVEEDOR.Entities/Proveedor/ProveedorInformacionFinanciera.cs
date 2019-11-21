using System.Collections.Generic;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities.Proveedor
{
    public class ProveedorInformacionFinanciera
    {
        public ProveedorCuentaDTO  CuentaBancaria { get; set; }
        public string RFC { get; set; }
        public List<ProveedorCuentaDTO> ProveedorCuentaList { get; set; }
        public List<ProveedorCuentaDTO> ProveedorCuentaListRegistro { get; set; }
        public List<CatalogoDocumentoDTO> CatalogoDocumentoList { get; set; }
        public bool Respuesta { get; set; }
    }
}
