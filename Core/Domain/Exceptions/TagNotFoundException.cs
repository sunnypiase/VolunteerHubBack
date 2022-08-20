namespace Domain.Exceptions
{
    public class TagNotFoundException : Exception
    {
        public TagNotFoundException(int tagId) : base($"Tag with id = {tagId} was not found")
        {
        }
    }
}
