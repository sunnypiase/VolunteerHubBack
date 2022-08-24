using Application.UnitOfWorks;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using MediatR;

namespace Application.Commands.PostConnections
{
    // TODO: I would propose to use records for command/query declaration.
    public class CreatePostConnectionCommand : IRequest<PostConnection>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public int VolunteerPostId { get; set; }
        public int NeedfulPostId { get; set; }
    }

    // TODO: Naming inconsistency - as for me, the handler should have the name CreatePostConnectionHandler
    public class CreatePostHandler : IRequestHandler<CreatePostConnectionCommand, PostConnection>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePostHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PostConnection> Handle(CreatePostConnectionCommand request, CancellationToken cancellationToken)
        {
            var postConnection = new PostConnection()
            {
                Title = request.Title,
                Message = request.Message,
                VolunteerPost = await PostValidation(request.VolunteerPostId, PostType.PROPOSITION),
                NeedfulPost = await PostValidation(request.NeedfulPostId, PostType.REQUEST)
            };

            await _unitOfWork.PostConnections.Insert(postConnection);
            await _unitOfWork.SaveChanges();
            return postConnection;
        }

        // TODO: By conventions, if method return Task type and may be called by other methods, it should have the Async suffix
        // Controller actions, MediatR`s Handle method, test methods in NUnit do not follow this since they are called by the framework, not by your code
        private async Task<Post> PostValidation(int id, PostType expectedType)
        {
            var post = await _unitOfWork.Posts.GetById(id);

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
