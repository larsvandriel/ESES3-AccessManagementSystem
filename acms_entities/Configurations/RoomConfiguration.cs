using AccessManagementSystem.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessManagementSystem.Entities.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => r.Id);
            builder.HasMany(r => r.Entrances).WithOne(e => e.Room).IsRequired();
            builder.Property(r => r.Name).IsRequired();
        }
    }
}
