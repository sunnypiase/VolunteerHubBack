using Application.Commands.Tags;
using Application.Repositories;
using Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Posts
{
    public record DeletePostCommand : IRequest
    {
        public int PostId { get; init; }
        public DeletePostCommand(int postId)
        {
            PostId = postId;
        }
    }
    public class DeletePostHandler : IRequestHandler<DeletePostCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePostHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Posts.DeleteAsync(request.PostId))
            {
                throw new PostNotFoundException(request.PostId.ToString());
            }

            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
