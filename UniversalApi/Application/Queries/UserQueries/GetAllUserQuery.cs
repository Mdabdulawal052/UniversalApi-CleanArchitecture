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

namespace Application.Queries.UserQueries
{

    public class GetAllUserQuery : IRequest<List<UserDto>>
    {


    }
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, List<UserDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private IMediator _mediator;

        public GetAllUserQueryHandler(IAppDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<List<UserDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var vm = await _context.Users
                //.Where(e => e.Id == request.Id)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            //var s=  await _mediator.Send(new GetUserDetailsQuery(vm[0].Id) { });

            //var data = _mapper.Map<List<UserDto>>(vm);
            return vm;
        }
    }

}
