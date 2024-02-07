using Application.Common.Interfaces;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.System.Commands.SeedSampleData
{
    public class SampleDataSeeder
    {
        private readonly IAppDbContext _context;
        private readonly Dictionary<int, Employee> Employees = new Dictionary<int, Employee>();
        public SampleDataSeeder(IAppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAllAsync(CancellationToken cancellationToken)
        {
            //if (_context.Employees.Any())
            //{
            //    return;
            //}
            await SeedEmployeesAsync(cancellationToken);
        }
        private async Task SeedEmployeesAsync(CancellationToken cancellationToken)
        {
            Employees.Add(2,
               new Employee
               {
                   Name = "Foysal",
                   CreateBy = "",
                   CreateDate = DateTime.Now
               });
            Employees.Add(2,
             new Employee
             {
                 Name = "Mridul",
                 CreateBy = "",
                 CreateDate = DateTime.Now
             });
            Employees.Add(2,
             new Employee
             {
                 Name = "Tawsif",
                 CreateBy = "",
                 CreateDate = DateTime.Now
             });
            foreach (var employee in Employees.Values)
            {
                _context.Employees.Add(employee);
            }

            await _context.SaveChanges(cancellationToken);
        }
    }
}
