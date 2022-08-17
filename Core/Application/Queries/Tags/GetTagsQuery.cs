using Application.UnitOfWorks;
using Domain.Models;
using MediatR;

namespace Application.Tags.Queries
{
    public record GetTagsQuery() : IRequest<IEnumerable<Tag>>;

    public class GetTagsHandler : IRequestHandler<GetTagsQuery, IEnumerable<Tag>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetTagsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<IEnumerable<Tag>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            return _unitOfWork.Tags.Get();
        }
    }
}
