namespace AccessManagementSystem.Entities.Models
{
    public class AccessSetting
    {
        public ElectronicLock Lock { get; set; }
        public Accessor Accessor { get; set; }
        public bool AllowedToEnter { get; set; }
    }
}