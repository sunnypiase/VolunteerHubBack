using Application.UnitOfWorks;
using Domain.Models;
using MediatR;

namespace Application.Queries.Posts
{
    public record GetPostByIdQuery(int PostId) : IRequest<IEnumerable<Post>>;

    public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, IEnumerable<Post>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPostByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Posts.Get(x => x.PostId == request.PostId/*, y => y.OrderByDescending(post => post.User), "User"*/); // example
        }
    }
}
