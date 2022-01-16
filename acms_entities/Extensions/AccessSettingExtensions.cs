using AccessManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessManagementSystem.Entities.Extensions
{
    public static class AccessSettingExtensions
    {
        public static void Map(this AccessSetting dbAccessSetting, AccessSetting accessSetting)
        {
            dbAccessSetting.AllowedToEnter = accessSetting.AllowedToEnter;
        }

        public static bool IsObjectNull(this AccessSetting accessSetting)
        {
            return accessSetting == null;
        }

        public static bool IsEmptyObject(this AccessSetting accessSetting)
        {
            return accessSetting.Lock.Equals(null) || accessSetting.Accessor.Equals(null);
        }
    }
}
