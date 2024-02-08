using Application.DTOS;
using Application.Queries.EmployeeQueries;
using AutoMapper;
using Domain.Entity;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApi.Common;

namespace TestApi.Query.EmployeeQueries
{
    [Collection("QueryCollection")]
    public class GetEmployeeDetailQueryHandlerTests
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetEmployeeDetailQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }
        [Fact]
        public async Task GetEmployeeDetail()
        {
            var sut = new GetEmployeeDetailQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetEmployeeDetailQuery(1), CancellationToken.None);

            result.ShouldBeOfType<EmployeeDto>();
            result.Id.ShouldBe(1);
        }
    }
}
