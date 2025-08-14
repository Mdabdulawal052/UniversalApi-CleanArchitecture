using Application.Common.Interfaces;
using Application.DTOS;
using Application.Queries.EmployeeQueries;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web.Api.Handlers;

namespace Web.Api.Controllers
{
    
    public class EmployeesController : ApiBaseController
    {



        [Authorize(Roles ="User")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
           // return Ok(await _appDbContext.Employees.ToListAsync());
           return Ok(await Mediator.Send(new GetEmployeesListQuery()));
        }
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            // return Ok(await _appDbContext.Employees.ToListAsync());
            return Ok(await Mediator.Send(new GetEmployeeDetailQuery(id)));
        }
    }
}
