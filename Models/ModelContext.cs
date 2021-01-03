using Microsoft.EntityFrameworkCore;

namespace IdentityExampleNetCore.Models
{
    public class ModelContext : DbContext
    {

        public ModelContext()
        {
            
        }
        public ModelContext(DbContextOptions opt) : base(opt) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=IdentityTest;userid=root;password=toor1212");
        }


        public DbSet<User> Users { get; set; }

    }
}