using Application.UnitOfWorks;
using Domain.Exceptions;
using Domain.Models;
using MediatR;

namespace Application.Queries.Posts
{
    public record GetPostByIdQuery(int PostId) : IRequest<Post>;

    public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, Post>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPostByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Post> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Posts.GetById(request.PostId) ?? throw new PostNotFoundException(request.PostId.ToString());
        }
    }
}
