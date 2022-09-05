﻿using Application.Commands.Users;
using Application.Repositories;
using Application.Repositories.Abstractions;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace Application.Commands.Posts
{
    public record UpdatePostCommand : IRequest
    {
        [Required]
        public int PostId { get; init; }
        [Required]
        public string Title { get; init; }
        [Required]
        public string Description { get; init; }
        [Required]
        public IFormFile PostImageFile { get; init; }
        [Required]
        public ICollection<Tag> Tags { get; init; }

        UpdatePostCommand(int postId, string title, string description, IFormFile postImageFile,
            ICollection<Tag> tags)
        {
            PostId = PostId;
            Title = title;
            Description = description;
            PostImageFile = postImageFile;
            Tags = tags;
        }
    }

    public class UpdatePostHandler : IRequestHandler<UpdatePostCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobRepository _blobRepository;
        public UpdatePostHandler(IUnitOfWork unitOfWork, IBlobRepository blobRepository)
        {
            _unitOfWork = unitOfWork;
            _blobRepository = blobRepository;
        }
        public async Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            Post? postToUpdate = (await _unitOfWork.Posts.GetAsync(post => post.PostId == request.PostId)).FirstOrDefault();

            if (postToUpdate == null)
            {
                throw new PostNotFoundException(request.PostId.ToString());
            }

            postToUpdate.Title = request.Title;
            postToUpdate.Description = request.Description;
            postToUpdate.Tags = request.Tags;
            postToUpdate.PostImage = new Image() { ImageId = postToUpdate.PostImageId, Format = request.PostImageFile.ContentType.Split('/')[1] };


            await _blobRepository.UploadImage(request.PostImageFile, postToUpdate.PostImage.ToString());
            await _unitOfWork.Images.UpdateAsync(postToUpdate.PostImage);
            await _unitOfWork.Posts.UpdateAsync(postToUpdate);
            await _unitOfWork.SaveChangesAsync();
            return default;
        }
    }

}