using System.Linq.Expressions;

namespace HouseBrokerMVP.Core.Repositories
{
    public interface IRepositoryBase<T, T1> where T : class
    {
        IQueryable<T> Get(bool trackEntity = false);
        IQueryable<T> GetById(T1 id, bool trackEntity = false);

        Task Insert(T entity);
        void Update(T entity);
        Task InsertRange(List<T> entity);
        void Remove(T entity);
        void RemoveRange(List<T> entity);
        Task RemoveById(T1 id);
        Task SaveChanges(CancellationToken token = default);

    }
}
