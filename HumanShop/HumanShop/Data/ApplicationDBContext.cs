using HumanShop.Entities;
using Microsoft.EntityFrameworkCore;

namespace HumanShop.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<AppUser> appUsers { get; set; }
    }
}
