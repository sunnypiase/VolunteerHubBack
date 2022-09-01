using Application.Commands.Images;
using Application.Repositories;
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
        public string Email { get; init; }
        [Required]
        public IFormFile ProfileImageFile { get; init; }

        public UpdateUserImageCommand(string email, IFormFile profileImageFile)
        {
            Email = email;
            ProfileImageFile = profileImageFile;
        }
    }

    public class UpdateUserImageHandler : IRequestHandler<UpdateUserImageCommand>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateUserImageHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateUserImageCommand request, CancellationToken cancellationToken)
        {
            User? userToUpdate = (await _unitOfWork.Users.GetAsync(user => user.Email == request.Email)).FirstOrDefault();

            if (userToUpdate == null)
            {
                throw new UserNotFoundException(request.Email);
            }

            userToUpdate.ProfileImage = await _mediator.Send(new CreateImageCommand(request.ProfileImageFile), cancellationToken);

            await _unitOfWork.Users.UpdateAsync(userToUpdate);
            await _unitOfWork.SaveChangesAsync();
            return default;
        }
    }
}
