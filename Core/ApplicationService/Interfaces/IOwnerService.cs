using PetShop.Core.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;

namespace PetShop.Core.ApplicationService.Interfaces
{
    public interface IOwnerService
    {
        public FilteredList<Owner> GetOwners(Filter filter);

        public Owner AddOwner(Owner owner);

        public Owner DeleteOwner(int id);

        public Owner EditOwner(int idOfOwnerToEdit, Owner editedOwner);

        public Owner SearchById(int id);
    }
}
