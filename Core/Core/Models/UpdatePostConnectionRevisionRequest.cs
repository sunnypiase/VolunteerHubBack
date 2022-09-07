namespace WebApi.Models
{
    public record UpdatePostConnectionRevisionRequest
    {
        public int PostConnectionId { get; init; }
        public UpdatePostConnectionRevisionRequest(int postConnectionId)
        {
            PostConnectionId = postConnectionId;
        }
    }
}
