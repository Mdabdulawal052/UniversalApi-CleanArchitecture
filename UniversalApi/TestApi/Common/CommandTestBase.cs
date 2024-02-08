using Infrastructure;
using System;

namespace TestApi.Common
{
    public class CommandTestBase : IDisposable
    {
        protected readonly AppDbContext _context;

        public CommandTestBase()
        {
            _context = NorthwindContextFactory.CreateAsync().Result;
        }

        public void Dispose()
        {
            NorthwindContextFactory.Destroy(_context);
        }
    }
}