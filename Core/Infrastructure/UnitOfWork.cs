using Application.Repositories;
using Application.Repositories.Abstractions;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IPostConnectionRepository _postConnectionRepository;

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

        public async Task SaveChangesAsync()
        {
            await _applicationContext.SaveChangesAsync();
        }
    }
}
