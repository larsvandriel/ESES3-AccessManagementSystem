using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessManagementSystem.Entities.Models
{
    public class Employee: Accessor
    {
        public Guid Id { get; set; }
        public int EmployeeNumber { get; set; }
        public List<Department> WorksInDepartments { get; set; }
        public List<EmployeeFunction> Functions { get; set; }
    }
}
