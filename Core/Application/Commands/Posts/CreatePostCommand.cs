using Application.Repositories;
using Application.Services;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.Posts
{
    public record CreatePostCommand : IRequest<Post>
    {
        // can there be a post without an image or tags?
        public string Title { get; init; }
        public string Description { get; init; }
        public int UserId { get; init; }
        public ICollection<int> TagIds { get; init; }

        [JsonConverter(typeof(ByteArrayConverter))]
        public byte[] Image { get; init; }
        public CreatePostCommand(string title, string description, int userId, ICollection<int> tagIds, byte[] image)
        {
            Title = title;
            Description = description;
            UserId = userId;
            TagIds = tagIds;
            Image = image;
        }
    }

    public class CreatePostHandler : IRequestHandler<CreatePostCommand, Post>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePostHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Post> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var postOwner = await _unitOfWork.Users.GetByIdAsync(request.UserId);

            if (postOwner == null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            var postType = postOwner.Role == UserRole.Needful ? PostType.Request : PostType.Proposition;

            var post = new Post()
            {
                Title = request.Title,
                Description = request.Description,
                UserId = request.UserId,
                User = postOwner,
                Tags = await GetTagsByIdsAsync(request.TagIds),
                Image = request.Image,
                PostType = postType
            };

            await _unitOfWork.Posts.InsertAsync(post);
            await _unitOfWork.SaveChangesAsync();
            return post;
        }

        private async Task<ICollection<Tag>> GetTagsByIdsAsync(ICollection<int> tagIds)
        {
            var tags = new List<Tag>();
            foreach (var tagId in tagIds)
            {
                var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
                if (tag != null)
                {
                    tags.Add(tag);
                }
            }
            return tags;
        }
    }
}
