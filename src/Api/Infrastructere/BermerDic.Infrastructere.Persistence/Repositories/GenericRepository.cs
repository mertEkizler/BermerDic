using BermerDic.Api.Application.Interfaces.Repositories;
using BermerDic.Api.Domain.Models;
using BermerDic.Infrastructere.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
            // check the entity with the id already tracked.
            if (!this.entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
            {
                _dbContext.Update(entity);
            }

            return _dbContext.SaveChanges();
        }

        public Task<int> AddOrUpdateAsync(TEntity entity)
        {
            // check the entity with the id already tracked.
            if (!this.entity.Local.Any(i=> EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
            {
                _dbContext.Update(entity);
            }

            return _dbContext.SaveChangesAsync();
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
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entity);
            }

            this.entity.Remove(entity);

            return _dbContext.SaveChanges();
        }

        public int Delete(Guid id)
        {
            var entityToBeDeleted = this.entity.Find(id);
            return Delete(entityToBeDeleted);
        }

        public virtual Task<int> DeleteAsync(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entity);
            }

            this.entity.Remove(entity);

            return _dbContext.SaveChangesAsync();
        }

        public virtual Task<int> DeleteAsync(Guid Id)
        {
            var entityToBeDeleted = this.entity.Find(Id);
            return DeleteAsync(entityToBeDeleted);
        }

        public virtual bool DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            _dbContext.RemoveRange(predicate);
            return _dbContext.SaveChanges() > 0;
        }

        public virtual async Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            _dbContext.RemoveRange(predicate);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = entity.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            query = ApplyIncludes(query, includes);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
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
            this.entity.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;

            return _dbContext.SaveChanges();
        }

        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            this.entity.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;

            return await _dbContext.SaveChangesAsync();
        }

        private static IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
        {
            if (includes != null)
            {
                foreach (var includeItem in includes)
                {
                    query = query.Include(includeItem);
                }
            }

            return query;
        }
    }
}