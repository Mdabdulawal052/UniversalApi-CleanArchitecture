
using Application.Common.Extentions;
using Application.Common.Interfaces;
using Application.DTOS;
using Application.Queries.RoleQueries;
using Application.Queries.UserQueries;
using Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Auth
{

    public class AuthCommand : IRequest<AuthResponseDto>
    {
        public UserForRegistrationDto Model { get; set; }

    }

    public class AuthCommandHandler : IRequestHandler<AuthCommand, AuthResponseDto>
    {
        private readonly ITokenService _tokenService;
        private IMediator _mediator;
        // private readonly IIdentityService _identityService;

        public AuthCommandHandler(ITokenService tokenService, IMediator mediator)
        {
            //_identityService = identityService;
            _tokenService = tokenService;
            _mediator = mediator;
        }

        public async Task<AuthResponseDto> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserDetailsQuery(request.Model.UserName));

            if (user == null)//|| (EncryptPassword.EncryptStringToBytes(request.Model.Password, user.HashKey) != user.PasswordHash)
            {
                // throw new AuthenticationException("Invalid authentication");
                throw new Exception("Invalid authentication");

            }
            var roles = await _mediator.Send(new GetAllRoleQuery());

            var signingCredentials = _tokenService.GetSigningCredentials();
            var claims = await _tokenService.GetClaims(user,roles);
            var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            return new AuthResponseDto()
            {
                Name = user.UserName,
                Token = token
            };
        }
    }
}
