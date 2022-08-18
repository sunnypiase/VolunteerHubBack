using Application.Services;
using Application.UnitOfWorks;
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
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is reqired")]
        [StringLength(50, ErrorMessage = "Surname must be between 2 and 50 characters", MinimumLength = 2)]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is reqired")]
        [StringLength(20, ErrorMessage = "Password must be between 8 and 20 characters", MinimumLength = 8)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password is reqired")]
        [StringLength(20, ErrorMessage = "Password must be between 8 and 20 characters", MinimumLength = 8)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string RepeatPassword { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [Required]
        [StringToEnum(typeof(UserRole), ErrorMessage = "Role is not valid")]
        public string Role { get; set; }
    }
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HashingService _hashingService;
        public RegisterUserCommandHandler(IUnitOfWork unitOfWork, HashingService hashingService)
        {
            _unitOfWork = unitOfWork;
            _hashingService = hashingService;
        }
        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = (await _unitOfWork.Users.Get(user => user.Email == request.Email)).FirstOrDefault();

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

            await _unitOfWork.Users.Insert(newUser);
            await _unitOfWork.SaveChanges();
            return default;
        }
    }
}
