using ProjectsAgenda.Common.Models;
using Prism.Commands;
using Prism.Navigation;
using MyLeasing.Common.Helpers;
using Newtonsoft.Json;

namespace ProjectsAgenda.Prism.ViewModels
{
    public class ProjectItemViewModel : ProjectResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectProjectCommand;
        public ProjectItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public DelegateCommand SelectProjectCommand => _selectProjectCommand ?? (_selectProjectCommand = new DelegateCommand(SelectProject));

        private async void SelectProject()
        {
            //var parameters = new NavigationParameters
            //{
            //    { "project", this }
            //};
            Settings.Project = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("ProjectPage");
        }
    }
}
