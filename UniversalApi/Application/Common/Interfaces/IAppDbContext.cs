using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<NavigationMenu> NavigationMenus { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleMenuPermission> RoleMenuPermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
