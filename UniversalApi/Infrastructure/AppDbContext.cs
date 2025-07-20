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
        //public DbSet<Employee> Employees { set; }
     

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<User> Users => Set<User>();
        public DbSet<NavigationMenu> NavigationMenus => Set<NavigationMenu>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<RoleMenuPermission> RoleMenuPermissions => Set<RoleMenuPermission>();

        public DbSet<UserRole> UserRoles => Set<UserRole>();




        public Task<int> SaveChanges(CancellationToken cancellationToken = new CancellationToken())
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
        public async Task<int> Save()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreateBy = string.Empty;
                        entry.Entity.ModifiedBy = string.Empty;
                        entry.Entity.CreateDate = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = string.Empty;
                        entry.Entity.ModifiedDate = DateTime.UtcNow;
                        break;
                }
            }
            return await base.SaveChangesAsync();
        }

    }
}
