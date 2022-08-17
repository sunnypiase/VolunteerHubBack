using Application.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Commands.Users
{
    public record LoginUserCommand : IRequest<string>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        public LoginUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = _unitOfWork.Users.Get(user=>user.)
        }
    }
}
