using Microsoft.EntityFrameworkCore;
using Registration.API.Domains;

namespace Registration.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<User>().HasIndex(e => e.EmailAddress).IsUnique(true);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOtp> UsersOtp { get; set; }
    }
}
