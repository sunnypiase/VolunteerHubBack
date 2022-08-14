using Domain.Abstractions;

namespace Infrastructure.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        public ITagRepository Tags { get; }
        public IUserRepository Users { get; }
        public IPostRepository Posts { get; }
        public Task SaveChanges();
    }
}
