using MyLeasing.Common.Helpers;
using Newtonsoft.Json;
using Prism.Navigation;
using ProjectsAgenda.Common.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProjectsAgenda.Prism.ViewModels
{
    public class ProjectPageViewModel : ViewModelBase
    {
        private ProjectResponse _project;


        private readonly INavigationService _navigationService;
        private ObservableCollection<ProjectRemarkResponse> _projectRemarks;
        

        public ProjectPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Details";
            Project = JsonConvert.DeserializeObject<ProjectResponse>(Settings.Project);

            _navigationService = navigationService;
        }
        public ObservableCollection<ProjectRemarkResponse> ProjectRemarks
        {
            get => _projectRemarks;
            set => SetProperty(ref _projectRemarks, value);
        }


        public ProjectResponse Project
        {
            get => _project;
            set => SetProperty(ref _project, value);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            Project = JsonConvert.DeserializeObject<ProjectResponse>(Settings.Project);
            ProjectRemarks = new ObservableCollection<ProjectRemarkResponse>(Project.ProjectRemarks.Select(p => new ProjectRemarkResponse()
            {
                Date=p.Date,
                ExpirationDate=p.ExpirationDate,
                ImageUrl=p.ImageUrl,
                Id=p.Id,
                Remark=p.Remark,
                Partner=p.Partner,
            }).ToList());
        }



    }
}