namespace AccessManagementSystem.Entities.Models
{
    public class Entrance
    {
        public Guid Id { get; set; }
        public Room Room { get; set; }
        public ElectronicLock Lock { get; set; }
    }
}