using Application.Repositories.Abstractions;
using Application.UnitOfWorks;
using Domain.Abstractions;

namespace Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        public readonly IPostConnectionRepository _postConnectionRepository;

        public UnitOfWork(
            ApplicationContext applicationContext,
            IUserRepository userRepository,
            IPostRepository postRepository,
            ITagRepository tagRepository,
            IPostConnectionRepository postConnectionRepository)
        {
            _applicationContext = applicationContext;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _postConnectionRepository = postConnectionRepository;
        }

        public IUserRepository Users => _userRepository;
        public IPostRepository Posts => _postRepository;
        public ITagRepository Tags => _tagRepository;
        public IPostConnectionRepository PostConnections => _postConnectionRepository;

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
