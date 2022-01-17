using AccessManagementSystem.Contracts;
using AccessManagementSystem.Entities;
using AccessManagementSystem.Entities.Extensions;
using AccessManagementSystem.Entities.Helpers;
using AccessManagementSystem.Entities.Models;
using AccessManagementSystem.Entities.Parameters;
using AccessManagementSystem.Entities.ShapedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessManagementSystem.Repository
{
    public class ElectronicLockRepository: RepositoryBase<ElectronicLock>, IElectronicLockRepository
    {
        private readonly ISortHelper<ElectronicLock> _sortHelper;

        private readonly IDataShaper<ElectronicLock> _dataShaper;

        public ElectronicLockRepository(RepositoryContext repositoryContext, ISortHelper<ElectronicLock> sortHelper, IDataShaper<ElectronicLock> dataShaper) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public void CreateElectronicLock(ElectronicLock electronicLock)
        {
            Create(electronicLock);
        }

        public void DeleteElectronicLock(ElectronicLock electronicLock)
        {
            ElectronicLock dbElectronicLock = GetElectronicLockById(electronicLock.Id);
            UpdateElectronicLock(dbElectronicLock, electronicLock);
        }

        public PagedList<ShapedEntity> GetAllElectronicLocks(ElectronicLockParameters electronicLockParameters)
        {
            var electronicLocks = FindAll();

            SearchBySerialNumber(ref electronicLocks, electronicLockParameters.SerialNumber);

            var sortedElectronicLocks = _sortHelper.ApplySort(electronicLocks, electronicLockParameters.OrderBy);
            var shapedElectronicLocks = _dataShaper.ShapeData(sortedElectronicLocks, electronicLockParameters.Fields).AsQueryable();

            return PagedList<ShapedEntity>.ToPagedList(shapedElectronicLocks, electronicLockParameters.PageNumber, electronicLockParameters.PageSize);
        }

        public ShapedEntity GetElectronicLockById(Guid electronicLockId, string fields)
        {
            var electronicLock = FindByCondition(electronicLock => electronicLock.Id.Equals(electronicLockId)).FirstOrDefault();

            if (electronicLock == null)
            {
                electronicLock = new ElectronicLock();
            }

            return _dataShaper.ShapeData(electronicLock, fields);
        }

        public ElectronicLock GetElectronicLockById(Guid electronicLockId)
        {
            return FindByCondition(i => i.Id.Equals(electronicLockId)).FirstOrDefault();
        }

        public void UpdateElectronicLock(ElectronicLock dbElectronicLock, ElectronicLock electronicLock)
        {
            dbElectronicLock.Map(electronicLock);
            Update(dbElectronicLock);
        }

        private void SearchBySerialNumber(ref IQueryable<ElectronicLock> electronicLocks, string electronicLockSerialNumber)
        {
            if (!electronicLocks.Any() || string.IsNullOrWhiteSpace(electronicLockSerialNumber))
            {
                return;
            }

            electronicLocks = electronicLocks.Where(i => i.SerialNumber.ToLower().Contains(electronicLockSerialNumber.Trim().ToLower()));
        }
    }
}
