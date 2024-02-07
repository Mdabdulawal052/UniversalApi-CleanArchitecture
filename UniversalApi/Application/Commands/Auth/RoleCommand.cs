using Application.Common.Extentions;
using Application.Common.Interfaces;
using Application.DTOS;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Application.Queries.RoleQueries;

namespace Application.Commands.Auth
{
    public class RoleCommand : IRequest<bool>
    {
        public UserDto User { get; set; }
        public string Role { get; set; }
    }

    public class RoleCommandHandler : IRequestHandler<RoleCommand, bool>
    {
        private readonly ITokenService _tokenService;
        private IMediator _mediator;
        private readonly IAppDbContext _context;

        public RoleCommandHandler(ITokenService tokenService, IMediator mediator, IAppDbContext context)
        {
            //_identityService = identityService;
            _tokenService = tokenService;
            _mediator = mediator;
            _context = context;
        }

        public async Task<bool> Handle(RoleCommand request, CancellationToken cancellationToken)
        {
            var roleId = 0;
            var result = new RoleDto();
            result = await _mediator.Send(new GetRoleByNameQuery()
            {
                Role = request.Role,
            });

            if (result == null)
            {
                var roleData = new Role
                {
                    Name = request.Role,
                    NormalizedName = request.Role.ToUpper()
                };
                var data = await _context.Roles.AddAsync(roleData);
                await _context.Save();
                roleId = roleData.Id;
            }
            else
            {
                roleId = result.Id;
            }
            var userRole = new UserRole
            {
                RoleId = roleId,
                UserId = request.User.Id,
            };
            await _context.UserRoles.AddAsync(userRole);
            return await _context.Save() > 0;

        }


    }
}
