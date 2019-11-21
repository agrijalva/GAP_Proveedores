using System.Collections.Generic;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto de respuesta de Estados
    /// </summary>
    public class EstadoResponseDTO: ResponseBaseDTO
    {
        /// <summary>
        /// Representa una lista de Estados
        /// </summary>
        public List<EstadoDTO> EstadoList { get; set; }
    }
}
