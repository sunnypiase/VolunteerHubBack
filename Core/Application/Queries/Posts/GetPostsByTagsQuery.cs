using Application.UnitOfWorks;
using Domain.Models;
using MediatR;

namespace Application.Queries.Posts
{
    public record GetPostsByTagsQuery(IEnumerable<Tag> Tags) : IRequest<IEnumerable<Post>>;

    public class GetPostsByTagsHandler : IRequestHandler<GetPostsByTagsQuery, IEnumerable<Post>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPostsByTagsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Post>> Handle(GetPostsByTagsQuery request, CancellationToken cancellationToken)
        {
            // TODO: I would propose you to profile this query since there is a closure, which will probably not be translated to SQL
            return await _unitOfWork.Posts.Get(post => request.Tags.All(tag => post.Tags.Contains(tag)));
        }
    }
}
