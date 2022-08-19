﻿using Application.Services;
using Application.UnitOfWorks;
using Domain.Models;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.Posts
{
    public record CreatePostCommand : IRequest<Post>
    {
        // can there be a post without an image or tags?
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public ICollection<int> TagIds { get; set; }

        [JsonConverter(typeof(ByteArrayConverter))]
        public byte[] Image { get; set; }
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
            return new Post()
            {
                Title = request.Title,
                Description = request.Description,
                UserId = request.UserId,
                User = await _unitOfWork.Users.GetById(request.UserId),
                Tags = await GetTagsByIdsAsync(request.TagIds),
                Image = request.Image
            };
        }

        private async Task<ICollection<Tag>> GetTagsByIdsAsync(ICollection<int> tagIds)
        {
            var tags = new List<Tag>(); 
            foreach (var tagId in tagIds)
            {
                tags.Add( await _unitOfWork.Tags.GetById(tagId));
            }
            return tags;
        }
    }
}
