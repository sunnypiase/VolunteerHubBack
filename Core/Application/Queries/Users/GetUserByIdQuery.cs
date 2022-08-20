using Application.Commands.Posts;
using Application.UnitOfWorks;
using Domain.Models;
using MediatR;

namespace Application.Queries.Users
{
    public record GetUserByIdQuery(int UserId) : IRequest<User>;
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return (await _unitOfWork.Users.GetById(request.UserId)) ?? throw new UserNotFoundException(request.UserId);
        }
    }
}
