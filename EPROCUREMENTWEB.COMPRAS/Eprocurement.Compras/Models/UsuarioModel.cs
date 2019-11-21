using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eprocurement.Compras.Models
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Usuario:")]
        [Required]
        public string NombreUsuario { get; set; }
        public string NombreCompania { get; set; }
        public int IdUsuarioRol { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool EsActivo { get; set; }
        public int IdProveedor { get; set; }
        public string Token { get; set; }
        [Display(Name = "Olvidsaste la contraseña")]
        public string TitleRecovery { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraeña:")]
        [Required]
        public string Password { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraeña:")]
        [Required]
        public string PasswordNueva { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraeña:")]
        [Compare("Password")]
        [Required]
        public string ConfirmarPassword { get; set; }
        public string Email { get; set; }
    }
}