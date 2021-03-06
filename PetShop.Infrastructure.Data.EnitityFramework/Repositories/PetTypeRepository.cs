﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;

namespace PetShop.Infrastructure.Data.EntityFramework.Repositories
{
    public class PetTypeRepository : IPetTypeRepository
    {
        private readonly PetShopDbContext _context;
        public PetTypeRepository(PetShopDbContext context)
        {
            _context = context;
        }

        public List<PetType> GetAllPetTypes()
        {
           return _context.PetTypes.ToList();
        }

        public FilteredList<PetType> GetAllPetTypesFiltered(Filter filter)
        {
            DateTime searchDate;
            Double searchDouble;
            var filteredList = new FilteredList<PetType>();

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

            IEnumerable<PetType> filtering = _context.PetTypes
                .Skip((filter.CurrentPage - 1) * filter.ItemsPrPage)
                .Take(filter.ItemsPrPage);

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
            var changeTracker = _context.ChangeTracker.Entries<PetType>();

            _context.PetTypes.Attach(petTypeToAdd).State = EntityState.Added;
            _context.SaveChanges();
            return petTypeToAdd;
        }

        public PetType DeletePetType(PetType petTypeToDelete)
        {
            var deletedPeType = _context.Remove(petTypeToDelete).Entity;
            _context.SaveChanges();
            return deletedPeType;
        }

        public PetType EditPetType(int id, PetType editedPetType)
        {
            _context.Attach(editedPetType).State = EntityState.Modified;
            _context.SaveChanges();
            return editedPetType;
        }

        public PetType SearchById(int id)
        {
            var changeTracker = _context.ChangeTracker.Entries<PetType>();

            return _context.PetTypes
                .AsNoTracking()
                .Include(petType => petType.Pets)
                .FirstOrDefault(petType => petType.PetTypeId == id);
        }

        public PetType SearchByIdWithoutRelations(int id)
        {
            var changeTracker = _context.ChangeTracker.Entries<PetType>();

            return _context.PetTypes
                .AsNoTracking()
                .FirstOrDefault(petType => petType.PetTypeId == id);
        }

        public int Count()
        {
            return _context.PetTypes.Count();
        }
    }
}
