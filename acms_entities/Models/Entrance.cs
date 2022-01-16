namespace AccessManagementSystem.Entities.Models
{
    public class Entrance: IEntity
    {
        public Guid Id { get; set; }
        public Room Room { get; set; }
        public ElectronicLock Lock { get; set; }
    }
}