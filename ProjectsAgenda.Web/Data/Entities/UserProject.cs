using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectsAgenda.Web.Data.Entities
{
    public class UserProject
    {
        public int Id { get; set; }
      
        
        public int IdAdmin { get; set; }

        [Display(Name = "Agregado el")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime AddDate {get; set;}

        [Display(Name = "Activo")]
        public bool Active { get; set; }

        public Project Project { get; set; }
        public Partner Partner { get; set; }
    }
}