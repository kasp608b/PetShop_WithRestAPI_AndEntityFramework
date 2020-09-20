using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PetShop.Core.ApplicationService.Interfaces;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;
using PetShop.Core.Entities.Exceptions;
using PetShop.Core.HelperClasses.Interfaces;

namespace PetShop.Core.ApplicationService.Implementations
{
    public class OwnerService : IOwnerService
    {
        private IOwnerRepository _ownerRepository;
        private IParser _parser;

        public OwnerService(IOwnerRepository ownerRepository, IParser parser)
        {
            _ownerRepository = ownerRepository;
            _parser = parser;
        }


        public FilteredList<Owner> GetOwners(Filter filter)
        {
            FilteredList<Owner> filteredOwners;

            if (!string.IsNullOrEmpty(filter.SearchText) && string.IsNullOrEmpty(filter.SearchField))
            {
                filter.SearchField = "Name";
            }

            filteredOwners = _ownerRepository.GetAllOwnersFiltered(filter);

            if (filteredOwners.List.Count < 1)
            {
                throw new KeyNotFoundException("Could not find owners that satisfy the filter");
            }

            return _ownerRepository.GetAllOwnersFiltered(filter);
        }

        public Owner AddOwner(Owner owner)
        {
            Owner addedOwner;

            if (owner.Equals(null))
            {
                throw new InvalidDataException("Owner cannot be null");
            }

            if (owner.Name.Length < 1)
            {
                throw new InvalidDataException("Owner name has to be longer than one");
            }

            addedOwner = _ownerRepository.AddOwner(owner);
            if (addedOwner == null)
            {
                throw new DataBaseException("Something went wrong in the database");
            }

            return addedOwner;
        }

        public Owner DeleteOwner(int id)
        {
            Owner ownerToDelete;
            if (!_ownerRepository.GetAllOwners().Exists(x => x.ID == id))
            {
                throw new KeyNotFoundException("An owner with this ID does not exist");
            }
            else
            {
                ownerToDelete = _ownerRepository.GetAllOwners().Find(x => x.ID == id);
                return _ownerRepository.DeleteOwner(ownerToDelete);
            }
        }

        public Owner EditOwner(int idOfOwnerToEdit, Owner editedOwner)
        {
            if (!_ownerRepository.GetAllOwners().Exists(x => x.ID == idOfOwnerToEdit))
            {
                throw new KeyNotFoundException("An owner with this ID does not exist");
            }
            else
            {
                return _ownerRepository.EditOwner(idOfOwnerToEdit, editedOwner);
            }
        }

        public Owner SearchById(int id)
        {
            
            if (!_ownerRepository.GetAllOwners().Exists(x => x.ID == id))
            {
                throw new KeyNotFoundException("No owners with this id exist");
            }
            else
            {
                return _ownerRepository.SearchById(id);
            }
        }
    }
}
