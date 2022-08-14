using Domain.Abstractions;
using Domain.Models;

namespace Infrastructure.Repositories
{
    public class PostRepository : SqlGenericRepository<Post>, IPostRepository
    {
        public PostRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }

        public override async Task<bool> Update(Post entityToUpdate)
        {
            var postToUpdate = await _entity.FindAsync(entityToUpdate.UserId);

            if (postToUpdate != null)
            {
                postToUpdate.Title = entityToUpdate.Title;
                postToUpdate.Description = entityToUpdate.Description;
                postToUpdate.Image = entityToUpdate.Image;
                postToUpdate.UserId = entityToUpdate.UserId;
                postToUpdate.User = entityToUpdate.User;
                postToUpdate.Tags = entityToUpdate.Tags;
                return true;
            }
            return false;
        }
    }
}
