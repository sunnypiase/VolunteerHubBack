using Application.UnitOfWorks;
using Domain.Models;
using MediatR;

namespace Application.Commands.PostConnections
{

    public class CreatePostConnectionCommand : IRequest<PostConnection>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public int VolunteerPostId { get; set; }
        public int NeedfulPostId { get; set; }
    }
    public class CreatePostHandler : IRequestHandler<CreatePostConnectionCommand, PostConnection>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePostHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PostConnection> Handle(CreatePostConnectionCommand request, CancellationToken cancellationToken)
        {
            PostConnection? res = new()
            {
                Title = request.Title,
                Message = request.Message,
                VolunteerPost = await _unitOfWork.Posts.GetById(request.VolunteerPostId),
                NeedfulPost = await _unitOfWork.Posts.GetById(request.NeedfulPostId),
            };
            await _unitOfWork.PostConnections.Insert(res);
            await _unitOfWork.SaveChanges();
            return res;
        }

    }
}
