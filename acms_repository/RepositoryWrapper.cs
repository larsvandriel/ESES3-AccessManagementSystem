using AccessManagementSystem.Contracts;
using AccessManagementSystem.Entities;
using AccessManagementSystem.Entities.Helpers;
using AccessManagementSystem.Entities.Models;

namespace AccessManagementSystem.Repository
{
    public class RepositoryWrapper: IRepositoryWrapper
    {
        private RepositoryContext _repoContext;

        private IAccessSettingRepository _accessSetting;
        private ISortHelper<AccessSetting> _accessSettingSortHelper;
        private IDataShaper<AccessSetting> _accessSettingDataShaper;

        private IElectronicLockRepository _electronicLock;
        private ISortHelper<ElectronicLock> _electronicLockSortHelper;
        private IDataShaper<ElectronicLock> _electronicLockDataShaper;

        public IAccessSettingRepository AccessSetting
        {
            get
            {
                if (_accessSetting == null)
                {
                    _accessSetting = new AccessSettingRepository(_repoContext, _accessSettingSortHelper, _accessSettingDataShaper);
                }

                return _accessSetting;
            }
        }

        public IElectronicLockRepository ElectronicLock
        {
            get
            {
                if (_electronicLock == null)
                {
                    _electronicLock = new ElectronicLockRepository(_repoContext, _electronicLockSortHelper, _electronicLockDataShaper);
                }

                return _electronicLock;
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext, ISortHelper<AccessSetting> accessSettingSortHelper, IDataShaper<AccessSetting> accessSettingDataShaper, ISortHelper<ElectronicLock> electronicLockSortHelper, IDataShaper<ElectronicLock> electronicLockDataShaper)
        {
            _repoContext = repositoryContext;
            _accessSettingSortHelper = accessSettingSortHelper;
            _accessSettingDataShaper = accessSettingDataShaper;
            _electronicLockSortHelper = electronicLockSortHelper;
            _electronicLockDataShaper = electronicLockDataShaper;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
