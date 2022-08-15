using Application.UnitOfWorks;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tags.Queries
{

    public record GetTagsQuery() : IRequest<IEnumerable<Tag>>;

    public class GetTagsHandler : IRequestHandler<GetTagsQuery, IEnumerable<Tag>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator mediator;

        public GetTagsHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            this.mediator = mediator;
        }
        public Task<IEnumerable<Tag>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            return _unitOfWork.Tags.Get();
        }
    }
}
