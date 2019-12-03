using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsAgenda.Web.Data;
using ProjectsAgenda.Web.Data.Entities;
using ProjectsAgenda.Web.Helpers;
using ProjectsAgenda.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectsAgenda.Web.Controllers
{
    [Authorize(Roles = "Partner")]
    public class PartnersController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IKombosHelper _kombosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IMailHelper _mailHelper;

        public PartnersController(
            DataContext context,
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

              


        // GET: Partners
        public IActionResult Index()
        {
            return View(_dataContext.Partners
                .Include(o => o.User)
                .Include(q => q.Projects));
        }


        // GET: Partners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await _dataContext.Partners
                .Include(o => o.User)
                .Include(o => o.Projects)
                .ThenInclude(p => p.ProjectRemarks)
                .ThenInclude(p2 => p2.Partner)
                .ThenInclude(p3 => p3.User)
                .Include(q => q.Projects)
                .ThenInclude(r => r.UserProjects)
                .ThenInclude(r2 => r2.Partner)
                .ThenInclude(r3 => r3.User)

                                .FirstOrDefaultAsync(o => o.Id == id);
            if (partner == null)
            {
                return NotFound();
            }

            return View(partner);
        }

        // GET: Partners/Create
        public IActionResult Create()
        {
            var view = new AddUserViewModel { RoleId = 2 };
            return View(view);
        }



        // POST: Partners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await CreateUserAsync(model);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este Email ya está en uso.");
                    return View(model);
                }

                var partner = new Partner
                {
                    Projects = new List<Project>(),
                    User = user,
                };

                _dataContext.Partners.Add(partner);
                await _dataContext.SaveChangesAsync();

                var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                var tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                _mailHelper.SendMail(model.Username, "MyLeasing - Email confirmation", $"<h1>MyLeasing - Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");


                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        private async Task<User> CreateUserAsync(AddUserViewModel model)
        {
            var user = new User
            {
                Email = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username
            };

            var result = await _userHelper.AddUserAsync(user, model.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }
            var newUser = await _userHelper.GetUserByEmailAsync(model.Username);
            await _userHelper.AddUserToRoleAsync(newUser, "Partner");
            return newUser;
        }


        // GET: Partner/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await _dataContext.Partners
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (partner == null)
            {
                return NotFound();
            }

            var view = new EditUserViewModel
            {
               
                FirstName = partner.User.FirstName,
                Id = partner.Id,
                LastName = partner.User.LastName,
                PhoneNumber = partner.User.PhoneNumber
            };

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var partner = await _dataContext.Partners
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.Id == model.Id);


                partner.User.FirstName = model.FirstName;
                partner.User.LastName = model.LastName;

                partner.User.PhoneNumber = model.PhoneNumber;

                await _userHelper.UpdateUserAsync(partner.User);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Partners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await _dataContext.Partners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partner == null)
            {
                return NotFound();
            }

            return View(partner);
        }

        // POST: Partners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partner = await _dataContext.Partners.FindAsync(id);
            _dataContext.Partners.Remove(partner);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartnerExists(int id)
        {
            return _dataContext.Partners.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AddProject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await _dataContext.Partners.FindAsync(id.Value);
            if (partner == null)
            {
                return NotFound();
            }

            var model = new ProjectViewModel
            {
                PartnerId = partner.Id,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var project = await _converterHelper.ToProjectAsync(model, true);
                _dataContext.Projects.Add(project);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(Details)}/{model.PartnerId}");
            }
            return View(model);
        }

        public async Task<IActionResult> EditProject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _dataContext.Projects
                .Include(p => p.Partner)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToProjectViewModel(project);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProject(ProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var project = await _converterHelper.ToProjectAsync(model, false);
                _dataContext.Projects.Update(project);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(Details)}/{ model.PartnerId}");
            }
            return View(model);
        }

        public async Task<IActionResult> DetailsProject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var project = await _dataContext.Projects
                .Include(o => o.Partner)
                .ThenInclude(p => p.User)
                .Include(o => o.ProjectRemarks)
                .ThenInclude(o2 => o2.Partner)
                .ThenInclude(o3 => o3.User)
                .Include(r => r.UserProjects)
                .ThenInclude(r2 => r2.Partner)
                .ThenInclude(r3 => r3.User)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        public async Task<IActionResult> AddPartner(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _dataContext.Projects
                .Include(p => p.Partner)
                .FirstOrDefaultAsync(p => p.Id == id.Value);
            if (project == null)
            {
                return NotFound();
            }

            var model = new PartnerViewModel
            {
                ProjectId = project.Id,
                Partners = _kombosHelper.GetComboPartners(),
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddPartner(PartnerViewModel model)
        {
            if (ModelState.IsValid)
            {

                var project = await _dataContext.Projects              
               .FirstOrDefaultAsync(p => p.Id == model.ProjectId);
                var partner = await _dataContext.Partners
               .FirstOrDefaultAsync(p => p.Id == model.PartnerId);

                var userProject = new UserProject
                {
                    Active=true,
                    AddDate=DateTime.Now,
                    IdAdmin = model.PartnerId,
                    Partner = partner,
                    Project =project,
                };
                _dataContext.UserProjects.Add(userProject);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsProject)}/{model.ProjectId}");
            }
            model.Partners = _kombosHelper.GetComboPartners();
            return View(model);
        }

        public async Task<IActionResult> DeleteUserProject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProject = await _dataContext.UserProjects
                .Include(p => p.Project)
                .FirstOrDefaultAsync(pi => pi.Id == id.Value);
            if (userProject == null)
            {
                return NotFound();
            }

            _dataContext.UserProjects.Remove(userProject);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(DetailsProject)}/{userProject.Project.Id}");
        }

        public async Task<IActionResult> DeleteProject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _dataContext.Projects
                .Include(p => p.Partner)
                .FirstOrDefaultAsync(pi => pi.Id == id.Value);
            if (project == null)
            {
                return NotFound();
            }

            _dataContext.Projects.Remove(project);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(Details)}/{project.Partner.Id}");
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
             Id=project.Id,
             Project=project,
             Date=DateTime.Now,
             Partner=partner,
             Remark="",
             ProjectId= project.Id,
             PartnerId= partner.Id,
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
                    Date=model.Date,
                    Remark = model.Remark,
                    ImageUrl = path,
                    ExpirationDate = model.ExpirationDate,
                    Partner = await _dataContext.Partners.FindAsync(model.PartnerId),
                    Project = await _dataContext.Projects.FindAsync(model.ProjectId)
                };

                _dataContext.ProjectRemarks.Add(projectRemark);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsProject)}/{model.Id}");
            }
            return View(model);
        }
    }
}