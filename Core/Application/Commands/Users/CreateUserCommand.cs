using Application.UnitOfWorks;
using Domain.Enums;
using Domain.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.Users
{
    public record CreateUserCommand : IRequest<User>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public UserRole Role { get; set; }
        public ICollection<Post> Posts { get; set; }
    }

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new User()
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                Surname= request.Surname,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Role = request.Role,
                Posts = request.Posts
            });
        }
    }
}
