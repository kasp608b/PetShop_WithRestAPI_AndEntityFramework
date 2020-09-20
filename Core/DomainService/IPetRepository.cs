using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using PetShop.Core.Entities.Entities;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;

namespace PetShop.Core.DomainService
{
    public interface IPetRepository
    {
        public List<Pet> GetAllPets();

        public FilteredList<Pet> GetAllPetsFiltered(Filter filter);

        public Pet AddPet(Pet petToAdd);

        public Pet DeletePet(Pet petToDelete);

        public Pet EditPet(int id, Pet editedPet);

        public Pet SearchById(int id);


    }

}
