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
    public record GetPostByIdQuery(int PostId) : IRequest<IEnumerable<Post>>;

    public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, IEnumerable<Post>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPostByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<IEnumerable<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_unitOfWork.Posts.Get().Result.Where(x => x.PostId == request.PostId));
        }
    }
}
