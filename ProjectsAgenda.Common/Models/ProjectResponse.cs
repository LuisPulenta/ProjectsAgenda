using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsAgenda.Common.Models
{
    public class ProjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Active { get; set; }
        //public PartnerResponse Partner { get; set; }
        public ICollection<ProjectRemarkResponse> ProjectRemarks { get; set; }
        public ICollection<UserProjectResponse> UserProjects { get; set; }
    }
}
