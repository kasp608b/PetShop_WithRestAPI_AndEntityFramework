using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;

namespace PetShop.Infrastructure.Data.EntityFramework.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly PetShopDbContext _context;
        public PetRepository(PetShopDbContext context)
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
                            filtering = filtering.Where(pet => pet.PetType.Id.Equals(_context.PetTypes.ToList().Find(petType => petType.Name.Equals(filter.SearchText)).Id));
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
                            filtering = filtering.Where(pet => pet.Owner.Id.Equals(_context.Owners.ToList().Find(owner => owner.Name.Equals(filter.SearchText)).Id));
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
            if (petToAdd.PetType != null)
            {
                _context.Attach(petToAdd.PetType).State = EntityState.Unchanged;
            }

            if (petToAdd.Owner != null)
            {
                _context.Attach(petToAdd.Owner).State = EntityState.Unchanged;
            }

            var addedPet =  _context.Pets.Add(petToAdd).Entity;
            _context.SaveChanges();
            return addedPet;
        }

        public Pet DeletePet(Pet petToDelete)
        {
            var deletedPet = _context.Pets.Remove(petToDelete).Entity;
            _context.SaveChanges();
            return deletedPet;
        }

        public Pet EditPet(int id, Pet editedPet)
        {
            if (editedPet.PetType != null)
            {
                _context.Attach(editedPet.PetType).State = EntityState.Unchanged;
            }

            if (editedPet.Owner != null)
            {
                _context.Attach(editedPet.Owner).State = EntityState.Unchanged;
            }

            var successfullyEditedPet = _context.Pets.Update(editedPet).Entity;
            return successfullyEditedPet;
        }

        public Pet SearchById(int id)
        {
            return _context.Pets
                .AsNoTracking()
                .Include(pet => pet.Owner)
                .Include(pet => pet.PetType )
                .FirstOrDefault(pet => pet.Id == id);
        }
    }
}
