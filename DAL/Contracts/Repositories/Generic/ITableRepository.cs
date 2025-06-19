using Domains.Entities.Base;
using System.Linq.Expressions;

namespace DAL.Contracts.Repositories.Generic
{
    public interface ITableRepository<T> : IRepository<T> where T : BaseEntity
    {
        T FindById(Guid id);
        bool Save(T model, Guid userId);
        bool Save(T model, Guid userId, out Guid id);
        bool Create(T model, Guid creatorId, out Guid id);
        bool Update(T model, Guid updaterId, out Guid id);
        public bool UpdateCurrentState(Guid entityId, Guid userId, int newValue = 0);
        bool AddRange(IEnumerable<T> entities, Guid userId);
        public bool SaveRange(IEnumerable<T> entities, Guid userId);
        public bool ResetRange(IEnumerable<T> newEntities, Guid userId, Expression<Func<T, bool>> filterForOldEntities);
        public bool Remove(Guid id);
        bool SaveChange();
    }
}
