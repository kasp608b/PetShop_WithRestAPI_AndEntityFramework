using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Infrastructure.Data.EntityFramework.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PetShopDbContext _context;

        public UserRepository(PetShopDbContext context)
        {
            _context = context;
        }

        public User AddUser(User userToAdd)
        {
            _context.Attach(userToAdd).State = EntityState.Added;
            _context.SaveChanges();
            return userToAdd;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}