using AccessManagementSystem.Entities.Helpers;
using AccessManagementSystem.Entities.Models;
using AccessManagementSystem.Entities.Parameters;
using AccessManagementSystem.Entities.ShapedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessManagementSystem.Contracts
{
    public interface IAccessSettingRepository: IRepositoryBase<AccessSetting>
    {
        PagedList<ShapedEntity> GetAllAccessSettings(AccessSettingParameters accessSettingParameters);
        ShapedEntity GetAccessSettingByElectronicLockAndAccessor(Guid electronicLockId, Guid accessorId, string fields);
        AccessSetting GetAccessSettingByElectronicLockAndAccessor(Guid electronicLockId, Guid accessorId);
        void CreateAccessSetting(AccessSetting accessSetting);
        void UpdateAccessSetting(AccessSetting dbAccessSetting, AccessSetting accessSetting);
        void DeleteAccessSetting(AccessSetting accessSetting);
    }
}
