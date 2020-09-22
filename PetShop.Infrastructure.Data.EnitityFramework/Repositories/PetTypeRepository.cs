using System;
using System.Collections.Generic;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;

namespace PetShop.Infrastructure.Data.EnitityFramework.Repositories
{
    public class PetTypeRepository : IPetTypeRepository
    {
        public List<PetType> GetAllPetTypes()
        {
            throw new NotImplementedException();
        }

        public FilteredList<PetType> GetAllPetTypesFiltered(Filter filter)
        {
            throw new NotImplementedException();
        }

        public PetType AddPetType(PetType petTypeToAdd)
        {
            throw new NotImplementedException();
        }

        public PetType DeletePetType(PetType petTypeToDelete)
        {
            throw new NotImplementedException();
        }

        public PetType EditPetType(int id, PetType editedPetType)
        {
            throw new NotImplementedException();
        }

        public PetType SearchById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
