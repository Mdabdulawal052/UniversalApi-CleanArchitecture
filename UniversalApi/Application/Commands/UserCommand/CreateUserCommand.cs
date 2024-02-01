using Application.Commands.Auth;
using Application.Common.Interfaces;
using Application.DTOS;
using AutoMapper;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserCommand
{
    public class CreateUserCommand : IRequest<bool>
    {
        public UserForRegistrationDto UserDto { get; set; }
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private IMediator _mediator;

        public CreateUserCommandHandler(IAppDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            var user = new User()
            {
                UserName = request.UserDto.UserName,
                Email = request.UserDto.Email,
                RefreshToken = ""
            };
            _context.Users.Add(user);

            return true;
           
        }
    }
}
