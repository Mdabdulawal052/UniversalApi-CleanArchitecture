using Application.Commands.Auth;
using Application.Common.Extentions;
using Application.Common.Interfaces;
using Application.DTOS;
using Application.Queries.UserQueries;
using Ardalis.Result;
using AutoMapper;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserCommand
{
    public class CreateUserCommand : IRequest<Result<bool>>
    {
        public UserForRegistrationDto UserDto { get; set; }
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<bool>>
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
        public async Task<Result<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<ValidationError>();
            Guid guid = Guid.NewGuid();
            byte[] bytes = guid.ToByteArray();
            string encoded = Convert.ToBase64String(bytes);
            string passwordHash = EncryptPassword.EncryptStringToBytes(request.UserDto.UserName, encoded);

            var user = new User()
            {
                UserName = request.UserDto.UserName,
                Email = request.UserDto.Email,
                HashKey = encoded,
                PasswordHash = passwordHash,
                EmailConfirmed = true,
                RefreshToken = "",
                RefreshTokenExpiryTime = DateTime.Now
            };


            UserDto findUser = await _mediator.Send(new GetUserDetailsQuery(request.UserDto.UserName));

            if (findUser == null)
            {
                await _context.Users.AddAsync(user);
                if (await _context.Save() > 0)
                {
                    var userDto = new UserDto() { Id = user.Id };
                    await _mediator.Send(new RoleCommand()
                    {
                        User = userDto,
                        Role = "Viewer"
                    });
                    return true;
                 }
                else
                {
                    return false;
                }


            }
            else
            {
                errors.Add(new ValidationError()
                {
                    Identifier = "CreateUserCommand",
                    ErrorMessage = "Name Already Exist"
                });
                return Result<bool>.Invalid(errors);

            }

        }


    }
}
