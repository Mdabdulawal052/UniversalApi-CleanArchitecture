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
using System.Xml.Linq;

namespace Application.Queries.Employee
{
    public class GetEmployeeDetailQuery : IRequest<EmployeeDto>
    {
        public int Id { get; set; }
        public GetEmployeeDetailQuery(int id)
        { 
        
            Id = id; 
        }

    }
    public class GetEmployeeDetailQueryHandler : IRequestHandler<GetEmployeeDetailQuery, EmployeeDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetEmployeeDetailQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeDetailQuery request, CancellationToken cancellationToken)
        {
            var vm = await _context.Employees
                .Where(e => e.Id == request.Id)
                //.ProjectTo<EmployeeDetailVm>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            var data = _mapper.Map<EmployeeDto>(vm);
            return data;
        }
    }

}
