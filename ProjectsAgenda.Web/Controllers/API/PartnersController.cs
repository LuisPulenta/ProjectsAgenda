using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsAgenda.Common.Models;
using ProjectsAgenda.Web.Data;
using ProjectsAgenda.Web.Data.Entities;
using ProjectsAgenda.Web.Helpers;

namespace ProjectsAgenda.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PartnersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;

        public PartnersController(
            DataContext dataContext,
            IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
        }

        [HttpPost]
        [Route("GetPartnerByEmail")]
        public async Task<IActionResult> GetPartner(EmailRequest emailRequest)
        {
            try
            {
                var user = await _userHelper.GetUserByEmailAsync(emailRequest.Email);
                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                if (await _userHelper.IsUserInRoleAsync(user, "Partner"))
                {
                    return await GetPartnerAsync(emailRequest);
                }
                else
                {
                    return await GetPartnerAsync(emailRequest);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        private async Task<IActionResult> GetPartnerAsync(EmailRequest emailRequest)
        {
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
                .FirstOrDefaultAsync(o => o.User.UserName.ToLower().Equals(emailRequest.Email.ToLower()));

            var userprojects = await _dataContext.UserProjects
               .Include(o => o.Partner)
               .ThenInclude(o => o.User)
               .Include(o => o.Project)
               .ThenInclude(o => o.Partner)
               .ThenInclude(o => o.User)
               .Include(o => o.Project)
               .ThenInclude(o => o.ProjectRemarks)
               .ThenInclude(o => o.Partner)
               .ThenInclude(o => o.User)
               .Where(o => o.Partner.User.UserName.ToLower().Equals(emailRequest.Email.ToLower())).ToListAsync();

            var response = new PartnerResponse
            {
                Id = partner.Id,
                FirstName = partner.User.FirstName,
                LastName = partner.User.LastName,
                PhoneNumber = partner.User.PhoneNumber,
                Email = partner.User.Email,

                Projects = userprojects?.Select(p => new ProjectResponse
                {
                    Id = p.Project.Id,
                    Name = p.Project.Name,
                    CreationDate = p.Project.CreationDate,
                    EndDate = p.Project.EndDate,
                    Active = p.Project.Active,

                    UserProjects = p.Project.UserProjects?.Select(c => new UserProjectResponse
                    {
                        Id = c.Id,
                        Active = c.Active,
                        IdAdmin = c.IdAdmin,
                        AddDate = c.AddDate,
                        Partner =ToPartnerResponse(c.Partner),
                    }).ToList(),


                    ProjectRemarks = p.Project.ProjectRemarks?.Select(c => new ProjectRemarkResponse
                    {
                        Id = c.Id,
                        Date = c.Date,
                        ExpirationDate = c.ExpirationDate,
                        ImageUrl = c.ImageUrl,
                        Remark = c.Remark,
                        Partner = ToPartnerResponse(c.Partner),
                    }).ToList(),

                }).ToList()


            };
            return Ok(response);
        }


        private PartnerResponse ToPartnerResponse(Partner partner)
        {
            return new PartnerResponse
            {
                Email = partner.User.Email,
                FirstName = partner.User.FirstName,
                LastName = partner.User.LastName,
                PhoneNumber = partner.User.PhoneNumber
            };
        }


    }
}
