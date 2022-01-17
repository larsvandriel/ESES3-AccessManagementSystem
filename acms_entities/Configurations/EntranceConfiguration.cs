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
    public class EntranceConfiguration : IEntityTypeConfiguration<Entrance>
    {
        public void Configure(EntityTypeBuilder<Entrance> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Lock).WithOne();
            builder.HasOne(e => e.Room).WithMany().IsRequired();
        }
    }
}
