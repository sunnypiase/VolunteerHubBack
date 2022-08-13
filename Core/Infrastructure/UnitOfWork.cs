using Domain;
using Domain.Models;

namespace Infrastructure
{
    public class UnitOfWork : IDisposable
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Post> _postRepository;
        private readonly IGenericRepository<Tag> _tagRepository;
        public UnitOfWork(
            ApplicationContext applicationContext,
            IGenericRepository<User> userRepository,
            IGenericRepository<Post> postRepository,
            IGenericRepository<Tag> tagRepository)
        {
            _applicationContext = applicationContext;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _tagRepository = tagRepository;
        }

        public IGenericRepository<User> UserRepository { get => _userRepository; }
        public IGenericRepository<Post> PostRepository { get => _postRepository; }
        public IGenericRepository<Tag> TagRepository { get => _tagRepository; }
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
