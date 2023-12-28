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

        public async Task<int> AddAsync(TEntity entity)
        {
            await this.entity.AddAsync(entity);
            return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
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

        public async Task<int> AddOrUpdateAsync(TEntity entity)
        {
            // check the entity with the id already tracked.
            if (!this.entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
            {
                _dbContext.Update(entity);
            }

            return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return entity.AsQueryable();
        }

        public async Task BulkAddAsync(IEnumerable<TEntity> entities)
        {
            if (entities != null && entities.Any())
            {
                await entity.AddRangeAsync(entities);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task BulkDeleteAsync(IEnumerable<TEntity> entities)
        {
            if (entities != null && entities.Any())
            {
                entity.RemoveRange(entities);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task BulkDeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entitiesToDelete = entity.Where(predicate);
            entity.RemoveRange(entitiesToDelete);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task BulkDeleteByIdAsync(IEnumerable<Guid> ids)
        {
            var entitiesToDelete = await entity.Where(e => ids.Contains(e.Id)).ToListAsync();
            entity.RemoveRange(entitiesToDelete);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task BulkUpdateAsync(IEnumerable<TEntity> entities)
        {
            if (entities != null && entities.Any())
            {
                foreach (var entity in entities)
                {
                    _dbContext.Entry(entity).State = EntityState.Modified;
                }

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
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

        public async Task<int> DeleteAsync(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entity);
            }

            this.entity.Remove(entity);

            return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public Task<int> DeleteAsync(Guid Id)
        {
            var entityToBeDeleted = this.entity.Find(Id);
            return DeleteAsync(entityToBeDeleted);
        }

        public bool DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            _dbContext.RemoveRange(predicate);
            return _dbContext.SaveChanges() > 0;
        }

        public async Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            _dbContext.RemoveRange(predicate);
            var savingChanges = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return savingChanges > 0;
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = GetQueryable(noTracking, includes);

            return await query.FirstOrDefaultAsync(predicate).ConfigureAwait(false);
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

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(bool noTracking = true)
        {
            var query = noTracking ? entity.AsNoTracking() : entity;

            return await query.ToListAsync().ConfigureAwait(false);
        }

        public async Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = entity.AsQueryable();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            query = ApplyIncludes(query, includes);

            return await query.FirstOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = GetQueryable(noTracking, includes);

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync().ConfigureAwait(false);
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = GetQueryable(noTracking, includes);

            return await query.SingleOrDefaultAsync(predicate).ConfigureAwait(false);
        }

        public int Update(TEntity entity)
        {
            this.entity.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;

            return _dbContext.SaveChanges();
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            this.entity.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;

            return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
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

        private IQueryable<TEntity> GetQueryable(bool noTracking, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = entity.AsQueryable();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            query = ApplyIncludes(query, includes);

            return query;
        }
    }
}