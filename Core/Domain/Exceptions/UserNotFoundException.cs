namespace Application.Commands.Posts
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string userId) : base($"User with id = {userId} was not found")
        {
        }
    }
}