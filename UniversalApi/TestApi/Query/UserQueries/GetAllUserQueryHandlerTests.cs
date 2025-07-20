using Application.Common.Interfaces;
using Application.DTOS;
using Application.Queries.EmployeeQueries;
using Application.Queries.UserQueries;
using AutoMapper;
using Domain.Entity;
using Infrastructure;
using MediatR;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApi.Common;

namespace TestApi.Query.UserQueries
{
    [Collection("QueryCollection")]
    public class GetAllUserQueryHandlerTests
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;


        public GetAllUserQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
            _mediator = fixture.Mediator;
        }
        [Fact]
        public async Task GetAllUser()
        {
            var sut = new GetAllUserQueryHandler(_context, _mapper,_mediator);

            var result = await sut.Handle(new GetAllUserQuery(), CancellationToken.None);

            result.ShouldBeOfType<List<UserDto>>();
            result.Count.ShouldBe(3);
        }
        //public async Task Handle_ReturnsListOfUserDto_WhenUsersExist()
        //{
        //    // Arrange
        //    var mockContext = new Mock<IAppDbContext>();
        //    var mockMapper = new Mock<IMapper>();
        //    var mockMediator = new Mock<IMediator>();

        //    var users = new List<User> {
        //        new User { /* Initialize properties */ },
        //        new User { /* Initialize properties */ },
        //        // Add more users as needed
        //    };
        //    var userDtos = new List<UserDto> {
        //        new UserDto { /* Initialize properties */ },
        //        new UserDto { /* Initialize properties */ },
        //        // Add corresponding UserDto objects
        //    };

        //    mockContext.Setup(c => c.Users)
        //               .Returns(new DbSetMock<User>(users).AsQueryable());

        //    mockMapper.Setup(m => m.Map<List<UserDto>>(users))
        //              .Returns(userDtos);

        //    var query = new GetAllUserQuery();
        //    var handler = new GetAllUserQueryHandler(mockContext.Object, mockMapper.Object, mockMediator.Object);

        //    // Act
        //    var result = await handler.Handle(query, CancellationToken.None);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(userDtos, result);
        //}
    }
}
