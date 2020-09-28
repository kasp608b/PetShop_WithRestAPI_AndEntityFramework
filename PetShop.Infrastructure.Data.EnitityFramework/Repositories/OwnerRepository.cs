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
    public class OwnerRepository : IOwnerRepository
    {
        private readonly PetShopDbContext _context;
        public OwnerRepository(PetShopDbContext context)
        {
            _context = context;
        }

        public List<Owner> GetAllOwners()
        {
            return _context.Owners.ToList();
        }

        public FilteredList<Owner> GetAllOwnersFiltered(Filter filter)
        {
            DateTime searchDate;
            Double searchDouble;
            var filteredList = new FilteredList<Owner>();

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

            IEnumerable<Owner> filtering = _context.Owners
                .Skip((filter.CurrentPage - 1) * filter.ItemsPrPage)
                .Take(filter.ItemsPrPage);

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                switch (filter.SearchField)
                {
                    case "Name":
                        filtering = filtering.Where(p => p.Name.Contains(filter.SearchText));
                        break;

                    case "Email":
                        filtering = filtering.Where(p => p.Email.Contains(filter.SearchText));
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

                    default:
                        throw new InvalidDataException("Wrong Search-field input, search-field has to match a corresponding pet property");

                }
            }

            if (!string.IsNullOrEmpty(filter.OrderDirection) && !string.IsNullOrEmpty(filter.OrderProperty))
            {
                var prop = typeof(Owner).GetProperty(filter.OrderProperty);
                if (prop == null)
                {
                    throw new InvalidDataException("Wrong OrderProperty input, OrderProperty has to match to corresponding owner property");
                }

                filtering = "ASC".Equals(filter.OrderDirection)
                    ? filtering.OrderBy(p => prop.GetValue(p, null))
                    : filtering.OrderByDescending(p => prop.GetValue(p, null));
            }

            filteredList.List = filtering.ToList();
            return filteredList;
        }

        public Owner AddOwner(Owner ownerToAdd)
        {
            var changeTracker = _context.ChangeTracker.Entries<Owner>();

            _context.Attach(ownerToAdd).State = EntityState.Added;
            _context.SaveChanges();
            return ownerToAdd;
        }

        public Owner DeleteOwner(Owner ownerToDelete)
        {
            var deletedOwner = _context.Remove(ownerToDelete).Entity;
            _context.SaveChanges();
            return deletedOwner;
        }

        public Owner EditOwner(int id, Owner editedOwner)
        {
            _context.Attach(editedOwner).State = EntityState.Modified;
            _context.SaveChanges();
            return editedOwner;
        }

        public Owner SearchById(int id)
        {
            var changeTracker = _context.ChangeTracker.Entries<Owner>();

            return _context.Owners
                .AsNoTracking()
                .Include(owner => owner.Pets)
                .FirstOrDefault(owner => owner.Id == id);
        }

        public Owner SearchByIdWithoutRelations(int id)
        {
            var changeTracker = _context.ChangeTracker.Entries<Owner>();

            return _context.Owners
                .AsNoTracking()
                .FirstOrDefault(owner => owner.Id == id);
        }

        public int Count()
        {
            return _context.Owners.Count();
        }
    }
}
