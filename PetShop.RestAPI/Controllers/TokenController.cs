using Microsoft.AspNetCore.Mvc;

using System.Linq;
using PetShop.Core.ApplicationService.Interfaces;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities.Business;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetShop.RestAPI.Controllers
{
    [Route("/token")]
    [ApiController]
    public class TokenController : Controller
    {
        private IUserService _userService;
        private IAuthenticationHelper _authenticationHelper;

        public TokenController(IUserService userService, IAuthenticationHelper authenticationHelper)
        {
            _userService = userService;
            _authenticationHelper = authenticationHelper;
        }


        [HttpPost]
        public IActionResult Login([FromBody] LoginInputModel model)
        {
            var user = _userService.GetAllUsers().FirstOrDefault(u => u.Username == model.Username);

            // check if username exists
            if (user == null)
                return Unauthorized();

            // check if password is correct
            if (!model.Password.Equals(user.Password))
                return Unauthorized();

            // Authentication successful
            return Ok(new
            {
                username = user.Username,
                token = _authenticationHelper.GenerateToken(user)
            });
        }

    }
}
