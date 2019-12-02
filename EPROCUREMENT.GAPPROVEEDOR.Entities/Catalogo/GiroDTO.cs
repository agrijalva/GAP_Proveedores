namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto y propiedades de Giro
    /// </summary>
    public class GiroDTO
    {
        /// <summary>
        /// Representa el Id del Giro
        /// </summary>
        public int IdGiro { get; set; }

        /// <summary>
        /// Representa el nombre del Giro
        /// </summary>
        public string GiroNombre { get; set; }
        public bool Registrado { get; set; }
    }
}
