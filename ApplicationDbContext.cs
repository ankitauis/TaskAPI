using Microsoft.EntityFrameworkCore;
using TaskAPI.Models;

namespace TaskAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Models.TaskEntity> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
