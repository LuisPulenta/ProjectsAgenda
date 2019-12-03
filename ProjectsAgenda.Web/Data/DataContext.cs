using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectsAgenda.Web.Data.Entities;
namespace ProjectsAgenda.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {

        #region Constructor
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        #endregion

        public DbSet<Partner> Partners { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectRemark> ProjectRemarks { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<Manager> Managers { get; set; }

    }
}
