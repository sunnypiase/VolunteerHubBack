using Application.Repositories.Abstractions;
using Domain.Abstractions;

namespace Application.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        public ITagRepository Tags { get; }
        public IUserRepository Users { get; }
        public IPostRepository Posts { get; }
        public IPostConnectionRepository PostConnections { get; }
        public Task SaveChanges();
    }
}
