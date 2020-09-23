using Microsoft.EntityFrameworkCore;
using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Infrastructure.Data.EntityFramework
{
    public class PetShopDbContext : DbContext
    {
        public PetShopDbContext(DbContextOptions<PetShopDbContext> opt) : base(opt) { }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<PetType> PetTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
