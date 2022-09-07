using Application.Repositories;
using Application.Services;
using Domain.Exceptions;
using Domain.Models;
using MediatR;

namespace Application.Commands.PostConnections
{
    public record UpdatePostConnectionRevisionCommand : IRequest
    {
        public string? Token { get; init; }
        public int PostConnectionId { get; init; }
        public UpdatePostConnectionRevisionCommand(string? token, int postConnectionId)
        {
            Token = token;
            PostConnectionId = postConnectionId;
        }
    }
    public class UpdatePostConnectionRevisionHandler : IRequestHandler<UpdatePostConnectionRevisionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdatePostConnectionRevisionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdatePostConnectionRevisionCommand request, CancellationToken cancellationToken)
        {
            var userFromToken = new GetUserFromTokenService(request.Token);

            var postConnectionToUpdate = await _unitOfWork.PostConnections.GetByIdAsync(request.PostConnectionId);
            if (postConnectionToUpdate == null ||
                (postConnectionToUpdate.VolunteerPost.UserId != userFromToken.UserId &&
                postConnectionToUpdate.NeedfulPost.UserId != userFromToken.UserId))
            {
                throw new PostConnectionNotFoundException(request.PostConnectionId.ToString());
            }

            await _unitOfWork.PostConnections.UpdateAsync(new PostConnection()
            {
                PostConnectionId = postConnectionToUpdate.PostConnectionId,
                Title = postConnectionToUpdate.Title,
                Message = postConnectionToUpdate.Message,
                VolunteerPost = postConnectionToUpdate.VolunteerPost,
                NeedfulPost = postConnectionToUpdate.NeedfulPost,
                SenderId = postConnectionToUpdate.SenderId,
                SenderHasSeen = userFromToken.UserId == postConnectionToUpdate.SenderId ? true : postConnectionToUpdate.SenderHasSeen,
                ReceiverHasSeen = userFromToken.UserId == postConnectionToUpdate.SenderId ? postConnectionToUpdate.ReceiverHasSeen : true
            });

            await _unitOfWork.SaveChangesAsync();
            return default;
        }
    }

}
