using Application.Models;
using Application.Repositories;
using Application.Services;
using Domain.Enums;
using Domain.Exceptions;
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
    public record GetPostConnectionByIdQuery : IRequest<PostConnectionResponse>
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

            PostConnection postConnection;

            if (userFromToken.Role == UserRole.Admin)
            {
                postConnection = await _unitOfWork.PostConnections.GetByIdAsync(request.PostConnectionId)
                ?? throw new PostConnectionNotFoundException(request.PostConnectionId.ToString());
            }
            else
            {
                Expression<Func<PostConnection, bool>> function = userFromToken.Role == UserRole.Volunteer ?
                                pc => pc.VolunteerPost.UserId == userFromToken.UserId && pc.PostConnectionId == request.PostConnectionId :
                                pc => pc.NeedfulPost.UserId == userFromToken.UserId && pc.PostConnectionId == request.PostConnectionId;

                postConnection = (await _unitOfWork.PostConnections.GetAsync(filter: function, includeProperties: new string[] { "VolunteerPost", "NeedfulPost" })).FirstOrDefault();

                if (postConnection == null)
                    return null;
            }
            return new PostConnectionResponse(
                    postConnection.PostConnectionId,
                    "Notification",
                    postConnection.Title,
                    postConnection.Message,
                    postConnection.VolunteerPost,
                    postConnection.NeedfulPost);
        }
    }
}
