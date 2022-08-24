using System.Linq.Expressions;

namespace Domain.Abstractions
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string commaSeparatedIncludeProperties = "");

        // TODO: I think, the IdType should be defined on the level of the interface since in GetById and Delete it will be the same
        public Task<TEntity?> GetById<IdType>(IdType id);
        public Task<bool> Insert(TEntity entity);
        public Task<bool> Update(TEntity entityToUpdate);
        public Task<bool> Delete<IdType>(IdType id);
    }
}