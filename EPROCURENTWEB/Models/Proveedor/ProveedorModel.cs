using EprocurementWeb.GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EprocurementWeb.Models
{
    public class ProveedorModel
    {
        public int IdProveedor { get; set; }        

        [Required(ErrorMessageResourceType = typeof(RHome), ErrorMessageResourceName = "Mensaje_Error_Required")]
        [Display(ResourceType = typeof(RHome), Name = "NameOfCompany")] 
        public string NombreEmpresa { get; set; }

        [Required(ErrorMessageResourceType = typeof(RHome), ErrorMessageResourceName = "Mensaje_Error_Required")]
        [Display(ResourceType = typeof(RHome), Name = "BName")]
        public string RazonSocial { get; set; }

        [StringLength(13, ErrorMessageResourceName = "Mensaje_Error_StringLength", ErrorMessageResourceType = typeof(RHome), MinimumLength = 12)]
        [Required(ErrorMessageResourceType = typeof(RHome), ErrorMessageResourceName = "Mensaje_Error_Required")]
        [Display(ResourceType = typeof(RHome), Name = "RFC")]
        public string RFC { get; set; }

        [Display(ResourceType = typeof(RHome), Name = "NIF")]
        public string NIF { get; set; }

        [Display(ResourceType = typeof(RHome), Name = "Phone")]
        public string ProvTelefono { get; set; }

        [Display(ResourceType = typeof(RHome), Name = "Fax")]
        public string ProvFax { get; set; }

        [Display(ResourceType = typeof(RHome), Name = "WPage")]
        public string PaginaWeb { get; set; }

        [Required(ErrorMessageResourceType = typeof(RHome), ErrorMessageResourceName = "Mensaje_Error_Required")]
        [Display(ResourceType = typeof(RHome), Name = "TZone")]
        public int IdZonaHoraria { get; set; }

        public int IdTipoProveedor { get; set; }

        public string AXNumeroProveedor { get; set; }

        public DateTime? AXFechaRegistro { get; set; }

        public int IdNacionalidad { get; set; }

        public ProveedorDireccionModel Direccion { get; set; }
        public List<ProveedorEmpresaModel> EmpresaList { get; set; }
        public ProveedorContactoModel Contacto { get; set; }
        public List<ProveedorGiroModel> ProveedorGiroList { get; set; }
        public List<AeropuertoModel> AeropuertoList { get; set; }

        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaRevision { get; set; }
        public int IdUsuario { get; set; }
        public string Observaciones { get; set; }
        public int IdEstatusEdicion { get; set; }


        [Display(ResourceType = typeof(RHome), Name = "CompanyTypeMexican")]
        public bool Mexicana { get; set; }
        [Display(ResourceType = typeof(RHome), Name = "CompanyTypeInternational")]
        public bool Extranjera { get; set; }
        public int IdEstatus { get; set; }
        [Display(ResourceType = typeof(RHome), Name = "TIN")]
        public string TIN { get; set; }
    }
}