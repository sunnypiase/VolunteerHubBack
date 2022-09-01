using Application.Repositories;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Commands.PostConnections
{
    public record CreatePostConnectionCommand : IRequest<PostConnection>
    {
        public string Title { get; init; }
        public string Message { get; init; }
        public int VolunteerPostId { get; init; }
        public int NeedfulPostId { get; init; }
        public CreatePostConnectionCommand(string title, string message, int volunteerPostId, int needfulPostId)
        {
            Title = title;
            Message = message;
            VolunteerPostId = volunteerPostId;
            NeedfulPostId = needfulPostId;

        }
    }

    public class CreatePostConnectionHandler : IRequestHandler<CreatePostConnectionCommand, PostConnection>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePostConnectionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PostConnection> Handle(CreatePostConnectionCommand request, CancellationToken cancellationToken)
        {
            PostConnection? postConnection = new PostConnection()
            {
                Title = request.Title,
                Message = request.Message,
                VolunteerPost = await PostValidationAsync(request.VolunteerPostId, PostType.Proposition),
                NeedfulPost = await PostValidationAsync(request.NeedfulPostId, PostType.Request),
                SenderId = int.Parse(new JwtSecurityTokenHandler()
                            .ReadJwtToken("token")
                            .Claims
                            .First(claim => claim.Type == "Id")
                            .Value),
            };


            await _unitOfWork.PostConnections.InsertAsync(postConnection);
            await _unitOfWork.SaveChangesAsync();
            return postConnection;
        }
        private async Task<Post> PostValidationAsync(int id, PostType expectedType)
        {
            Post? post = await _unitOfWork.Posts.GetByIdAsync(id);

            if (post == null)
            {
                throw new PostNotFoundException(id.ToString());
            }
            else if (post.PostType != expectedType)
            {
                throw new WrongPostTypeException(id.ToString(), post.PostType.ToString(), expectedType.ToString());
            }

            return post;
        }
    }
}
