namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto de solicitud de estatus de proveedor
    /// </summary>
    public class ProveedorAprobarRequestDTO
    {
        /// <summary>
        /// Representa la infomración del estatus y proveedor
        /// </summary>
        public HistoricoEstatusProveedorDTO EstatusProveedor { get; set; }
    }
}
