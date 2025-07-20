using System;
using Domain.Entity;
using Infrastructure;
using Microsoft.EntityFrameworkCore;


namespace TestApi.Common
{
    public class NorthwindContextFactory
    {
        public static async Task<AppDbContext> CreateAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            context.Database.EnsureCreated();

            context.Employees.AddRange(new[] {
                new Employee { Id = 1, Name = "Adam Cogan",Address="Dhaka" },
                new Employee { Id = 2, Name = "Jason Taylor",Address="Dhaka" },
                new Employee { Id = 3, Name = "Brendan Richards",Address="Dhaka" },
            });
            context.Users.AddRange(new[] {
                new User { Id = 1, UserName = "Adam Cogan", CreateDate=DateTime.Now,Email="",PasswordHash="",HashKey="",RefreshToken="" },
                new User { Id = 2, UserName = "Brendan Cn", CreateDate=DateTime.Now,Email="",PasswordHash="",HashKey="",RefreshToken=""  },
                new User {Id = 3, UserName = "Brendan Cogan", CreateDate = DateTime.Now, Email = "", PasswordHash = "", HashKey = "",RefreshToken="" }

            });

            var r = await context.Save() > 0;

            return context;
        }

        public static void Destroy(AppDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}