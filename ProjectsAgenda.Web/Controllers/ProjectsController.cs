using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsAgenda.Web.Data;
using ProjectsAgenda.Web.Data.Entities;
using ProjectsAgenda.Web.Helpers;
using ProjectsAgenda.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectsAgenda.Web.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IKombosHelper _kombosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IMailHelper _mailHelper;

        public ProjectsController(DataContext context,
            IUserHelper userHelper,
            IConverterHelper converterHelper,
            IImageHelper imageHelper,
            IMailHelper mailHelper,
            IKombosHelper kombosHelper)
        {
            _dataContext = context;
            _userHelper = userHelper;

            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
            _mailHelper = mailHelper;
            _kombosHelper = kombosHelper;
        }

        // GET: Projects

        public  IActionResult Index()
        {
            return View(_dataContext.UserProjects
                .Include(c => c.Partner)
                .ThenInclude(o => o.User)
                .Include(c => c.Project)
                .ThenInclude(l => l.Partner)
                .ThenInclude(l => l.User)
                .Where(c => c.Partner.User.UserName.ToLower().Equals(User.Identity.Name.ToLower())));
        }





        // GET: Partners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _dataContext.Projects
                .Include(o => o.Partner)
                .ThenInclude(o => o.User)

                .Include(p => p.ProjectRemarks)
                .ThenInclude(p2 => p2.Partner)
                .ThenInclude(p3 => p3.User)

                .Include(q => q.UserProjects)
                .ThenInclude(q2 => q2.Partner)
                .ThenInclude(q3 => q3.User)

                                .FirstOrDefaultAsync(o => o.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        public async Task<IActionResult> AddRemark(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _dataContext.Projects
                .Include(p => p.Partner)
                .FirstOrDefaultAsync(p => p.Id == id.Value);


            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            var partId = await _dataContext.Partners
                .FirstOrDefaultAsync(p => p.User.Id == user.Id);





            var partner = await _dataContext.Partners
                .FirstOrDefaultAsync(p => p.Id == partId.Id);


            if (project == null)
            {
                return NotFound();
            }

            var model = new ProjectRemarkViewModel
            {
                Id = project.Id,
                Project = project,
                Date = DateTime.Now,
                Partner = partner,
                Remark = "",
                ProjectId = project.Id,
                PartnerId = partner.Id,
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddRemark(ProjectRemarkViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile);
                }

                var projectRemark = new ProjectRemark
                {
                    Date = model.Date,
                    Remark = model.Remark,
                    ImageUrl = path,
                    ExpirationDate = model.ExpirationDate,
                    Partner = await _dataContext.Partners.FindAsync(model.PartnerId),
                    Project = await _dataContext.Projects.FindAsync(model.ProjectId)
                };

                _dataContext.ProjectRemarks.Add(projectRemark);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(Details)}/{model.Id}");
            }
            return View(model);
        }
        public async Task<IActionResult> DeleteRemark(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectRemark = await _dataContext.ProjectRemarks
                .Include(p => p.Project)
                .Include(p=>p.Partner)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(pi => pi.Id == id.Value);
            
            
            if (projectRemark == null)
            {
                return NotFound();
            }


            if (projectRemark.Partner.User.UserName.ToLower() != User.Identity.Name.ToLower())
            {
                return NotFound();
            }



            _dataContext.ProjectRemarks.Remove(projectRemark);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(Details)}/{projectRemark.Project.Id}");
        }
    }
}
