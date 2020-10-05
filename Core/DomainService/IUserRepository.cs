using System.Collections.Generic;
using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Core.DomainService
{
    public interface IUserRepository
    {
        public User AddUser(User userToAdd);
        public List<User> GetAllUsers();
    }
}