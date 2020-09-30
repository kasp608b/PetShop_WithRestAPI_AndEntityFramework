using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Infrastructure.Data.EntityFramework.Repositories
{
    public class ColorRepository : IColorRepository
    {
        private readonly PetShopDbContext _context;

        public ColorRepository(PetShopDbContext context)
        {
            _context = context;
        }

        public List<Color> GetAllColors()
        {
            return _context.Colors.ToList();
        }

        public Color AddColor(Color colorToAdd)
        {
            _context.Attach(colorToAdd).State = EntityState.Added;
            _context.SaveChanges();
            return colorToAdd;
        }
    }
}