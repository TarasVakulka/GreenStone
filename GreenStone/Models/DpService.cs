using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenStone.Models
{
    public class DpService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ServicePrice { get; set; }
        public bool IsDone { get; set; }

        public int? DepartmentId { get; set; }
        public virtual Department Department { get; set; } 

        public virtual ICollection<Product> Products { get; set; }

        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int? EmployeeForSalaryId { get; set; }
        public virtual Employee EmployeeForSalary { get; set; }

        public DpService()
        {
            
            Products = new List<Product>();
           
        }
    }
}
