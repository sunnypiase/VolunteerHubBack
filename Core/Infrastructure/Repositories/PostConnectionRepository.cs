using Application.Repositories.Abstractions;
using Domain.Models;

namespace Infrastructure.Repositories
{
    internal class PostConnectionRepository : SqlGenericRepository<PostConnection>, IPostConnectionRepository
    {
        public PostConnectionRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }
        public override async Task<bool> Update(PostConnection entityToUpdate)
        {
            PostConnection? postToUpdate = await _entity.FindAsync(entityToUpdate.Id);

            if (postToUpdate != null)
            {
                postToUpdate.Title = entityToUpdate.Title;
                postToUpdate.Message = entityToUpdate.Message;
                postToUpdate.VolunteerPost = entityToUpdate.VolunteerPost;
                postToUpdate.NeedfulPost = entityToUpdate.NeedfulPost;
                return true;
            }
            return false;
        }
    }
}
