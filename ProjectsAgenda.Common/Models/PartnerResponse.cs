using System.Collections.Generic;

namespace ProjectsAgenda.Common.Models
{
    public class PartnerResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public ICollection<ProjectResponse> Projects { get; set; }

    }
}
