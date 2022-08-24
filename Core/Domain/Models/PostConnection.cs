namespace Domain.Models
{
    public class PostConnection
    {
        // TODO: Inconsistency in ID naming. Here you name it as Id, but in other entities it is called EntityId
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public Post VolunteerPost { get; set; }
        public Post NeedfulPost { get; set; }
    }
}
