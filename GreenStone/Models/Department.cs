using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenStone.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Profitability { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<DpService> DpServices { get; set; }
        
        public Department ()
        {
       
            Employees = new List<Employee>();
            DpServices = new List<DpService>();
        }
    }
}
