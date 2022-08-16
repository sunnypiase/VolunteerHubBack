using Application.UnitOfWorks;
using Domain.Abstractions;
using Domain.Models;

namespace Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        public UnitOfWork(
            ApplicationContext applicationContext,
            IUserRepository userRepository,
            IPostRepository postRepository,
            ITagRepository tagRepository)
        {
            _applicationContext = applicationContext;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _tagRepository = tagRepository;
        }

        public IUserRepository Users { get => _userRepository; }
        public IPostRepository Posts { get => _postRepository; }
        public ITagRepository Tags { get => _tagRepository; }
        public async Task SaveChanges()
        {
            await _applicationContext.SaveChangesAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _applicationContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
