using Application.Models;
using Application.Repositories;
using Application.Services;
using Domain.Enums;
using Domain.Models;
using MediatR;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Queries.PostConnections
{
    public record GetPostConnectionsByUserQuery : IRequest<IEnumerable<PostConnectionResponse>>
    {
        public string? Token { get; init; }
        public GetPostConnectionsByUserQuery(string? token)
        {
            Token = token;
        }
    }

    public class GetPostConnectionsByUserHandler : IRequestHandler<GetPostConnectionsByUserQuery, IEnumerable<PostConnectionResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetPostConnectionsByUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PostConnectionResponse>> Handle(GetPostConnectionsByUserQuery request, CancellationToken cancellationToken)
        {
            var userFromToken = new GetUserFromTokenService(request.Token);

            Expression<Func<PostConnection, bool>> function =
                userFromToken.Role == UserRole.Volunteer ?
                pc => pc.VolunteerPost.UserId == userFromToken.UserId :
                pc => pc.NeedfulPost.UserId == userFromToken.UserId;


            return (await _unitOfWork.PostConnections.GetAsync(filter: function, includeProperties: new string[] { "VolunteerPost", "NeedfulPost" }))
                .Select(postConnection => new PostConnectionResponse() { // TODO make it async

                    PostConnectionId = postConnection.PostConnectionId,
                    Header = userFromToken.UserId == postConnection.SenderId ? "You have sent a message" : "You have received a message",
                    Title = postConnection.Title,
                    Message= postConnection.Message,
                    NeedfulPost =  _unitOfWork.Posts.GetByIdAsync(postConnection.VolunteerPost.PostId).Result, 
                    VolunteerPost = _unitOfWork.Posts.GetByIdAsync(postConnection.NeedfulPost.PostId).Result}
                );
        }
    }
}