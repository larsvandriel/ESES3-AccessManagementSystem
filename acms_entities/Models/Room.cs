namespace AccessManagementSystem.Entities.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Entrance> Entrances { get; set; }
    }
}