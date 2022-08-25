using Application.UnitOfWorks;
using Domain.Models;
using MediatR;
using System.Linq.Expressions;

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
            return await _unitOfWork.Posts.Get(
                filter: post => !post.Tags.Except(request.Tags).Any(), 
                commaSeparatedIncludeProperties: "Tags"
                );
            //return await _unitOfWork.Posts.Get(post => request.Tags.All(tag => post.Tags.Contains(tag)));
        }
    }
}
