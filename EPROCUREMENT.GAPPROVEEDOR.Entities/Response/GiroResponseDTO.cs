using System.Collections.Generic;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto de respuesta de Giro
    /// </summary>
    public class GiroResponseDTO: ResponseBaseDTO
    {
        /// <summary>
        /// Representa una lista de Giros
        /// </summary>
        public List<GiroDTO> GiroList { get; set; }
    }
}
