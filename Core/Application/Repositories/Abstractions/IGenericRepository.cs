using System.Linq.Expressions;

namespace Domain.Abstractions
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string commaSeparatedIncludeProperties = "");
        public Task<TEntity> GetById(object id);
        public Task<bool> Insert(TEntity entity);
        public Task<bool> Update(TEntity entityToUpdate);
        public Task<bool> Delete(object id);
    }
}