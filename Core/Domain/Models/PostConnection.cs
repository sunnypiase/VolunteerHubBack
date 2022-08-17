namespace Domain.Models
{
    public class PostConnection
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public Post VolunteerPost { get; set; }
        public Post NeedfulPost { get; set; }
    }
}
