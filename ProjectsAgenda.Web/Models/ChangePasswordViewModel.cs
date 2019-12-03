using System.ComponentModel.DataAnnotations;

namespace ProjectsAgenda.Web.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Password Actual")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string OldPassword { get; set; }

        [Display(Name = "Nuevo password")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm. Password")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Compare("NewPassword")]
        public string Confirm { get; set; }
    }
}
