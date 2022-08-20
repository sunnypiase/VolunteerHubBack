using Application.UnitOfWorks;
using Domain.Exceptions;
using Domain.Models;
using MediatR;

namespace Application.Queries.Tags
{
    public record GetTagByIdQuery(int TagId) : IRequest<Tag>;

    public class GetTagByIdHandler : IRequestHandler<GetTagByIdQuery, Tag>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetTagByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Tag> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Tags.GetById(request.TagId);

            if(result == null)
            {
                throw new TagNotFoundException(request.TagId);
            }

            return result;
        }
    }
}
