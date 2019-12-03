using ProjectsAgenda.Web.Data;
using ProjectsAgenda.Web.Data.Entities;
using ProjectsAgenda.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectsAgenda.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _dataContext;

        public ConverterHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Project> ToProjectAsync(ProjectViewModel model, bool isNew)
        {
            return new Project
            {
                Active=model.Active,
                CreationDate = model.CreationDate,
                EndDate = model.EndDate,
                Name = model.Name,
                Id = isNew ? 0 : model.Id,
                Partner= await _dataContext.Partners.FindAsync(model.PartnerId),
                ProjectRemarks = isNew ? new List<ProjectRemark>() : model.ProjectRemarks,
                UserProjects = isNew ? new List<UserProject>() : model.UserProjects,
            };
        }

        public ProjectViewModel ToProjectViewModel(Project project)
        {
            return new ProjectViewModel
            {
                Active = project.Active,
                Id=project.Id,
                CreationDate = project.CreationDate,
                EndDate = project.EndDate,
                Name = project.Name,
                PartnerId=project.Partner.Id,
                Partner=project.Partner,
            };
        }


    }
}
