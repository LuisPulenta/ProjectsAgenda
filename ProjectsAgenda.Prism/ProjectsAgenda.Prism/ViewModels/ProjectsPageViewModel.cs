using Prism.Navigation;
using ProjectsAgenda.Common.Models;

namespace ProjectsAgenda.Prism.ViewModels
{
    public class ProjectsPageViewModel : ViewModelBase
    {

        private PartnerResponse _partner;
        public ProjectsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Projects";
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("partner"))
            {
                _partner = parameters.GetValue<PartnerResponse>("partner");
                Title = $"Proyectos de: {_partner.FullName}";
            }
        }

    }
}
