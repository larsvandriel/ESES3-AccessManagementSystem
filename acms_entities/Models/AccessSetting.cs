namespace AccessManagementSystem.Entities.Models
{
    public class AccessSetting
    {
        public Guid ElectricalLockId { get; set; }
        public ElectronicLock Lock { get; set; }
        public Guid AccessorId { get; set; }
        public Accessor Accessor { get; set; }
        public bool AllowedToEnter { get; set; }
    }
}