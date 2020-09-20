using PetShop.Core.Entities;
using PetShop.Core.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;

namespace PetShop.Core.ApplicationService
{
    public interface IPetService
    {

        public FilteredList<Pet> GetPets(Filter filter);

        public List<Pet> GetPets();

        public Pet AddPet(Pet pet);

        public Pet DeletePet(int id);

        public Pet EditPet(int idOfPetToEdit, Pet editedPet);

        public Pet SearchById(int id);
    }
}
