using System.Collections.Generic;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto de respuesta de municipios
    /// </summary>
    public class MunicipioResponseDTO : ResponseBaseDTO
    {
        /// <summary>
        /// Representa una lista de Municipios
        /// </summary>
        public List<MunicipioDTO> MunicipioList { get; set; }
    }
}
