using EprocurementWeb.GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class ProveedorDireccionModel
    {
        public int IdProveedorDireccion { get; set; }

        [Required(ErrorMessageResourceType = typeof(RHome), ErrorMessageResourceName = "Mensaje_Error_Required")]
        [Display(ResourceType = typeof(RHome), Name = "ZipCode")]
        public string CodigoPostal { get; set; }

        [Display(ResourceType = typeof(RHome), Name = "Suburb")]
        public string Colonia { get; set; }

        public int IdMunicipio { get; set; }

        [Required(ErrorMessageResourceType = typeof(RHome), ErrorMessageResourceName = "Mensaje_Error_Required")]
        [Display(ResourceType = typeof(RHome), Name = "Address")]
        public string Calle { get; set; }

        //[Required(ErrorMessageResourceType = typeof(RHome), ErrorMessageResourceName = "Mensaje_Error_Required")]
        [Display(ResourceType = typeof(RHome), Name = "Country")]
        public int IdPais { get; set; }
        public int IdEstado { get; set; }

        [Display(ResourceType = typeof(RHome), Name = "State")]
        public string Estado { get; set; }

        [Display(ResourceType = typeof(RHome), Name = "City")]
        public string Municipio { get; set; }
        public bool DireccionValidada { get; set; }
        public int IdProveedor { get; set; }

        [Display(ResourceType = typeof(RHome), Name = "TIN")]
        public string TIN { get; set; }
    }
}