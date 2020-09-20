using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;

namespace PetShop.Infrastructure.Data.EF.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly PetShopDBContext _context;
        public PetRepository(PetShopDBContext context)
        {
            _context = context;
        }

        public List<Pet> GetAllPets()
        {
            return _context.Pets.ToList();
        }

        public FilteredList<Pet> GetAllPetsFiltered(Filter filter)
        {
            throw new NotImplementedException();
        }

        public Pet AddPet(Pet petToAdd)
        {
            var addedPet =  _context.Pets.Add(petToAdd).Entity;
            _context.SaveChanges();
            return addedPet;
        }

        public Pet DeletePet(Pet petToDelete)
        {
            throw new NotImplementedException();
        }

        public Pet EditPet(int id, Pet editedPet)
        {
            throw new NotImplementedException();
        }

        public Pet SearchById(int id)
        {
            return _context.Pets.FirstOrDefault(pet => pet.ID == id);
        }
    }
}
