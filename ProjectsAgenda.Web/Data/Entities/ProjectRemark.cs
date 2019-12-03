using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectsAgenda.Web.Data.Entities
{
    public class ProjectRemark
    {
        public int Id { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Comentario")]
        public string Remark { get; set; }

        [Display(Name = "Foto")]
        public string ImageUrl { get; set; }

        [Display(Name = "Fecha Vencimiento")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpirationDate { get; set; }
        public Project Project { get; set; }
        public Partner Partner { get; set; }

    }
}
