using System.Collections.Generic;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto de respuesta de Idiomas
    /// </summary>
    public class IdiomaResponseDTO : ResponseBaseDTO
    {
        /// <summary>
        /// Representa una lista de Idiomas
        /// </summary>
        public List<IdiomaDTO> IdiomaList { get; set; }
    }
}
