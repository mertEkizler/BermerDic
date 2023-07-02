using BermerDic.Api.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BermerDic.Api.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<int> AddAsync(TEntity entity);

        int Add(TEntity entity);

        int Add(IEnumerable<TEntity> entities);

        Task<int> UpdateAsync(TEntity entity);

        int Update(TEntity entity);

        Task<int> AddOrUpdateAsync(TEntity entity);

        int AddOrUpdate(TEntity entity);

        Task<int> DeleteAsync(TEntity entity);

        Task<int> DeleteAsync(Guid Id);

        Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate);

        int Delete(TEntity entity);

        int Delete(Guid id);

        bool DeleteRange(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> AsQueryable();

        Task<IReadOnlyList<TEntity>> GetAllAsync(bool noTracking = true);

        Task<IReadOnlyList<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> predicate, 
            bool noTracking = true, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);

        Task BulkAddAsync(IEnumerable<TEntity> entities);

        Task BulkUpdateAsync(IEnumerable<TEntity> entities);

        Task BulkDeleteAsync(IEnumerable<TEntity> entities);

        Task BulkDeleteAsync(Expression<Func<TEntity, bool>> predicate);

        Task BulkDeleteByIdAsync(IEnumerable<Guid> ids);

    }
}