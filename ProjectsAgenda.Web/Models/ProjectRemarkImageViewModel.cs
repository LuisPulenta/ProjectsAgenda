using Microsoft.AspNetCore.Http;
using ProjectsAgenda.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjectsAgenda.Web.Models
{
    public class ProjectRemarkImageViewModel : ProjectRemark
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

    }
}
