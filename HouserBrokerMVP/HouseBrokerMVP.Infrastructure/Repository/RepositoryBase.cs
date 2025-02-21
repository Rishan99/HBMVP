using HouseBrokerMVP.Core.Context;
using HouseBrokerMVP.Core.Entities;
using HouseBrokerMVP.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HouseBrokerMVP.Infrastructure.Repository
{
    public abstract class RepositoryBase<T, T1> : IRepositoryBase<T, T1>
        where T : class
    {
        protected readonly AppDbContext _appDbContext;

        private readonly DbSet<T> DbSet;

        public RepositoryBase(AppDbContext context)
        {
            _appDbContext = context;
            DbSet = _appDbContext.Set<T>();
        }
        public IQueryable<T> Get(bool trackEntity = false)
        {
            if (!trackEntity)
            {
                return DbSet.AsNoTracking();
            }
            return DbSet.AsQueryable();
        }

        public IQueryable<T> GetById(T1 id, bool trackEntity = false)
        {
            var queryableData = DbSet.Where(e => EF.Property<T1>(e, "Id")!.Equals(id));
            if (!trackEntity)
            {
                return queryableData.AsNoTracking(); ;
            }
            return queryableData;
        }

        public async Task Insert(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public async Task InsertRange(List<T> entity)
        {
            await DbSet.AddRangeAsync(entity);
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public async Task RemoveById(T1 id)
        {
            await DbSet.Where(e => EF.Property<T1>(e, "id")!.Equals(id)).ExecuteDeleteAsync();
        }

        public void RemoveRange(List<T> entity)
        {
            DbSet.RemoveRange(entity);
        }

        public async Task SaveChanges(CancellationToken token = default)
        {
            await _appDbContext.SaveChangesAsync(token);
        }
    }
}
