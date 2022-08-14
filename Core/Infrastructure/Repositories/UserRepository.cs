using Domain.Abstractions;
using Domain.Models;

namespace Infrastructure.Repositories
{
    public class UserRepository : SqlGenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }

        public override async Task<bool> Update(User entityToUpdate)
        {
            var userToUpdate = await _entity.FindAsync(entityToUpdate.UserId);

            if (userToUpdate != null)
            {
                userToUpdate.Name = entityToUpdate.Name;
                userToUpdate.Surname = entityToUpdate.Surname;
                userToUpdate.PhoneNumber = entityToUpdate.PhoneNumber;
                userToUpdate.Address = entityToUpdate.Address;
                userToUpdate.Role = entityToUpdate.Role;
                userToUpdate.Posts = entityToUpdate.Posts;
                return true;
            }
            return false;
        }
    }
}
