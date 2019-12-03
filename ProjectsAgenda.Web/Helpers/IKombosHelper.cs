using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectsAgenda.Web.Helpers
{
    public interface IKombosHelper
    {
        IEnumerable<SelectListItem> GetComboPartners();
        IEnumerable<SelectListItem> GetComboRoles();
    }
}
