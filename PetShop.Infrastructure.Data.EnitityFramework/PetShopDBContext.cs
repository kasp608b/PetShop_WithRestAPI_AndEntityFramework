using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Infrastructure.Data.EF
{
    public class PetShopDBContext : DbContext
    {
        public PetShopDBContext(DbContextOptions<PetShopDBContext> opt) : base(opt) { }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<PetType> PetTypes { get; set; }
    }
}
