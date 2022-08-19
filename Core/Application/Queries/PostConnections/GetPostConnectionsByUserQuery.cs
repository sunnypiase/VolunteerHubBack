using Application.UnitOfWorks;
using Domain.Enums;
using Domain.Models;
using MediatR;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Application.Queries.PostConnections
{
    public record GetPostConnectionsByUserQuery(IEnumerable<Claim> Claims) : IRequest<IEnumerable<PostConnection>>;

    public class GetPostConnectionsByUserIdQueryHandler : IRequestHandler<GetPostConnectionsByUserQuery, IEnumerable<PostConnection>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetPostConnectionsByUserIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PostConnection>> Handle(GetPostConnectionsByUserQuery request, CancellationToken cancellationToken)
        {
            var UserId = int.Parse(request.Claims
                .First(claim => claim.Type == "Id")
                .Value);

            var UserRole = Enum.Parse<UserRole>(request.Claims
                .First(claim => claim.Type == ClaimTypes.Role)
                .Value);

            Expression<Func<PostConnection, bool>> function =
                UserRole == UserRole.Volunteer ?
                pc => pc.VolunteerPost.UserId == UserId :
                pc => pc.NeedfulPost.UserId == UserId;

            return await _unitOfWork.PostConnections.Get(filter: function, commaSeparatedIncludeProperties: "VolunteerPost,NeedfulPost");
        }
    }
}
