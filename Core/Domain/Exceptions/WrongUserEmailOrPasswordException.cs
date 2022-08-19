namespace Domain.Exceptions
{
    public class WrongUserEmailOrPasswordException : Exception
    {
        public WrongUserEmailOrPasswordException(string message = null) : base(message ?? "Wrong email or password")
        {

        }
    }
}
