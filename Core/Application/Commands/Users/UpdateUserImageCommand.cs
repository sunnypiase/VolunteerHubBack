using Application.Commands.Images;
using Application.Repositories;
using Application.Repositories.Abstractions;
using Application.Services;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Users
{
    public record UpdateUserImageCommand : IRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public IFormFile ProfileImageFile { get; set; }
    }

    public class UpdateUserImageHandler : IRequestHandler<UpdateUserImageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobRepository _blobRepository;

        public UpdateUserImageHandler(IUnitOfWork unitOfWork, IBlobRepository blobRepository)
        {
            _unitOfWork = unitOfWork;
            _blobRepository = blobRepository;
        }
        public async Task<Unit> Handle(UpdateUserImageCommand request, CancellationToken cancellationToken)
        {
            User? userToUpdate = (await _unitOfWork.Users.GetAsync(user => user.Email == request.Email)).FirstOrDefault();

            if (userToUpdate == null)
            {
                throw new UserNotFoundException(request.Email);
            }
            userToUpdate.ProfileImage = new Image() { ImageId = userToUpdate.ProfileImageId, Format = request.ProfileImageFile.ContentType.Split('/')[1] };


            await _blobRepository.UploadImage(request.ProfileImageFile, userToUpdate.ProfileImage.ToString());
            await _unitOfWork.Images.UpdateAsync(userToUpdate.ProfileImage);
            await _unitOfWork.SaveChangesAsync();
            return default;
        }
    }
}
