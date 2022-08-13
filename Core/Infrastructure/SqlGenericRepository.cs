using Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure
{
    public class SqlGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ApplicationContext _applicationContext;
        internal DbSet<TEntity> _entity;
        public SqlGenericRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _entity = _applicationContext.Set<TEntity>();
        }
        public virtual async Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string commaSeparatedIncludeProperties = "")
        {
            IQueryable<TEntity> query = _entity;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in commaSeparatedIncludeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }
        public virtual async Task<TEntity> GetById(object id)
        {
            return await _entity.FindAsync(id);
        }
        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            return (await _entity.AddAsync(entity)).Entity;
        }
        public virtual async Task Delete(object id)
        {
            var entityToDelete = await _entity.FindAsync(id);
            if (entityToDelete != null)
            {
                await Delete(entityToDelete);
            }
        }
        public virtual async Task Delete(TEntity entityToDelete)
        {
            if (_applicationContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _entity.Attach(entityToDelete);
            }
            await Task.Run(() => _entity.Remove(entityToDelete));
        }
        public async virtual Task Update(TEntity entityToUpdate)
        {
            await Task.Run(() => _entity.Update(entityToUpdate));
        }
    }
}