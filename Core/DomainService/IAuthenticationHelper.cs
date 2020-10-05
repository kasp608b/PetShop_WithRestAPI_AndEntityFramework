using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Core.DomainService
{
    public interface IAuthenticationHelper
    {
        string GenerateToken(User user);
    }
}