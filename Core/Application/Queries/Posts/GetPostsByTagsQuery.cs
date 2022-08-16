using Application.UnitOfWorks;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Task<IEnumerable<Post>> Handle(GetPostsByTagsQuery request, CancellationToken cancellationToken)
        {
            return (Task<IEnumerable<Post>>)_unitOfWork.Posts.Get().Result.Where(post => !post.Tags.Except(request.Tags).Any());
        }
    }
}
