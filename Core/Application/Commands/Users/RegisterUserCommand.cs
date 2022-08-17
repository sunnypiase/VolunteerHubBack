using Application.Services;
using Application.UnitOfWorks;
using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.Commands.Users
{
    public record RegisterUserCommand : IRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public UserRole Role { get; set; }
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
            if (request.Role == UserRole.Admin)
            {
                throw new Exception();// TODO: Add custom exception (maybe i should not check this)
            }

            var existingUser = (await _unitOfWork.Users.Get(user => user.Email == request.Email)).FirstOrDefault();

            if (existingUser != null)
            {
                throw new Exception();// TODO: Add custom exception
            }

            var newUser = new User()
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                Password = _hashingService.GetHash(request.Password),
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Role = request.Role
            };

            await _unitOfWork.Users.Insert(newUser);
            await _unitOfWork.SaveChanges();
            return default;
        }
    }
}
