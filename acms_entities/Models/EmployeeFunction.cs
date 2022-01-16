namespace AccessManagementSystem.Entities.Models
{
    public class EmployeeFunction: Accessor
    {
        public Guid Id { get; set; }
        public List<Department> Departments { get; set; }
        public List<Employee> Employees { get; set; }
    }
}