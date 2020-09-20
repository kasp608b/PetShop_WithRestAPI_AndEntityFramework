using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using PetShop.Core.Entities.Entities;
using PetShop.Core.HelperClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;
using PetShop.Core.Entities.Exceptions;

namespace PetShop.Core.ApplicationService.Implementations
{
    public class PetService : IPetService
    {
        private IPetRepository _petRepository;
        private IParser _parser;

        public PetService(IPetRepository petRepository, IParser parser)
        {
            _petRepository = petRepository;
            _parser = parser;
        }

        public List<Pet> GetPets()
        {
            return _petRepository.GetAllPets();
        }

        public Pet AddPet(Pet pet)
        {
            Pet addedPet;

            if(pet.Equals(null))
            {
                throw new InvalidDataException("Pet cannot be null");
            }

            if(pet.Name.Length < 1)
            {
                throw new InvalidDataException("Pet name has to be longer than one");
            }

            addedPet = _petRepository.AddPet(pet);
            if (addedPet == null)
            {
                throw new DataBaseException("Something went wrong in the database");
            }

            return addedPet;
        }

        public Pet DeletePet(int id)
        {
           Pet petToDelete;
           if(!_petRepository.GetAllPets().Exists(x => x.ID == id))
           {
                throw new KeyNotFoundException("A pet with this ID does not exist");
           }
           else 
           {
                petToDelete = _petRepository.GetAllPets().Find(x => x.ID == id);
                return _petRepository.DeletePet(petToDelete);
           }

        }

        public Pet EditPet(int idOfPetToEdit, Pet editedPet)
        {
            if (!_petRepository.GetAllPets().Exists(x => x.ID == idOfPetToEdit))
            {
                throw new KeyNotFoundException("A pet with this ID does not exist");
            }
            else
            {
                return _petRepository.EditPet(idOfPetToEdit, editedPet);
            }
            
        }

        public FilteredList<Pet> GetPets(Filter filter)
        {
            FilteredList<Pet> filteredPets;

            if (!string.IsNullOrEmpty(filter.SearchText) && string.IsNullOrEmpty(filter.SearchField))
            {
                filter.SearchField = "Name";
            }

            filteredPets = _petRepository.GetAllPetsFiltered(filter);

            if (filteredPets.List.Count < 1)
            {
                throw new KeyNotFoundException("Could not find pets that satisfy the filter");
            }

            return _petRepository.GetAllPetsFiltered(filter);
        }


        public Pet SearchById(int id)
        {
            if (!_petRepository.GetAllPets().Exists(x => x.ID == id))
            {
                throw new KeyNotFoundException("No pets with this id exist");
            }
            else
            {
                return _petRepository.SearchById(id);
            }
        }

    }
}
