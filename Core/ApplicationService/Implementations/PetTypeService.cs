using System.Collections.Generic;
using System.IO;
using PetShop.Core.ApplicationService.Interfaces;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;
using PetShop.Core.Entities.Exceptions;
using PetShop.Core.HelperClasses.Interfaces;

namespace PetShop.Core.ApplicationService.Implementations
{
    public class PetTypeService : IPetTypeService
    {
        private IPetTypeRepository _petTypeRepository;

        public PetTypeService(IPetTypeRepository petTypeRepository)
        {
            _petTypeRepository = petTypeRepository;
        }

        public FilteredList<PetType> GetPetTypes(Filter filter)
        {
            FilteredList<PetType> filteredPetTypes;

            if (!string.IsNullOrEmpty(filter.SearchText) && string.IsNullOrEmpty(filter.SearchField))
            {
                filter.SearchField = "Name";
            }

            if (filter.CurrentPage < 0 || filter.ItemsPrPage < 0)
            {
                throw new InvalidDataException("CurrentPage and ItemsPrPage must be zero or more");
            }

            if ((filter.CurrentPage - 1 * filter.ItemsPrPage) >= _petTypeRepository.GetAllPetTypes().Count)
            {
                throw new InvalidDataException("Index out of bounds, CurrentPage is to high");
            }

            filteredPetTypes = _petTypeRepository.GetAllPetTypesFiltered(filter);

            if (filteredPetTypes.List.Count < 1)
            {
                throw new KeyNotFoundException("Could not find petTypes that satisfy the filter");
            }

            return filteredPetTypes;
        }

        public PetType AddPetType(PetType petType)
        {
            PetType addedPetType;

            if (petType.Equals(null))
            {
                throw new InvalidDataException("Pet cannot be null");
            }

            if (petType.Name.Length < 1)
            {
                throw new InvalidDataException("Pet name has to be longer than one");
            }

            if (petType.PetTypeId != 0)
            {
                throw new InvalidDataException("A new petType cannot have an id, that is only for already existing pets");
            }

            if (petType.Pets != null)
            {
                if (petType.Pets.Count > 0)
                {
                    throw new InvalidDataException("You cant add pets to a petType like this, go ad an owner id to a pet instead");
                }
            }

            addedPetType = _petTypeRepository.AddPetType(petType);
            if (addedPetType == null)
            {
                throw new DataBaseException("Something went wrong in the database");
            }

            return addedPetType;
        }

        public PetType DeletePetType(int id)
        {
            PetType petTypeToDelete;
            if (!_petTypeRepository.GetAllPetTypes().Exists(x => x.PetTypeId == id))
            {
                throw new KeyNotFoundException("A pet with this ID does not exist");
            }
            else
            {
                petTypeToDelete = _petTypeRepository.GetAllPetTypes().Find(x => x.PetTypeId == id);
                return _petTypeRepository.DeletePetType(petTypeToDelete);
            }
        }

        public PetType EditPetType(int idOfPetTypeToEdit, PetType editedPetType)
        {
            if (_petTypeRepository.SearchById(editedPetType.PetTypeId) == null)
            {
                throw new KeyNotFoundException("An petType with this ID does not exist");
            }

            if (editedPetType.Pets != null)
            {
                if (editedPetType.Pets.Count > 0)
                {
                    throw new InvalidDataException("You cant add pets to a petType like this, go ad an owner id to a pet instead");
                }
            }

            return _petTypeRepository.EditPetType(idOfPetTypeToEdit, editedPetType);
            
        }

        public PetType SearchById(int id)
        {
            if (!_petTypeRepository.GetAllPetTypes().Exists(x => x.PetTypeId == id))
            {
                throw new KeyNotFoundException("No pets with this id exist");
            }
            else
            {
                return _petTypeRepository.SearchById(id);
            }
        }
    }
}