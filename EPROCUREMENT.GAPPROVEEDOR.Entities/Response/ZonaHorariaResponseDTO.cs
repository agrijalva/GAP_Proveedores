using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto de respuesta de ZonaHoraria
    /// </summary>
    public class ZonaHorariaResponseDTO: ResponseBaseDTO
    {
        /// <summary>
        /// Representa una lista de ZonaHoraria
        /// </summary>
        public List<ZonaHorariaDTO> ZonaHorariaList { get; set; }
    }
}
