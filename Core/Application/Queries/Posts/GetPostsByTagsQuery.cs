using Application.Repositories;
using Domain.Models;
using MediatR;

namespace Application.Queries.Posts
{
    public record GetPostsByTagsQuery : IRequest<IEnumerable<Post>>
    {
        public IEnumerable<int> TagIds { get; init; }
        public GetPostsByTagsQuery(IEnumerable<int> tagIds)
        {
            TagIds = tagIds;
        }
    }

    public class GetPostsByTagsHandler : IRequestHandler<GetPostsByTagsQuery, IEnumerable<Post>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPostsByTagsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Post>> Handle(GetPostsByTagsQuery request, CancellationToken cancellationToken)
        {
            var suitablePosts = new List<Post>();
            foreach (var tagId in request.TagIds)
            {
                var result = (await _unitOfWork.Tags.GetAsync(tag => tag.TagId == tagId, includeProperties: new string[] { "Posts.User" })).FirstOrDefault();
                if (result != null)
                {
                    suitablePosts.AddRange(result.Posts);
                }
            }
            return suitablePosts.DistinctBy(post => post.PostId);
            /*return await _unitOfWork.Posts.GetAsync(post => post.Tags.IntersectBy(request.TagIds, tag => tag.TagId).Any());*/
        }
    }
}
