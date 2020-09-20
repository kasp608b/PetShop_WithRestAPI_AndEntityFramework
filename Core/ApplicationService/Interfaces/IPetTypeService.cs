using System.Collections.Generic;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;

namespace PetShop.Core.ApplicationService.Interfaces
{
    public interface IPetTypeService
    {
        public FilteredList<PetType> GetPetTypes(Filter filter);

        public PetType AddPetType(PetType petType);

        public PetType DeletePetType(int id);

        public PetType EditPetType(int idOfPetTypeToEdit, PetType editedPetType);

        public PetType SearchById(int id);
    }
}