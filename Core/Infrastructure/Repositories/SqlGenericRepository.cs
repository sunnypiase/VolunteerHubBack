using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class SqlGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationContext _applicationContext;
        protected readonly DbSet<TEntity> _entity;
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
        public virtual async Task<bool> Insert(TEntity entity)
        {
            await _entity.AddAsync(entity);
            return true;
        }
        public virtual async Task<bool> Delete(object id)
        {
            var entityToDelete = await _entity.FindAsync(id);

            if (entityToDelete != null)
            {
                _entity.Remove(entityToDelete);
                return true;
            }
            return false;
        }
        public async virtual Task<bool> Update(TEntity entityToUpdate)//Maybe i should throw NotImplementedException here???
        {
            await Task.Run(() => _entity.Update(entityToUpdate));
            return true;
        }
    }
}