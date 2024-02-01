
using Application.Queries.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{

    public class UsersController : ApiBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // return Ok(await _appDbContext.Employees.ToListAsync());
            return Ok(await Mediator.Send(new GetAllUserQuery()));
        }

        [HttpGet]
        [Route("GetAllByPage")]
        public async Task<IActionResult> GetAll(int pageNumber,int pageSize)
        {
            // return Ok(await _appDbContext.Employees.ToListAsync());
            return Ok(await Mediator.Send(new GetAllUserByPaginationQuery(pageNumber,pageSize)));
        }
    }
}
