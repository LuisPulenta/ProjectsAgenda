using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectsAgenda.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectsAgenda.Web.Models
{
    public class ProjectViewModel:Project
    {
        public int PartnerId { get; set; }
    }
}
