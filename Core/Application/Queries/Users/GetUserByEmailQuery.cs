using Application.UnitOfWorks;
using Domain.Exceptions;
using Domain.Models;
using MediatR;

namespace Application.Queries.Users
{
    public record GetUserByEmailQuery(string UserEmail) : IRequest<User>;
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, User>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByEmailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            return (await _unitOfWork.Users.Get(user => user.Email.Equals(request.UserEmail))).FirstOrDefault() ?? throw new UserNotFoundException(request.UserEmail);
        }
    }
}
