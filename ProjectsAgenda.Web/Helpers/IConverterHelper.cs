using System.Threading.Tasks;
using ProjectsAgenda.Web.Data.Entities;
using ProjectsAgenda.Web.Models;

namespace ProjectsAgenda.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<Project> ToProjectAsync(ProjectViewModel model, bool isNew);
        ProjectViewModel ToProjectViewModel(Project project);
    }
}
