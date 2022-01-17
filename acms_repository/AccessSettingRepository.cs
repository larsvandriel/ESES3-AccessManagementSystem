using AccessManagementSystem.Contracts;
using AccessManagementSystem.Entities;
using AccessManagementSystem.Entities.Extensions;
using AccessManagementSystem.Entities.Helpers;
using AccessManagementSystem.Entities.Models;
using AccessManagementSystem.Entities.Parameters;
using AccessManagementSystem.Entities.ShapedEntities;

namespace AccessManagementSystem.Repository
{
    public class AccessSettingRepository: RepositoryBase<AccessSetting>, IAccessSettingRepository
    {
        private readonly ISortHelper<AccessSetting> _sortHelper;

        private readonly IDataShaper<AccessSetting> _dataShaper;

        public AccessSettingRepository(RepositoryContext repositoryContext, ISortHelper<AccessSetting> sortHelper, IDataShaper<AccessSetting> dataShaper) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public void CreateAccessSetting(AccessSetting accessSetting)
        {
            Create(accessSetting);
        }

        public void DeleteAccessSetting(AccessSetting accessSetting)
        {
            AccessSetting dbAccessSetting = GetAccessSettingByElectronicLockAndAccessor(accessSetting.ElectricalLockId, accessSetting.AccessorId);
            UpdateAccessSetting(dbAccessSetting, accessSetting);
        }

        public PagedList<ShapedEntity> GetAllAccessSettings(AccessSettingParameters accessSettingParameters)
        {
            var accessSettings = FindAll();

            var sortedAccessSettings = _sortHelper.ApplySort(accessSettings, accessSettingParameters.OrderBy);
            var shapedAccessSettings = _dataShaper.ShapeData(sortedAccessSettings, accessSettingParameters.Fields).AsQueryable();

            return PagedList<ShapedEntity>.ToPagedList(shapedAccessSettings, accessSettingParameters.PageNumber, accessSettingParameters.PageSize);
        }

        public ShapedEntity GetAccessSettingByElectronicLockAndAccessor(Guid electricalLockId, Guid accessorId, string fields)
        {
            var accessSetting = FindByCondition(accessSetting => accessSetting.Lock.Equals(electricalLockId) && accessSetting.AccessorId.Equals(accessorId)).FirstOrDefault();

            if (accessSetting == null)
            {
                accessSetting = new AccessSetting();
            }

            return _dataShaper.ShapeData(accessSetting, fields);
        }

        public AccessSetting GetAccessSettingByElectronicLockAndAccessor(Guid electronicLockId, Guid accessorId)
        {
            return FindByCondition(i => i.Lock.Equals(electronicLockId) && i.AccessorId.Equals(accessorId)).FirstOrDefault();
        }

        public void UpdateAccessSetting(AccessSetting dbAccessSetting, AccessSetting accessSetting)
        {
            dbAccessSetting.Map(accessSetting);
            Update(dbAccessSetting);
        }
    }
}