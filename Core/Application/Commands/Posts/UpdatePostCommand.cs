﻿using Application.Repositories;
using Application.Repositories.Abstractions;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace Application.Commands.Posts
{
    public class UpdatePostCommand : IRequest
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile PostImageFile { get; set; }
        [Required]
        public ICollection<int> TagIds { get; set; }
    }
     
    public class UpdatePostHandler : IRequestHandler<UpdatePostCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdatePostHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            Post? postToUpdate = await _unitOfWork.Posts.GetByIdAsync(request.PostId);

            if (postToUpdate == null)
            {
                throw new PostNotFoundException(request.PostId.ToString());
            }

            postToUpdate.Title = request.Title;
            postToUpdate.Description = request.Description;
            postToUpdate.Tags = await GetTagsByIdsAsync(request.TagIds);

            
            await _unitOfWork.Posts.UpdateAsync(postToUpdate);
            await _unitOfWork.SaveChangesAsync();
            return default;
        }

        private async Task<ICollection<Tag>> GetTagsByIdsAsync(ICollection<int> tagIds)
        {
            List<Tag>? tags = new List<Tag>();
            foreach (int tagId in tagIds)
            {
                Tag? tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
                if (tag != null)
                {
                    tags.Add(tag);
                }
            }
            return tags;
        }
    }

}
