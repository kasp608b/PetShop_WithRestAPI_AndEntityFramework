using System;
using System.Collections.Generic;
using System.Text;
using PetShop.Core.Entities.Entities;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;

namespace PetShop.Core.DomainService
{
    public interface IOwnerRepository
    {
        public List<Owner> GetAllOwners();

        public FilteredList<Owner> GetAllOwnersFiltered(Filter filter);

        public Owner AddOwner(Owner ownerToAdd);

        public Owner DeleteOwner(Owner ownerToDelete);

        public Owner EditOwner(int id, Owner editedOwner);

        public Owner SearchById(int id);
    }
}
