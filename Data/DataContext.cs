using datingAppreal.Entities;
using Microsoft.EntityFrameworkCore;

namespace datingAppreal.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
       public DbSet<User> User { get; set; }    
    }
}
