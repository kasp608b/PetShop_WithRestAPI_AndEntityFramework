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
        private IParser _parser;

        public PetTypeService(IPetTypeRepository petTypeRepository, IParser parser)
        {
            _petTypeRepository = petTypeRepository;
            _parser = parser;
        }

        public FilteredList<PetType> GetPetTypes(Filter filter)
        {
            FilteredList<PetType> filteredPetTypes;

            if (!string.IsNullOrEmpty(filter.SearchText) && string.IsNullOrEmpty(filter.SearchField))
            {
                filter.SearchField = "Name";
            }

            filteredPetTypes = _petTypeRepository.GetAllPetTypesFiltered(filter);

            if (filteredPetTypes.List.Count < 1)
            {
                throw new KeyNotFoundException("Could not find petTypes that satisfy the filter");
            }

            return _petTypeRepository.GetAllPetTypesFiltered(filter);
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
            if (!_petTypeRepository.GetAllPetTypes().Exists(x => x.ID == id))
            {
                throw new KeyNotFoundException("A pet with this ID does not exist");
            }
            else
            {
                petTypeToDelete = _petTypeRepository.GetAllPetTypes().Find(x => x.ID == id);
                return _petTypeRepository.DeletePetType(petTypeToDelete);
            }
        }

        public PetType EditPetType(int idOfPetTypeToEdit, PetType editedPetType)
        {
            if (!_petTypeRepository.GetAllPetTypes().Exists(x => x.ID == idOfPetTypeToEdit))
            {
                throw new KeyNotFoundException("A pet with this ID does not exist");
            }
            else
            {
                return _petTypeRepository.EditPetType(idOfPetTypeToEdit, editedPetType);
            }
        }

        public PetType SearchById(int id)
        {
            if (!_petTypeRepository.GetAllPetTypes().Exists(x => x.ID == id))
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