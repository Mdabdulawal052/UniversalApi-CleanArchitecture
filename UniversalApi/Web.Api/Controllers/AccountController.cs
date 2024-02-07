using Application.Commands.Auth;
using Application.Commands.UserCommand;
using Application.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
  
    public class AccountController : ApiBaseController
    {
        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            return Ok(await Mediator.Send(new CreateUserCommand()
            {
                UserDto = userForRegistration,
            }));
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForRegistrationDto userDto)
        {
            return Ok(await Mediator.Send(new AuthCommand()
            {
                Model = userDto,
            }));
        }
    }
}
