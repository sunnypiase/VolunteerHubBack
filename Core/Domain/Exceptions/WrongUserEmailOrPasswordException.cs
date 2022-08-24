namespace Domain.Exceptions
{
    public class WrongUserEmailOrPasswordException : BadRequestException
    {
        // TODO: You project has the nullability feature enabled.
        // When it is enabled, you should explicitly mark nullable types. Here, the message arg should have nullable type - string?
        // This comment is related to other nullability issues as well. Such issues are marked as compile errors during the compilation  
        public WrongUserEmailOrPasswordException(string message = null) : base(message ?? "Wrong email or password")
        {

        }
    }
}
