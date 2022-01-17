using AccessManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessManagementSystem.Entities.Extensions
{
    public static class ElectronicLockExtensions
    {
        public static void Map(this ElectronicLock dbElectronicLock, ElectronicLock electronicLock)
        {
            dbElectronicLock.SerialNumber = electronicLock.SerialNumber;
            dbElectronicLock.AccessSettings = electronicLock.AccessSettings;
        }
    }
}
