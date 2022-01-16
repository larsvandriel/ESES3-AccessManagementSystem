namespace AccessManagementSystem.Entities.Models
{
    public class Department: Accessor
    {
        public Guid Id { get; set; }
        public List<EmployeeFunction> EmployeeFunctions { get; set; }
    }
}