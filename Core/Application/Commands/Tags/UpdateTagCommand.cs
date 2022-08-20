using Application.UnitOfWorks;
using Domain.Exceptions;
using Domain.Models;
using MediatR;

namespace Application.Commands.Tags
{
    public record UpdateTagCommand : IRequest<Tag>
    {
        public int TagId { get; set; }
        public string Name { get; set; }
    }

    public class UpdateTagHandler : IRequestHandler<UpdateTagCommand, Tag>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTagHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Tag> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = new Tag()
            {
                TagId = request.TagId,
                Name = request.Name
            };

            if (!await _unitOfWork.Tags.Update(tag))
            {
                throw new TagNotFoundException(tag.TagId);
            }

            await _unitOfWork.SaveChanges();
            return tag;
        }
    }
}
