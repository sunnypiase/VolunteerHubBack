using Application.UnitOfWorks;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Tags
{
    public record CreateTagCommand(string Name) : IRequest<Tag>;

    public class CreateTagHandler : IRequestHandler<CreateTagCommand, Tag>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTagHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Tag> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = new Tag()
            {
                Name = request.Name
            };
            await _unitOfWork.Tags.Insert(tag);
            await _unitOfWork.SaveChanges();
            return tag;
        }
    }
}
