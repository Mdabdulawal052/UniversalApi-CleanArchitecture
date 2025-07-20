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
         DbSet<Employee> Employees { get; }
        DbSet<User> Users { get; }
        DbSet<NavigationMenu> NavigationMenus { get; }
        DbSet<Role> Roles { get; }
        DbSet<RoleMenuPermission> RoleMenuPermissions { get; }
        DbSet<UserRole> UserRoles { get; }


        Task<int> SaveChanges(CancellationToken cancellationToken);
        Task<int> Save();
    }
}
