using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto de respuesta de Nacionalidad
    /// </summary>
    public class NacionalidadResponseDTO: ResponseBaseDTO
    {
        /// <summary>
        /// Representa una lista de nacionalidades
        /// </summary>
        public List<NacionalidadDTO> NacionalidadList { get; set; }
    }
}
