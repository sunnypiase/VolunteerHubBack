﻿using Application.Repositories;
using Application.Services;
using Domain.Attributes;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.Users
{
    public record RegisterUserCommand : IRequest
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
        [Required]
        [StringToEnum(typeof(UserRole), ErrorMessage = "Role is not valid")]
        public string Role { get; init; }
        public RegisterUserCommand(string name, string surname, string email, string password, string repeatPassword, string phoneNumber, string address, string role)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            RepeatPassword = repeatPassword;
            PhoneNumber = phoneNumber;
            Address = address;
            Role = role;
        }
    }
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashingService _hashingService;
        public RegisterUserHandler(IUnitOfWork unitOfWork, IHashingService hashingService)
        {
            _unitOfWork = unitOfWork;
            _hashingService = hashingService;
        }
        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = (await _unitOfWork.Users.GetAsync(user => user.Email == request.Email)).FirstOrDefault();

            if (existingUser != null)
            {
                throw new EmailTakenByOtherUserException(request.Email);
            }

            var newUser = new User()
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                Password = _hashingService.GetHash(request.Password),
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Role = Enum.Parse<UserRole>(request.Role)
            };

            await _unitOfWork.Users.InsertAsync(newUser);
            await _unitOfWork.SaveChangesAsync();
            return default;
        }
    }
}
