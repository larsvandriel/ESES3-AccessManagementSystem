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
    public class AccessSettingConfiguration : IEntityTypeConfiguration<AccessSetting>
    {
        public void Configure(EntityTypeBuilder<AccessSetting> builder)
        {
            builder.HasKey(a => new { a.Lock, a.Accessor });
            builder.Property(a => a.AllowedToEnter);
        }
    }
}
