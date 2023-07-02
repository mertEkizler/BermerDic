using BermerDic.Api.Application.Interfaces.Repositories;
using BermerDic.Api.Domain.Models;
using BermerDic.Infrastructere.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BermerDic.Infrastructere.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly BermerDicContext _dbContext;

        protected DbSet<TEntity> entity => _dbContext.Set<TEntity>();

        public GenericRepository(BermerDicContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public int Add(TEntity entity)
        {
            this.entity.Add(entity);
            return _dbContext.SaveChanges();
        }

        public int Add(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
            {
                return 0;
            }

            entity.AddRange(entities);
            return _dbContext.SaveChanges();
        }

        public virtual async Task<int> AddAsync(TEntity entity)
        {
            await this.entity.AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public int AddOrUpdate(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddOrUpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> AsQueryable()
        {
            throw new NotImplementedException();
        }

        public Task BulkAddAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task BulkDeleteAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task BulkDeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task BulkDeleteByIdAsync(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task BulkUpdateAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TEntity>> GetAllAsync(bool noTracking = true)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public int Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}