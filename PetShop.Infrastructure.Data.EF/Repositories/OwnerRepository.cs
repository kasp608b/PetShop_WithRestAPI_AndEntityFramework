using System;
using System.Collections.Generic;
using System.Text;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;

namespace PetShop.Infrastructure.Data.EF.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        public List<Owner> GetAllOwners()
        {
            throw new NotImplementedException();
        }

        public FilteredList<Owner> GetAllOwnersFiltered(Filter filter)
        {
            throw new NotImplementedException();
        }

        public Owner AddOwner(Owner ownerToAdd)
        {
            throw new NotImplementedException();
        }

        public Owner DeleteOwner(Owner ownerToDelete)
        {
            throw new NotImplementedException();
        }

        public Owner EditOwner(int id, Owner editedOwner)
        {
            throw new NotImplementedException();
        }

        public Owner SearchById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
