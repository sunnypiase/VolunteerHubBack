using System.Linq.Expressions;

namespace Domain
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string commaSeparatedIncludeProperties = "");
        public Task<TEntity> GetById(object id);
        public Task<TEntity> Insert(TEntity entity);
        public Task Delete(object id);
        public Task Delete(TEntity entityToDelete);
        public Task Update(TEntity entityToUpdate);
    }
}