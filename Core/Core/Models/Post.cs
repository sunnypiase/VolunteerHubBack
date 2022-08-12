namespace Core.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[] Image { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
