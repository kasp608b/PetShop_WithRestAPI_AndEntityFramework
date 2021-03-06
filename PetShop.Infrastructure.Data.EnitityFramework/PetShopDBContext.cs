﻿using Microsoft.EntityFrameworkCore;
using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Infrastructure.Data.EntityFramework
{
    public class PetShopDbContext : DbContext
    {
        public PetShopDbContext(DbContextOptions<PetShopDbContext> opt) : base(opt) { }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<PetType> PetTypes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<PetColor> PetColors { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pet>()
                .HasOne(pet => pet.Owner)
                .WithMany(owner => owner.Pets)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Pet>()
                .HasOne(pet => pet.PetType)
                .WithMany(petType => petType.Pets)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PetColor>()
                .HasKey(petColor => new {petColor.PetId, petColor.ColorId});

            modelBuilder.Entity<PetColor>()
                .HasOne(petColor => petColor.Color)
                .WithMany(color => color.PetColors)
                .HasForeignKey(petColor => petColor.ColorId);

            modelBuilder.Entity<PetColor>()
                .HasOne(petColor => petColor.Pet)
                .WithMany(pet => pet.PetColors )
                .HasForeignKey(petColor => petColor.PetId);

        }
    }
}
