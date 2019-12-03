using System.ComponentModel.DataAnnotations;

namespace ProjectsAgenda.Common.Models
{
    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}