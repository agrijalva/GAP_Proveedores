using EprocurementWeb.GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class ActualizaPasswordModel
    {
        //[StringLength(15, ErrorMessageResourceName = "Mensaje_Error_StringLength", ErrorMessageResourceType = typeof(EtiquetaForm), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(EtiquetaForm), Name = "ActualizaPassword_ContrasenaAnterior")]
        [Required(ErrorMessageResourceType = typeof(EtiquetaForm), ErrorMessageResourceName = "Mensaje_Error_Required")]
        public string Password { get; set; }

        [StringLength(15, ErrorMessageResourceName = "Mensaje_Error_StringLength", ErrorMessageResourceType = typeof(EtiquetaForm), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(EtiquetaForm), Name = "ActualizaPassword_NuevaContrasena")]
        [Required(ErrorMessageResourceType = typeof(EtiquetaForm), ErrorMessageResourceName = "Mensaje_Error_Required")]
        public string PasswordNueva { get; set; }

        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [StringLength(15, ErrorMessageResourceName = "Mensaje_Error_StringLength", ErrorMessageResourceType = typeof(EtiquetaForm), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(EtiquetaForm), Name = "ActualizaPassword_ConfirmarContrasena")]
        [Compare("PasswordNueva", ErrorMessageResourceName = "Mensaje_Error_ComparePassword", ErrorMessageResourceType = typeof(EtiquetaForm))]
        [Required(ErrorMessageResourceType = typeof(EtiquetaForm), ErrorMessageResourceName = "Mensaje_Error_Required")]
        public string ConfirmarPassword { get; set; }

        public bool Accion { get; set; }
    }
}