using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectsAgenda.Web.Data.Entities
{
    public class Partner
    {
        public int Id { get; set; }
        public User User { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<UserProject> UserProjects { get; set; }
        public ICollection<ProjectRemark> ProjectRemarks { get; set; }
    }
}
