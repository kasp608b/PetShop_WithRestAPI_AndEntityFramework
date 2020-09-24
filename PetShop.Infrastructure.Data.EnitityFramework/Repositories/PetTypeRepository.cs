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
            var addedPetType = _context.PetTypes.Add(petTypeToAdd).Entity;
            _context.SaveChanges();
            return addedPetType;
        }

        public PetType DeletePetType(PetType petTypeToDelete)
        {
            throw new NotImplementedException();
        }

        public PetType EditPetType(int id, PetType editedPetType)
        {
            throw new NotImplementedException();
        }

        public PetType SearchById(int id)
        {
            return _context.PetTypes
                .AsNoTracking()
                .Include(petType => petType.Pets)
                .FirstOrDefault(PetType => PetType.Id == id);
        }
    }
}
