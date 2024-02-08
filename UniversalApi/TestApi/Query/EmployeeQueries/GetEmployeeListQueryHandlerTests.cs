using Application.DTOS;
using Application.Queries.EmployeeQueries;
using AutoMapper;
using Infrastructure;
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
    public class GetEmployeeListQueryHandlerTests
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetEmployeeListQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }
        [Fact]
        public async Task GetEmployeeList()
        {
            var sut = new GetEmployeesListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetEmployeesListQuery(), CancellationToken.None);

            result.ShouldBeOfType<EmployeeListDto>();
            result.Employees.Count.ShouldBe(3);
        }
    }
}
