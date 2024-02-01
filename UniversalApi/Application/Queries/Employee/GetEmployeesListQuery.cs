using Application.Common.Interfaces;
using Application.DTOS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Employee
{
    public class GetEmployeesListQuery : IRequest<EmployeeListDto>
    {

    }
    public class GetEmployeesListQueryHandler : IRequestHandler<GetEmployeesListQuery, EmployeeListDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetEmployeesListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EmployeeListDto> Handle(GetEmployeesListQuery request, CancellationToken cancellationToken)
        {
            var employees = await _context.Employees
                .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                .OrderBy(e => e.Name)
                .ToListAsync(cancellationToken);

            var vm = new EmployeeListDto
            {
                Employees = employees
            };

            return vm;
        }
    }

}
