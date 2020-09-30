using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Infrastructure.Data.EntityFramework.Repositories
{
    public class PetColorRepository : IPetColorRepository
    {
        private readonly PetShopDbContext _context;

        public PetColorRepository(PetShopDbContext context)
        {
            _context = context;
        }


        public PetColor AddPetColor(PetColor petColorToAdd)
        {
            _context.Attach(petColorToAdd).State = EntityState.Added;
            _context.SaveChanges();
            return petColorToAdd;
        }

        public List<PetColor> GetAllPetColors()
        {
            return _context.PetColors.ToList();
        }
    }
}