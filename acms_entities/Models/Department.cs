namespace AccessManagementSystem.Entities.Models
{
    public class Department: Accessor
    {
        public Guid DepartmentId { get; set; }
        public List<EmployeeFunction> EmployeeFunctions { get; set; }
    }
}