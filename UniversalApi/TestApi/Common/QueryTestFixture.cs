using System;
using Application.Common.Mappings;
using AutoMapper;
using Infrastructure;
using MediatR;
using Xunit;

namespace TestApi.Common
{
    public class QueryTestFixture : IDisposable
    {
        public AppDbContext Context { get; private set; }
        public IMapper Mapper { get; private set; }
        public IMediator Mediator { get; private set; }
        public QueryTestFixture()
        {
            Context = NorthwindContextFactory.CreateAsync().Result;

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = configurationProvider.CreateMapper();
        }

        public void Dispose()
        {
            NorthwindContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}