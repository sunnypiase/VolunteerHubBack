namespace WebApi.Models
{
    public record CreatePostConnectionRequestModel
    {
        public string Title { get; init; }
        public string Message { get; init; }
        public int VolunteerPostId { get; init; }
        public int NeedfulPostId { get; init; }
        public CreatePostConnectionRequestModel(string title, string message, int volunteerPostId, int needfulPostId)
        {
            Title = title;
            Message = message;
            VolunteerPostId = volunteerPostId;
            NeedfulPostId = needfulPostId;
        }
    }
}
