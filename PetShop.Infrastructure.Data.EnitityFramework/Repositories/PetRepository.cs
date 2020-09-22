using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;

namespace PetShop.Infrastructure.Data.EnitityFramework.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly PetShopDBContext _context;
        public PetRepository(PetShopDBContext context)
        {
            _context = context;
        }

        public List<Pet> GetAllPets()
        {
            return _context.Pets.ToList();
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
                        if (_context.PetTypes.ToList().Exists(petType => petType.Name.Contains(filter.SearchText)))
                        {
                            filtering = filtering.Where(pet => pet.PetTypeID.Equals(_context.PetTypes.ToList().Find(petType => petType.Name.Equals(filter.SearchText)).ID));
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
                        if (_context.Owners.ToList().Exists(owner => owner.Name.Contains(filter.SearchText)))
                        {
                            filtering = filtering.Where(pet => pet.PreviousOwnerID.Equals(_context.Owners.ToList().Find(owner => owner.Name.Equals(filter.SearchText)).ID));
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
            var addedPet =  _context.Pets.Add(petToAdd).Entity;
            _context.SaveChanges();
            return addedPet;
        }

        public Pet DeletePet(Pet petToDelete)
        {
            throw new NotImplementedException();
        }

        public Pet EditPet(int id, Pet editedPet)
        {
            throw new NotImplementedException();
        }

        public Pet SearchById(int id)
        {
            return _context.Pets.FirstOrDefault(pet => pet.ID == id);
        }
    }
}
