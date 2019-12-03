using Microsoft.AspNetCore.Http;
using ProjectsAgenda.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectsAgenda.Web.Models
{
    public class ProjectRemarkViewModel: ProjectRemark
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
        public int ProjectId { get; set; }
        public int PartnerId { get; set; }
    }
}
