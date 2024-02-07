using Application.Common.Interfaces;
using Application.DTOS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.MenuQueries
{

    public class GetMenuItemQuery : IRequest<bool>
    {
        public ClaimsPrincipal ctx { get; set; }
        public string userName { get; set; }
        public string Roles { get; set; }
        public string path { get; set; }
    }

    public class GetMenuItemQueryHandler : IRequestHandler<GetMenuItemQuery, bool>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetMenuItemQueryHandler(
            IAppDbContext context,
            IMapper mapper,
            IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<bool> Handle(GetMenuItemQuery request, CancellationToken cancellationToken)
        {
            var result = false;

            var RolesData = request.Roles;

            //var user = await _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
            string uName = request.userName == null ? "" : request.userName;
            //var userRoles = await _context.Roles.ToListAsync();
            var data = await (from roles in _context.Roles
                              join rp in _context.UserRoles on roles.Id equals rp.RoleId
                              join u in _context.Users on rp.UserId equals u.Id
                              join rmp in _context.RoleMenuPermissions on roles.Id equals rmp.RoleId
                              join nm in _context.NavigationMenus on rmp.NavigationMenuId equals nm.Id
                              where u.UserName == uName && roles.Name == RolesData && nm.Url == request.path
                              select nm)
                            .FirstOrDefaultAsync();

            if (data != null)
            {
                if (data != null)
                {
                    result = true;
                }

            }
            return result;
        }
    }       
}
