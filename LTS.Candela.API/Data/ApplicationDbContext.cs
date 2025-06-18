using Microsoft.EntityFrameworkCore;
using LTS.Candela.API.Models;

namespace LTS.Candela.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
