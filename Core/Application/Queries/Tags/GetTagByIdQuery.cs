using Application.Common.Exceptions;
using Application.UnitOfWorks;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Task<Tag> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Tags.GetById(request.TagId);
            if(result.Result == null)
            {
                throw new NotFoundException(nameof(Tag), request.TagId);
            }
            return result;
        }
    }
}
