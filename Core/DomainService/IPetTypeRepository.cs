using System.Collections.Generic;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;

namespace PetShop.Core.DomainService
{
    public interface IPetTypeRepository
    {
        public List<PetType> GetAllPetTypes();

        public FilteredList<PetType> GetAllPetTypesFiltered(Filter filter);

        public PetType AddPetType(PetType petTypeToAdd);

        public PetType DeletePetType(PetType petTypeToDelete);

        public PetType EditPetType(int id, PetType editedPetType);

        public PetType SearchById(int id);
    }
}