﻿using Application.Commands.Images;
using Application.Repositories;
using Application.Services;
using Domain.Attributes;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Users
{
    public record UpdateUserInfoCommand : IRequest
    {
        [Required(ErrorMessage = "Name is reqired")]
        [StringLength(50, ErrorMessage = "Name must be between 2 and 50 characters", MinimumLength = 2)]
        public string Name { get; init; }
        [Required(ErrorMessage = "Surname is reqired")]
        [StringLength(50, ErrorMessage = "Surname must be between 2 and 50 characters", MinimumLength = 2)]
        public string Surname { get; init; }
        [Required]
        [EmailAddress]
        public string Email { get; init; }
        [Required(ErrorMessage = "Password is reqired")]
        [StringLength(20, ErrorMessage = "Password must be between 8 and 20 characters", MinimumLength = 8)]
        public string Password { get; init; }
        [Required(ErrorMessage = "Password is reqired")]
        [StringLength(20, ErrorMessage = "Password must be between 8 and 20 characters", MinimumLength = 8)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string RepeatPassword { get; init; }
        [Required]
        [Phone]
        public string PhoneNumber { get; init; }
        public string Address { get; init; }
        public UpdateUserInfoCommand(string name, string surname, string email, string password, string repeatPassword, string phoneNumber, string address)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            RepeatPassword = repeatPassword;
            PhoneNumber = phoneNumber;
            Address = address;
        }
    }


    public class UpdateUserInfoHandler : IRequestHandler<UpdateUserInfoCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashingService _hashingService;
        public UpdateUserInfoHandler(IUnitOfWork unitOfWork, IHashingService hashingService)
        {
            _unitOfWork = unitOfWork;
            _hashingService = hashingService;
        }
        public async Task<Unit> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
        {
            byte[]? passwordHash = _hashingService.GetHash(request.Password);
            User? userToUpdate = (await _unitOfWork.Users.GetAsync(user => user.Email == request.Email && user.Password == passwordHash)).FirstOrDefault();

            if (userToUpdate == null)
            {
                throw new UserNotFoundException(request.Email); 
            }

            userToUpdate.Name = request.Name;
            userToUpdate.Surname = request.Surname;
            userToUpdate.Address = request.Address;
            userToUpdate.PhoneNumber = request.PhoneNumber;

            await _unitOfWork.Users.UpdateAsync(userToUpdate);
            await _unitOfWork.SaveChangesAsync();
            return default;
        }
    }
}
