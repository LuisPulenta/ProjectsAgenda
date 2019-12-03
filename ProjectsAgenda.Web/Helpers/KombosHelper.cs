using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectsAgenda.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace ProjectsAgenda.Web.Helpers
{
    public class KombosHelper : IKombosHelper
    {
        private readonly DataContext _dataContext;
        public KombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboPartners()
        {
            var list = _dataContext.Partners.Select(pt => new SelectListItem
            {
                Text = pt.User.FullName,
                Value = $"{pt.Id}"
            }).OrderBy(p => p.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Elija Socio...)",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboRoles()
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "(Elija un Rol...)" },
                new SelectListItem { Value = "1", Text = "Manager" },
                new SelectListItem { Value = "2", Text = "Partner" }
            };

            return list;
        }
    }
}
