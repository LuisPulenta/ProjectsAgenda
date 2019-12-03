using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectsAgenda.Web.Models
{
    public class PartnerViewModel
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Socio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe elegir un Socio.")]
        public int PartnerId { get; set; }

        public IEnumerable<SelectListItem> Partners { get; set; }
    }
}