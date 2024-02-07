using Application.Common.Interfaces;
using Application.DTOS;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.UserQueries
{
    public class GetUserDetailsQuery : IRequest<UserDto>
    {
        public string UserName { get; set; }

        public GetUserDetailsQuery(string userName)
        {
            UserName = userName;
        }
    }
    public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetUserDetailsQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var vm = await _context.Users
                .Where(e => e.UserName == request.UserName)
                //.ProjectTo<EmployeeDetailVm>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            var data = _mapper.Map<UserDto>(vm);
            return data;
        }
    }


}
