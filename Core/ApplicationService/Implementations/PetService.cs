using PetShop.Core.DomainService;
using System.Collections.Generic;
using System.IO;
using PetShop.Core.ApplicationService.Interfaces;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;
using PetShop.Core.Entities.Exceptions;

namespace PetShop.Core.ApplicationService.Implementations
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;
        private readonly IPetTypeRepository _petTypeRepository;
        private readonly IOwnerRepository _ownerRepository;

        public PetService(IPetRepository petRepository, IPetTypeRepository petTypeRepository, IOwnerRepository ownerRepository)
        {
            _petRepository = petRepository;
            _petTypeRepository = petTypeRepository;
            _ownerRepository = ownerRepository;
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

            if (pet.PetType != null)
            {
                var petType = _petTypeRepository.SearchByIdWithoutRelations(pet.PetType.Id);
                if (petType == null)
                {
                    throw new InvalidDataException("The petType has to be an existing petType in the database");
                }

                pet.PetType = petType;

            }
            else
            {
                throw new InvalidDataException("New Pet has to have a petType");
            }

            if (pet.Owner != null)
            {
                var owner = _ownerRepository.SearchByIdWithoutRelations(pet.Owner.Id);
                if (owner == null)
                {
                    throw new InvalidDataException("The owner has to be an existing owner in the database");
                }

                pet.Owner = owner;

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
           if(!_petRepository.GetAllPets().Exists(x => x.Id == id))
           {
                throw new KeyNotFoundException("A pet with this ID does not exist");
           }

           petToDelete = _petRepository.GetAllPets().Find(x => x.Id == id);
           return _petRepository.DeletePet(petToDelete);
           

        }

        public Pet EditPet(int id, Pet editedPet)
        {
            if (!_petRepository.GetAllPets().Exists(x => x.Id == id))
            {
                throw new KeyNotFoundException("A pet with this ID does not exist");
            }

           

            if (editedPet.Equals(null))
            {
                throw new InvalidDataException("Pet cannot be null");
            }

            if (editedPet.Name.Length < 1)
            {
                throw new InvalidDataException("Pet name has to be longer than one");
            }

            

            if (editedPet.PetType != null)
            {
                var petType = _petTypeRepository.SearchByIdWithoutRelations(editedPet.PetType.Id);
                if (petType == null)
                {
                    throw new InvalidDataException("The petType has to be an existing petType in the database");
                }

                editedPet.PetType = petType;

            }
            else
            {
                throw new InvalidDataException("New Pet has to have a petType");
            }

            if (editedPet.Owner != null)
            {
                var owner = _ownerRepository.SearchByIdWithoutRelations(editedPet.Owner.Id);
                if (owner == null)
                {
                    throw new InvalidDataException("The owner has to be an existing owner in the database");
                }

                editedPet.Owner = owner;

            }

            Pet petToBeEdited = _petRepository.SearchByIdWithoutRelations(id);
            petToBeEdited.Owner = editedPet.Owner;
            petToBeEdited.PetType = editedPet.PetType;
            petToBeEdited.BirthDate = editedPet.BirthDate;
            petToBeEdited.SoldDate = editedPet.SoldDate;
            petToBeEdited.Name = editedPet.Name;
            petToBeEdited.Color = editedPet.Color;
            petToBeEdited.Price = editedPet.Price;

            var successfullyEditedPet = _petRepository.EditPet(id, petToBeEdited);

            if (successfullyEditedPet == null)
            {
                throw new DataBaseException("Something went wrong in the database");
            }

            return successfullyEditedPet;


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
            if (!_petRepository.GetAllPets().Exists(x => x.Id == id))
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
