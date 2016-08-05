using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GreenStone.Models;

namespace GreenStone.DataContext
{
    class ManagerContext : DbContext
    {
        public ManagerContext()
            :base("ManagerDb")
        { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }

        public DbSet<DpService> DpServices { get; set; }
        public DbSet<Stone> Stones { get; set; }
    }
}
