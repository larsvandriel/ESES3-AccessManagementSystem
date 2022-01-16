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
    public class ElectronicLockConfiguration : IEntityTypeConfiguration<ElectronicLock>
    {
        public void Configure(EntityTypeBuilder<ElectronicLock> builder)
        {
            builder.HasKey(el => el.Id);
            builder.HasMany(el => el.AccessSettings).WithOne(a => a.Lock);
            builder.HasIndex(el => el.SerialNumber).IsUnique();
        }
    }
}
