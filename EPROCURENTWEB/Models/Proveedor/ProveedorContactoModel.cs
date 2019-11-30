using EprocurementWeb.GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class ProveedorContactoModel
    {
        public int IdContacto { get; set; }
        public int IdProveedor { get; set; }

        [Required(ErrorMessageResourceType = typeof(RHome), ErrorMessageResourceName = "Mensaje_Error_Required")]
        [Display(ResourceType = typeof(RHome), Name = "CName")]
        public string NombreContacto { get; set; }

        [Display(ResourceType = typeof(RHome), Name = "Position")]
        public string Cargo { get; set; }

        [Required(ErrorMessageResourceType = typeof(RHome), ErrorMessageResourceName = "Mensaje_Error_Required")]
        [Display(ResourceType = typeof(RHome), Name = "Nacionality")]
        public int IdNacionalidad { get; set; }

        [Display(ResourceType = typeof(RHome), Name = "CPhone")]
        public string TelefonoDirecto { get; set; }

        [Display(ResourceType = typeof(RHome), Name = "CellPhone")]
        public string TelefonoMovil { get; set; }

        [Display(ResourceType = typeof(RHome), Name = "Fax")]
        public string Fax { get; set; }

        [Required(ErrorMessageResourceType = typeof(RHome), ErrorMessageResourceName = "Mensaje_Error_Required")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(RHome), ErrorMessageResourceName = "Message_Error_Email")]
        [EmailAddress]
        [Display(ResourceType = typeof(RHome), Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(RHome), ErrorMessageResourceName = "Mensaje_Error_Required")]
        [Display(ResourceType = typeof(RHome), Name = "TZone")]
        public int IdZonaHoraria { get; set; }

        [Display(ResourceType = typeof(RHome), Name = "Country")]
        public int IdPais { get; set; }

        [Required(ErrorMessageResourceType = typeof(RHome), ErrorMessageResourceName = "Mensaje_Error_Required")]
        [Display(ResourceType = typeof(RHome), Name = "Language")]
        public int IdIdioma { get; set; }

        public int ContactoPrincipal { get; set; }

        [Display(ResourceType = typeof(RHome), Name = "PrincipalContacto")]
        public bool EsPrincipal { get; set; }
    }
}