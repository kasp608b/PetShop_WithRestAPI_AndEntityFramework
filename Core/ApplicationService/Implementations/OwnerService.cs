using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PetShop.Core.ApplicationService.Interfaces;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;
using PetShop.Core.Entities.Exceptions;

namespace PetShop.Core.ApplicationService.Implementations
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;

        public OwnerService(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }


        public FilteredList<Owner> GetOwners(Filter filter)
        {
            FilteredList<Owner> filteredOwners;

            if (!string.IsNullOrEmpty(filter.SearchText) && string.IsNullOrEmpty(filter.SearchField))
            {
                filter.SearchField = "Name";
            }

            if (filter.CurrentPage < 0 || filter.ItemsPrPage < 0)
            {
                throw new InvalidDataException("CurrentPage and ItemsPrPage must be zero or more");
            }

            if ((filter.CurrentPage - 1 * filter.ItemsPrPage) >= _ownerRepository.GetAllOwners().Count)
            {
                throw new InvalidDataException("Index out of bounds, CurrentPage is to high");
            }

            filteredOwners = _ownerRepository.GetAllOwnersFiltered(filter);

            if (filteredOwners.List.Count < 1)
            {
                throw new KeyNotFoundException("Could not find owners that satisfy the filter");
            }

            return filteredOwners;
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


            if (owner.Id != 0)
            {
                throw new InvalidDataException("A new owner cannot have an id, that is only for already existing pets");
            }

            if (owner.Pets != null)
            {
                if (owner.Pets.Count < 0)
                {
                    throw new InvalidDataException("You cant add pets to an owner like this, go ad an owner id to a pet instead");
                }
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
            if (!_ownerRepository.GetAllOwners().Exists(x => x.Id == id))
            {
                throw new KeyNotFoundException("An owner with this ID does not exist");
            }
            else
            {
                ownerToDelete = _ownerRepository.GetAllOwners().Find(x => x.Id == id);
                return _ownerRepository.DeleteOwner(ownerToDelete);
            }
        }

        public Owner EditOwner(int idOfOwnerToEdit, Owner editedOwner)
        {
            if (_ownerRepository.SearchById(editedOwner.Id) == null)
            {
                throw new KeyNotFoundException("An owner with this ID does not exist");
            }

            if (editedOwner.Pets != null)
            {
                if (editedOwner.Pets.Count < 0)
                {
                    throw new InvalidDataException("You cant add pets to an owner like this, go ad an owner id to a pet instead");
                }
            }

            return _ownerRepository.EditOwner(idOfOwnerToEdit, editedOwner);
            
        }

        public Owner SearchById(int id)
        {
            
            if (!_ownerRepository.GetAllOwners().Exists(x => x.Id == id))
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
