using Application.Common.Interfaces;
using Application.Common.Pagination;
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

    public class GetAllUserByPaginationQuery : IRequest<PaginatedResult<UserDto>>
    {

        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public GetAllUserByPaginationQuery(int pageNumber, int pageSize)
        {
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
        }
    }

    public class GetAllUserByPaginationQueryHandler : IRequestHandler<GetAllUserByPaginationQuery, PaginatedResult<UserDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private IMediator _mediator;

        public GetAllUserByPaginationQueryHandler(IAppDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<PaginatedResult<UserDto>> Handle(GetAllUserByPaginationQuery request, CancellationToken cancellationToken)
        {
            var vm = await _context.Users
                //.Where(e => e.Id == request.Id)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.pageNumber, request.pageSize, cancellationToken);

            //var s=  await _mediator.Send(new GetUserDetailsQuery(vm[0].Id) { });

            //var data = _mapper.Map<List<UserDto>>(vm);
            return vm;


        }
    }
}
