using Domain.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class PostRepository : SqlGenericRepository<Post>, IPostRepository
    {
        public PostRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }

        public override async Task<bool> Update(Post entityToUpdate)
        {
            Post? postToUpdate = await _entity.FindAsync(entityToUpdate.UserId);

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
        public override async Task<Post?> GetById<IdType>(IdType id)
        {
            if (id == null)
            {
                return null;
            }
            return await _entity.Include("User").Include(post => post.Tags).FirstOrDefaultAsync(post => post.PostId == int.Parse(id.ToString()));
        }
    }
}
