using System.Collections.Generic;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto de respuesta de Pais
    /// </summary>
    public class PaisResponseDTO: ResponseBaseDTO
    {
        /// <summary>
        /// Representa una lista de Pais
        /// </summary>
        public List<PaisDTO> PaisList { get; set; }
    }
}
