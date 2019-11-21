using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    /// <summary>
    /// Representa el objeto y propiedades del Idioma
    /// </summary>
    public class IdiomaDTO
    {
        /// <summary>
        /// Representa el Id del Idioma
        /// </summary>
        public int IdIdioma { get; set; }

        /// <summary>
        /// Representa el nombre del idioma
        /// </summary>
        public string NombreIdioma { get; set; }

        /// <summary>
        /// Representa el origen del idioma
        /// </summary>
        public string Origen { get; set; }

        /// <summary>
        /// Representa el estatus del idioma
        /// </summary>
        public bool Status { get; set; }
    }
}
