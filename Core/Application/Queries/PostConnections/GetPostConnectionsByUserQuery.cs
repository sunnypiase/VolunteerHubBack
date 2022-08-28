using Application.Repositories;
using Domain.Enums;
using Domain.Models;
using MediatR;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Application.Queries.PostConnections
{
    public record GetPostConnectionsByUserQuery : IRequest<IEnumerable<PostConnection>>
    {
        public IEnumerable<Claim> Claims { get; init; }
        public GetPostConnectionsByUserQuery(IEnumerable<Claim> claims)
        {
            Claims = claims;
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
            int UserId = int.Parse(request.Claims
                .First(claim => claim.Type == "Id")
                .Value);

            UserRole UserRole = Enum.Parse<UserRole>(request.Claims
                .First(claim => claim.Type == ClaimTypes.Role)
                .Value);

            Expression<Func<PostConnection, bool>> function =
                UserRole == UserRole.Volunteer ?
                pc => pc.VolunteerPost.UserId == UserId :
                pc => pc.NeedfulPost.UserId == UserId;

            return await _unitOfWork.PostConnections.GetAsync(filter: function, includeProperties: new string[] { "VolunteerPost", "NeedfulPost" });
        }
    }
}
