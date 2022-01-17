namespace AccessManagementSystem.Entities.Models
{
    public class ElectronicLock: IEntity
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public List<AccessSetting> AccessSettings { get; set; }
    }
}