using Application.Repositories;
using Application.Repositories.Abstractions;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.Users
{
    public record UpdateUserImageCommand : IRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class UpdateUserImageHandler : IRequestHandler<UpdateUserImageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserImageHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateUserImageCommand request, CancellationToken cancellationToken)
        {
            User? userToUpdate = (await _unitOfWork.Users.GetAsync(user => user.Email == request.Email)).FirstOrDefault();

            if (userToUpdate == null)
            {
                throw new UserNotFoundException(request.Email);
            }


            await _unitOfWork.Users.UpdateAsync(userToUpdate);
            await _unitOfWork.SaveChangesAsync();

            return default;
        }
    }
}
