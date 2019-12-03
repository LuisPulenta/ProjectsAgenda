using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsAgenda.Common.Models
{
    public class ProjectRemarkResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Remark { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ExpirationDate { get; set; }
        //public ProjectResponse Project { get; set; }
        public PartnerResponse Partner { get; set; }
    }
}