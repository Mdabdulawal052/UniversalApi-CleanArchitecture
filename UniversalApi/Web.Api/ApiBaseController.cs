using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api
{
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiBaseController:ControllerBase
    {
        private ISender _sender = null;
        protected ISender Mediator => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();//HttpContext.RequestServices.GetService<ISender>();
    }
}
