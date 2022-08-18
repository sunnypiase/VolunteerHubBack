using Application.Queries.Posts;
using Application.UnitOfWorks;
using Domain.Models;
using MediatR;

namespace Application.Queries.Users
{
    public record GetUserByIdQuery(int UserId) : IRequest<IEnumerable<User>>;
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, IEnumerable<User>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Users.Get(user => user.UserId == request.UserId);
        }
    }
}
