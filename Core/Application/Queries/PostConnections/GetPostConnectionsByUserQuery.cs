using Application.Repositories;
using Application.Services;
using Domain.Enums;
using Domain.Models;
using MediatR;
using System.Linq.Expressions;

namespace Application.Queries.PostConnections
{
    public record GetPostConnectionsByUserQuery : IRequest<IEnumerable<PostConnection>>
    {
        public string? Token { get; init; }
        public GetPostConnectionsByUserQuery(string? token)
        {
            Token = token;
        }
    }

    public class GetPostConnectionsByUserHandler : IRequestHandler<GetPostConnectionsByUserQuery, IEnumerable<PostConnection>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetPostConnectionsByUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PostConnection>> Handle(GetPostConnectionsByUserQuery request, CancellationToken cancellationToken)
        {
            var userFromToken = new GetUserFromTokenService(request.Token);

            Expression<Func<PostConnection, bool>> function =
                userFromToken.Role == UserRole.Volunteer ?
                pc => pc.VolunteerPost.UserId == userFromToken.UserId :
                pc => pc.NeedfulPost.UserId == userFromToken.UserId;

            return await _unitOfWork.PostConnections.GetAsync(filter: function, includeProperties: new string[] { "VolunteerPost", "NeedfulPost" });
        }
    }
}
