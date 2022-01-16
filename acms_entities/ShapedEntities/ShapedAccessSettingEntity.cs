using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessManagementSystem.Entities.ShapedEntities
{
    public class ShapedAccessSettingEntity: ShapedEntity
    {
        public Guid ElectronicLockId { get; set; }
        public Guid AccessorId { get; set; }
    }
}
