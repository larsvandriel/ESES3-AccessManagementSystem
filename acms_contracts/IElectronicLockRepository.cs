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
    public interface IElectronicLockRepository: IRepositoryBase<ElectronicLock>
    {
        PagedList<ShapedEntity> GetAllElectronicLocks(ElectronicLockParameters electronicLockParameters);
        ShapedEntity GetElectronicLockById(Guid electronicLockId, string fields);
        ElectronicLock GetElectronicLockById(Guid electronicLockId);
        void CreateElectronicLock(ElectronicLock electronicLock);
        void UpdateElectronicLock(ElectronicLock dbElectronicLock, ElectronicLock electronicLock);
        void DeleteElectronicLock(ElectronicLock electronicLock);
    }
}
