using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessManagementSystem.Contracts
{
    public interface IRepositoryWrapper
    {
        IAccessSettingRepository AccessSetting { get; }
        IElectronicLockRepository ElectronicLock { get; }

        void Save();
    }
}
