using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class EmailDTO
    {
        /// <summary>
        /// Cuenta o correo de origen
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Nombre que se desplegará con la cuenta origen
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// Asunto del correo
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Mensaje del correo
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Formato Html del correo
        /// </summary>
        public bool Html { get; set; }

        /// <summary>
        /// Destinatarios del correo
        /// </summary>
        public List<DireccionEmailDTO> RecipientsList { get; set; }

        /// <summary>
        /// Destinatarios con copia
        /// </summary>
        public List<DireccionEmailDTO> CopyList { get; set; }

        /// <summary>
        /// Destinatarios con copia oculta
        /// </summary>
        public List<DireccionEmailDTO> HiddenCopyList { get; set; }

        /// <summary>
        /// Archivos adjuntos del correo
        /// </summary>
        public List<string> AttachedFilesList { get; set; }

        /// <summary>
        /// Prioridad del correo.
        /// </summary>
        public EmailPrioridadDTO Prioridad { get; set; }
    }
}
