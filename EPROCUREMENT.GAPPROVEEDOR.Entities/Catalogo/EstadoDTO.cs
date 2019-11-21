using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto y propiedades del Estado
    /// </summary>
    public class EstadoDTO
    {
        /// <summary>
        /// Representa el Id del Estado
        /// </summary>
        public int IdEstado { get; set; }

        /// <summary>
        /// Representa el nombre
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Representa el Id del Pais
        /// </summary>
        public int IdPais { get; set; }
    }
}
