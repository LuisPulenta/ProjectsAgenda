using Prism.Navigation;
using ProjectsAgenda.Common.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProjectsAgenda.Prism.ViewModels
{
    public class ProjectsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private PartnerResponse _partner;
        private ObservableCollection<ProjectItemViewModel> _projects;
        public ProjectsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Projects";
        }
        public ObservableCollection<ProjectItemViewModel> Projects
        {
            get => _projects;
            set => SetProperty(ref _projects, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("partner"))
            {
                _partner = parameters.GetValue<PartnerResponse>("partner");
                Title = $"Proyectos de: {_partner.FullName}";
                Projects = new ObservableCollection<ProjectItemViewModel>(_partner.Projects.Select(p => new ProjectItemViewModel(_navigationService)
                {
                    Active=p.Active,
                    CreationDate=p.CreationDate,
                    EndDate=p.EndDate,
                    Name=p.Name,
                    ProjectRemarks=p.ProjectRemarks,
                    UserProjects=p.UserProjects,
                }).ToList());



            }
        }

    }
}
