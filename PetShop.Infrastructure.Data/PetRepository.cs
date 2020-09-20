using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PetShop.Core.Entities.Entities;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;
using PetShop.Core.Entities.Exceptions;

namespace PetShop.Infrastructure.Data
{
    public class PetRepository : IPetRepository
    {
        
        public List<Pet> GetAllPets()
        {
            return FakeDB._pets;
        }

        public FilteredList<Pet> GetAllPetsFiltered(Filter filter)
        {
            DateTime searchDate;
            Double searchDouble;
            var filteredList = new FilteredList<Pet>();

            filteredList.TotalCount = GetAllPets().Count;
            filteredList.FilterUsed = filter;

            IEnumerable<Pet> filtering = GetAllPets();

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                switch (filter.SearchField)
                {
                    case "Name":
                        filtering = filtering.Where(pet => pet.Name.Contains(filter.SearchText));
                        break;

                    case "PetType":
                        if (FakeDB._petTypes.Exists(petType => petType.Name.Contains(filter.SearchText)))
                        {
                            filtering = filtering.Where(pet => pet.PetTypeID.Equals(FakeDB._petTypes.Find(petType => petType.Name.Equals(filter.SearchText)).ID));
                        }
                        else
                        {
                            throw new KeyNotFoundException("Could not find a pet with that petType");
                        }
                        break;

                    case "BirthDate":
                        
                        if (DateTime.TryParse(filter.SearchText, out searchDate))
                        {
                            filtering = filtering.Where(p => p.BirthDate.ToShortDateString().Contains(searchDate.ToShortDateString()));
                        }
                        else
                        {
                            throw new InvalidDataException("Wrong input, has to be a valid birthdate in format day/month/year");
                        }
                        
                        break;

                    case "SoldDate":
                        
                        if (DateTime.TryParse(filter.SearchText, out searchDate))
                        {
                            filtering = filtering.Where(p => p.SoldDate.ToShortDateString().Contains(searchDate.ToShortDateString()));
                        }
                        else
                        {
                            throw new InvalidDataException("Wrong input, has to be a valid birthdate in format day/month/year");
                        }
                        break;

                    case "Color":
                        filtering = filtering.Where(p => p.Color.Contains(filter.SearchText));
                        break;

                    case "PreviousOwner":
                        if (FakeDB._owners.Exists(owner => owner.Name.Contains(filter.SearchText)))
                        {
                            filtering = filtering.Where(pet => pet.PreviousOwnerID.Equals(FakeDB._owners.Find(owner => owner.Name.Equals(filter.SearchText)).ID));
                        }
                        else
                        {
                            throw new KeyNotFoundException("Could not find a pet with that owner");
                        }

                        break;

                    case "Price":
                        if (double.TryParse(filter.SearchText, out searchDouble))
                        {
                            filtering = filtering.Where(p => p.Price.Equals(searchDouble));
                        }
                        else
                        {
                            throw new InvalidDataException("Wrong input, has to be a valid double");
                        }
                        break;
                    default:
                        throw new InvalidDataException("Wrong Search-field input, search-field has to match a corresponding pet property");

                }
            }

            if (!string.IsNullOrEmpty(filter.OrderDirection) && !string.IsNullOrEmpty(filter.OrderProperty))
            {
                var prop = typeof(Pet).GetProperty(filter.OrderProperty);
                if (prop == null)
                {
                    throw new InvalidDataException("Wrong OrderProperty input, OrderProperty has to match to corresponding pet property");
                }



                filtering = "ASC".Equals(filter.OrderDirection)
                    ? filtering.OrderBy(p => prop.GetValue(p, null))
                    : filtering.OrderByDescending(p => prop.GetValue(p, null));
            }

            filteredList.List = filtering.ToList();
            return filteredList;
        }

        public Pet AddPet(Pet petToAdd)
        {
            return FakeDB.AddPet(petToAdd);
        }

        public Pet DeletePet(Pet petToDelete)
        {
            if (GetAllPets().Remove(petToDelete))
            {

            }
            else
            {
                throw new DataBaseException("Database failed to delete pet");
            }
            return petToDelete;
        }

        public Pet EditPet(int id, Pet editedPet)
        {
            Pet petToEdit = FakeDB._pets.Find(x => x.ID == id);
            petToEdit.Name = editedPet.Name;
            petToEdit.PetTypeID = editedPet.PetTypeID;
            petToEdit.BirthDate = editedPet.BirthDate;
            petToEdit.SoldDate = editedPet.SoldDate;
            petToEdit.Color = editedPet.Color;
            petToEdit.PreviousOwnerID = editedPet.PreviousOwnerID;
            petToEdit.Price = editedPet.Price;
            return petToEdit;
        }


        public Pet SearchById(int id)
        {
            return FakeDB._pets.Find(x => x.ID == id);
        }
    }
}
