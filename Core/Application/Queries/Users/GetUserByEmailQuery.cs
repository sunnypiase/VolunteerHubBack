using Application.Queries.Posts;
using Application.UnitOfWorks;
using Domain.Models;
using MediatR;

namespace Application.Queries.Users
{
    public record GetUserByEmailQuery(string UserEmail) : IRequest<IEnumerable<User>>;
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, IEnumerable<User>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByEmailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Users.Get(user => user.Email.Equals(request.UserEmail));
        }
    }
}
