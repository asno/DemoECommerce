using AuthenticationAPI.Application.DTO;
using AuthenticationAPI.Application.Interface;
using ECommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IUser userInterface) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<Response>> Register(AppUserDTO appUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await userInterface.Register(appUserDTO);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response>> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await userInterface.Login(loginDTO);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUserWithoutPasswordDTO>> GetUser(int id)
        {
            if (id < 1)
            {
                return BadRequest("Invalid argument passed.");
            }

            var user = await userInterface.GetUser(id);
            if (user is null)
            {
                return NotFound("User not found.");
            }

            return user.Id > 0 ? Ok(user) : BadRequest(user);
        }
    }
}
