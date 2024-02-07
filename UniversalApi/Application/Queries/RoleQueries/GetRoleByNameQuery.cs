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

namespace Application.Queries.RoleQueries
{
    public class GetRoleByNameQuery : IRequest<RoleDto>
    {
        public string Role { get; set; }
    }
   
    public class GetRoleByNameQueryHandler : IRequestHandler<GetRoleByNameQuery, RoleDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private IMediator _mediator;

        public GetRoleByNameQueryHandler(IAppDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<RoleDto> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
        {
            var vm = await _context.Roles
                .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x=>x.Name == request.Role,cancellationToken);


            return vm;
        }


    }
}
