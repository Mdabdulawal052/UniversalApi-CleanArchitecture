using Application.Common.Interfaces;
using Application.DTOS;
using Application.Queries.Employee;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Controllers
{
    
    public class EmployeesController : ApiBaseController
    {
       



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           // return Ok(await _appDbContext.Employees.ToListAsync());
           return Ok(await Mediator.Send(new GetEmployeesListQuery()));
        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            // return Ok(await _appDbContext.Employees.ToListAsync());
            return Ok(await Mediator.Send(new GetEmployeeDetailQuery(id)));
        }
    }
}
