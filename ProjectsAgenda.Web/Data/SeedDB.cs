using System;
using System.Linq;
using System.Threading.Tasks;
using ProjectsAgenda.Web.Data.Entities;
using ProjectsAgenda.Web.Helpers;

namespace ProjectsAgenda.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(
            DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRoles();
            var manager = await CheckUserAsync("Luis", "Nuñez", "luisalbertonu@gmail.com", "Manager");
            var partner = await CheckUserAsync("Luis", "Nuñez", "luis.albiazul@hotmail.com","Partner");
            
            await CheckManagerAsync(manager);
            await CheckPartnersAsync(partner);
            await CheckProjectsAsync();
        }
        private async Task CheckManagerAsync(User user)
        {
            if (!_context.Managers.Any())
            {
                _context.Managers.Add(new Manager { User = user });
                await _context.SaveChangesAsync();
            }
        }
        private async Task<User> CheckUserAsync(string firstName, string lastName, string email,  string role)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                   
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, role);
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }
            return user;
        }

        private async Task CheckRoles()
        {
            await _userHelper.CheckRoleAsync("Manager");
            await _userHelper.CheckRoleAsync("Partner");
        }
        private async Task CheckProjectsAsync()
        {
            var partner = _context.Partners.FirstOrDefault();
            
            if (!_context.Projects.Any())
            {
                AddProject("Proyecto 1");
                AddProject("Proyecto 2");
                await _context.SaveChangesAsync();
            }
        }
        private async Task CheckPartnersAsync(User user)
        {
            if (!_context.Partners.Any())
            {
                _context.Partners.Add(new Partner { User = user });
                await _context.SaveChangesAsync();
            }
        }
        private void AddProject(string name)
        {
            var partner = _context.Partners.FirstOrDefault();
            _context.Projects.Add(new Project
            {
                Name=name,
                CreationDate=DateTime.Now,
                EndDate = DateTime.Now,
                Active=true,
                Partner= partner,
            });
        }
    }
}