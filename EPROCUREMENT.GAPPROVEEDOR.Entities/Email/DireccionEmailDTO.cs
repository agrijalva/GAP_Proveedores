using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class DireccionEmailDTO
    {
        /// <summary>
        /// Obtiene o establece la dirección de correo electrónico
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre a mostrar
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Representa el identificador del usuario
        /// </summary>
        public ulong UserIdentifier { get; set; }
    }
}
