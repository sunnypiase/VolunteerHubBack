using Application.UnitOfWorks;
using Domain.Exceptions;
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
            if (!await _unitOfWork.Tags.Delete(request.TagId))
            {
                throw new TagNotFoundException(request.TagId);
            }

            await _unitOfWork.SaveChanges();
            return Unit.Value;
        }
    }
}
