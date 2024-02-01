using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class ApiBaseController:ControllerBase
    {
        private ISender _sender = null;
        protected ISender Mediator => _sender ??= HttpContext.RequestServices.GetService<ISender>();
    }
}
