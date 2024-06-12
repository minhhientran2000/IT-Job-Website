using Api.Models.Chat;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Reflection;

namespace Api.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) 
        {            
        }
        public DbSet<JobList> jobLists { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<Category> categories { get; set; }       
        public DbSet<TypeWork> typeWorks { get; set; }
        public DbSet<LevelOfWork> levelOfWorks { get; set; }
        public DbSet<Seeker> seekers { get; set; }
        public DbSet<Application> applications { get; set; }
        public DbSet<ApplicationStatus> applicationStatuses { get; set; }
        public DbSet<Message> messages { get; set; }
        public DbSet<GroupChat> groupChats { get; set; }
        public DbSet<Location> locations { get; set; }


    }
}
