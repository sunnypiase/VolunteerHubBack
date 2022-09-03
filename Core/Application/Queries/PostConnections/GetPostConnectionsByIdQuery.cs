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
        public int PostConnectionId { get; init; }
        public GetPostConnectionByIdQuery(int postConnectionId)
        {
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
            var postConnection = await _unitOfWork.PostConnections.GetByIdAsync(request.PostConnectionId)
                ?? throw new PostConnectionNotFoundException(request.PostConnectionId.ToString());
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
