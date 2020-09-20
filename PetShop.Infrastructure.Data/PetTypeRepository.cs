using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;
using PetShop.Core.Entities.Exceptions;

namespace PetShop.Infrastructure.Data
{
    public class PetTypeRepository : IPetTypeRepository
    {
        private readonly IPetRepository _petRepository;

        public PetTypeRepository(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public List<PetType> GetAllPetTypes()
        {
            return FakeDB._petTypes;
        }

        public FilteredList<PetType> GetAllPetTypesFiltered(Filter filter)
        {
            DateTime searchDate;
            Double searchDouble;
            var filteredList = new FilteredList<PetType>();

            filteredList.TotalCount = GetAllPetTypes().Count;
            filteredList.FilterUsed = filter;

            IEnumerable<PetType> filtering = GetAllPetTypes();

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                switch (filter.SearchField)
                {
                    case "Name":
                        filtering = filtering.Where(p => p.Name.Contains(filter.SearchText));
                        break;

                    default:
                        throw new InvalidDataException("Wrong Search-field input, search-field has to match a corresponding petType property");

                }
            }

            if (!string.IsNullOrEmpty(filter.OrderDirection) && !string.IsNullOrEmpty(filter.OrderProperty))
            {
                var prop = typeof(PetType).GetProperty(filter.OrderProperty);
                if (prop == null)
                {
                    throw new InvalidDataException("Wrong OrderProperty input, OrderProperty has to match to corresponding petType property");
                }

                filtering = "ASC".Equals(filter.OrderDirection)
                    ? filtering.OrderBy(p => prop.GetValue(p, null))
                    : filtering.OrderByDescending(p => prop.GetValue(p, null));
            }

            filteredList.List = filtering.ToList();
            return filteredList;
        }

        public PetType AddPetType(PetType petTypeToAdd)
        {
            return FakeDB.AddPetTypes(petTypeToAdd);
        }

        public PetType DeletePetType(PetType petTypeToDelete)
        {
            if (GetAllPetTypes().Remove(petTypeToDelete))
            {
                foreach (Pet pet in _petRepository.GetAllPets().FindAll(pet => pet.PetTypeID == petTypeToDelete.ID))
                {
                    FakeDB._pets.Remove(pet);
                }
            }
            else
            {
                throw new DataBaseException("Database failed to delete petType");
            }
            return petTypeToDelete;
        }

        public PetType EditPetType(int id, PetType editedPetType)
        {
            PetType petTypeToEdit = FakeDB._petTypes.Find(x => x.ID == id);
            petTypeToEdit.Name = editedPetType.Name;
            return petTypeToEdit;
        }

        public PetType SearchById(int id)
        {
            return FakeDB._petTypes.Find(x => x.ID == id);
        }
    }
}