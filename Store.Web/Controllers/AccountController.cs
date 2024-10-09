using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities.IdinitiesEntities;
using Store.Service.UserSerives;
using Store.Service.UserSerives.Dto;

namespace Store.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly UserManager<AppUser> _userManger;

        public AccountController(IUserServices userServices , UserManager<AppUser> userManger) 
        {
           _userServices = userServices;
            _userManger = userManger;
        }
        [HttpPost]
        public async Task<ActionResult<UserDto>>Login(LoginDto input)
        {
            var user = await _userServices.Login(input);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult<UserDto>> Register(RegisterDto input)
        {
            var user = await _userServices.Register(input);
            if (user == null)
            {
                return BadRequest(new { message = "User already exists" });
            }

            return Ok(user);
        }
        public async Task<UserDto> GetCurrentUserDetails()
        {
            var userIdClaim = User?.FindFirst("UserId");

            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }
            var user = await _userManger.FindByIdAsync(userIdClaim.Value);
            return new UserDto
            {
                Id = Guid.Parse(user.Id),
                DisplayName = user.DisplayName,
                Email = user.Email,
            };
        }

    }
}
