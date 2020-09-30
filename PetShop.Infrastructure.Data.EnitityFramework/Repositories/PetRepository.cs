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

            filteredList.TotalCount = Count();
            filteredList.FilterUsed = filter;

            if (filter.CurrentPage == 0)
            {
                filter.CurrentPage = 1;
            }

            if (filter.ItemsPrPage == 0)
            {
                filter.ItemsPrPage = 10;
            }

            IEnumerable<Pet> filtering = _context.Pets
                .Skip((filter.CurrentPage - 1) * filter.ItemsPrPage)
                .Take(filter.ItemsPrPage);

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
                            filtering = filtering.Where(pet => pet.PetType.PetTypeId.Equals(_context.PetTypes.ToList().Find(petType => petType.Name.Equals(filter.SearchText)).PetTypeId));
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

                    case "PreviousOwner":
                        if (_context.Owners.ToList().Exists(owner => owner.Name.Contains(filter.SearchText)))
                        {
                            filtering = filtering.Where(pet => pet.Owner.OwnerId.Equals(_context.Owners.ToList().Find(owner => owner.Name.Equals(filter.SearchText)).OwnerId));
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
            //var petTypeChangeTracker = _context.ChangeTracker.Entries<PetType>();

            //if (petToAdd.PetType != null && _context.ChangeTracker.Entries<PetType>()
            //    .FirstOrDefault(petTypeEntry => petTypeEntry.Entity.OwnerId == petToAdd.PetType.OwnerId) == null)
            //{
            //    _context.Attach(petToAdd.PetType);
            //}

            //var petOwnerChangeTracker = _context.ChangeTracker.Entries<Owner>();

            //if (petToAdd.Owner != null && _context.ChangeTracker.Entries<Owner>()
            //    .FirstOrDefault(ownerEntry =>ownerEntry.Entity.OwnerId == petToAdd.Owner.OwnerId) == null)
            //{
            //    _context.Attach(petToAdd.Owner);
            //}

            _context.Attach(petToAdd).State = EntityState.Added;
            _context.SaveChanges();
            return petToAdd;
        }

        public void AddPets(List<Pet> pets)
        {
            _context.AddRange(pets);
            _context.SaveChanges();
        }

        public Pet DeletePet(Pet petToDelete)
        {
            var deletedPet = _context.Pets.Remove(petToDelete).Entity;
            _context.SaveChanges();
            return deletedPet;
        }

        public Pet EditPet(int id, Pet editedPet)
        {
            //var petTypeChangeTracker = _context.ChangeTracker.Entries<PetType>();
            //var ChangeTracker = _context.ChangeTracker.Entries<Pet>();
            
            //if (editedPet.PetType != null && _context.ChangeTracker.Entries<PetType>()
            //    .FirstOrDefault(petTypeEntry => petTypeEntry.Entity.OwnerId == editedPet.PetType.OwnerId) == null)
            //{
            //    _context.Attach(editedPet.PetType);
            //}
            //else
            //{
            //    _context.Entry(editedPet).Reference(pet => pet.PetType).IsModified = true;
            //}

            //var petOwnerChangeTracker = _context.ChangeTracker.Entries<Owner>();

            //if (editedPet.Owner != null && _context.ChangeTracker.Entries<Owner>()
            //    .FirstOrDefault(ownerEntry => ownerEntry.Entity.OwnerId == editedPet.Owner.OwnerId) == null)
            //{
            //    _context.Attach(editedPet.Owner);
            //}
            //else
            //{
            //    _context.Entry(editedPet).Reference(pet => pet.Owner).IsModified = true;
            //}

           // _context.Attach(editedPet).State = EntityState.Modified;

           if (editedPet.Owner != null)
           {
               _context.Attach(editedPet.Owner).State = EntityState.Unchanged;
           }
           else
           {
               _context.Entry(editedPet).Reference(pet => pet.Owner).IsModified = true;

           }

           _context.Attach(editedPet.PetType).State = EntityState.Unchanged;
           _context.Update(editedPet);
           _context.SaveChanges();
           return editedPet;
        }

        public Pet SearchById(int id)
        {
            var changeTracker = _context.ChangeTracker.Entries<Pet>();

            return _context.Pets
                .AsNoTracking()
                .Include(pet => pet.Owner)
                .Include(pet => pet.PetType)
                .Include(pet => pet.PetColors)
                .ThenInclude(petColor => petColor.Color)
                .FirstOrDefault(pet => pet.PetId == id);
        }

        public Pet SearchByIdWithoutRelations(int id)
        {
            var changeTracker = _context.ChangeTracker.Entries<Pet>();

            return _context.Pets
                .AsNoTracking()
                .FirstOrDefault(pet => pet.PetId == id);
        }

        public int Count()
        {
            return _context.Pets.Count();
        }
    }
}
