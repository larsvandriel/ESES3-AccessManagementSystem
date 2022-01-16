using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessManagementSystem.Entities.Models
{
    public class Location
    {
        public Guid Id { get; set; }
        public int LocationNumber { get; set; }
        public string Name { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
