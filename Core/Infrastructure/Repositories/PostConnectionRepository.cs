using Application.Repositories.Abstractions;
using Domain.Models;

namespace Infrastructure.Repositories
{
    internal class PostConnectionRepository : SqlGenericRepository<PostConnection, int>, IPostConnectionRepository
    {
        public PostConnectionRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }
        public override async Task<bool> UpdateAsync(PostConnection entityToUpdate)
        {
            PostConnection? postToUpdate = await _entity.FindAsync(entityToUpdate.PostConnectionId);

            if (postToUpdate != null)
            {
                postToUpdate.Title = entityToUpdate.Title;
                postToUpdate.Message = entityToUpdate.Message;
                postToUpdate.VolunteerPost = entityToUpdate.VolunteerPost;
                postToUpdate.NeedfulPost = entityToUpdate.NeedfulPost;
                postToUpdate.SenderId = entityToUpdate.SenderId;
                return true;
            }
            return false;
        }
    }
}
