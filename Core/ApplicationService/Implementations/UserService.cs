using System.Collections.Generic;
using PetShop.Core.ApplicationService.Interfaces;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Core.ApplicationService.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User AddUser(User userToAdd)
        {
           return _userRepository.AddUser(userToAdd);
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }
    }
}