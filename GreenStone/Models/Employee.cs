using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenStone.Models
{
    public class Employee : PersonProfile
    {
        public int Id { get; set; }

        public int Salary { get; set; }
        public int? DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        
        [ForeignKey("EmployeeId")]
        public virtual List<DpService> Services { get; set; }
        
        [ForeignKey("EmployeeForSalaryId")]
        public virtual List<DpService> ServicesForSalary { get; set; }

        public Employee()
        {
            Services = new List<DpService>();
            ServicesForSalary = new List<DpService>();
        }



    }
}
