using Application.Common.Exceptions;
using Application.UnitOfWorks;
using Domain.Models;
using MediatR;

namespace Application.Commands.Tags
{
    public record DeleteTagCommand(int TagId) : IRequest;

    public class DeleteTagHandler : IRequestHandler<DeleteTagCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTagHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Tags.Delete(request.TagId);
            if (result == false)
            {
                throw new NotFoundException(nameof(Tag), request.TagId);
            }
            await _unitOfWork.SaveChanges();
            return Unit.Value;
        }
    }
}
