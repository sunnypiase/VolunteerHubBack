using Application.UnitOfWorks;
using Domain.Models;
using MediatR;

namespace Application.Commands.Posts
{
    public record CreatePostCommand : IRequest<Post>
    {
        // can there be a post without an image or tags?
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }

    public class CreatePostHandler : IRequestHandler<CreatePostCommand, Post>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePostHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<Post> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Post()
            {
                Title = request.Title,
                Description = request.Description,
                Image = request.Image,
                UserId = request.UserId,
                User = request.User,
                Tags = request.Tags,
            });
        }
    }
}
