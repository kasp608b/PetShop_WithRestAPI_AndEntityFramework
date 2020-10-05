using System.Collections.Generic;
using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Core.ApplicationService.Interfaces
{
    public interface IUserService
    {
        public User AddUser(User userToAdd);
        public List<User> GetAllUsers();
    }
}