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
            return Ok();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForRegistrationDto userForRegistration)
        {
            return Ok();
        }
    }
}
