using Application.UnitOfWorks;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return new PostConnection()
            {
                Title = request.Title,
                Message = request.Message,
                VolunteerPost = await _unitOfWork.Posts.GetById(request.VolunteerPostId),
                NeedfulPost = await _unitOfWork.Posts.GetById(request.NeedfulPostId),
            };
        }
        
    }
}
