using Application.Models;
using Application.Repositories;
using Application.Services;
using Domain.Enums;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.PostConnections
{
    public record GetPostConnectionByIdQuery: IRequest<PostConnectionResponse>
    {
        public string? Token { get; init; }

        public int PostConnectionId { get; init; }
        public GetPostConnectionByIdQuery(string? token, int postConnectionId)
        {
            Token = token;
            PostConnectionId = postConnectionId;
        }
    }

    public class GetPostConnectionByIdHandler : IRequestHandler<GetPostConnectionByIdQuery, PostConnectionResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetPostConnectionByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PostConnectionResponse?> Handle(GetPostConnectionByIdQuery request, CancellationToken cancellationToken)
        {
            var userFromToken = new GetUserFromTokenService(request.Token);

            Expression<Func<PostConnection, bool>> function = userFromToken.Role == UserRole.Volunteer ?
                pc => pc.VolunteerPost.UserId == userFromToken.UserId && pc.PostConnectionId == request.PostConnectionId :
                pc => pc.NeedfulPost.UserId == userFromToken.UserId && pc.PostConnectionId == request.PostConnectionId;

            return (await _unitOfWork.PostConnections.GetAsync(filter: function, includeProperties: new string[] { "VolunteerPost", "NeedfulPost" }))
                .Select(postConnection => new PostConnectionResponse(

                    postConnection.PostConnectionId,
                    userFromToken.UserId == postConnection.SenderId ? "You have sent a message" : "You have received a message",
                    postConnection.Title,
                    postConnection.Message,
                    postConnection.VolunteerPost,
                    postConnection.NeedfulPost
                )).FirstOrDefault();
        }
    }
}
