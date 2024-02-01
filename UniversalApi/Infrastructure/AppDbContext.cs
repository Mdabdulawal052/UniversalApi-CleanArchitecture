using Application.Common;
using Application.Common.Interfaces;
using Domain;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public AppDbContext(
           DbContextOptions<AppDbContext> options,
           ICurrentUserService currentUserService,
           IDateTime  dateTime)
           : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<NavigationMenu> NavigationMenus { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleMenuPermission> RoleMenuPermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }





        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreateBy = _currentUserService.UserId;
                        entry.Entity.CreateDate = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _currentUserService.UserId;
                        entry.Entity.ModifiedDate = _dateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
