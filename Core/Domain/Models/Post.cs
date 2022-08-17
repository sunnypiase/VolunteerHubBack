using Domain.Enums;

namespace Domain.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public PostType PostType { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
