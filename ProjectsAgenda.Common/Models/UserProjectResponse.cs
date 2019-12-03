using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsAgenda.Common.Models
{
    public class UserProjectResponse
    {
        public int Id { get; set; }
        public int IdAdmin { get; set; }
        public DateTime AddDate { get; set; }
        public bool Active { get; set; }
        //public ProjectResponse Project { get; set; }
        public PartnerResponse Partner { get; set; }
    }
}
