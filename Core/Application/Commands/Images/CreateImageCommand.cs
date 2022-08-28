using Application.Repositories;
using Application.Repositories.Abstractions;
using Domain.Models;
using MediatR;

namespace Application.Commands.Images
{
    public class CreateImageCommand : IRequest<Image>
    {
        public string ImagePath { get; init; }

        public CreateImageCommand(string imagePath)
        {
            ImagePath = imagePath;
        }
    }
    public class CreateImageHandler : IRequestHandler<CreateImageCommand, Image>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobRepository _blobRepository;

        public CreateImageHandler(IUnitOfWork unitOfWork, IBlobRepository blobRepository)
        {
            _unitOfWork = unitOfWork;
            _blobRepository = blobRepository;
        }

        public async Task<Image> Handle(CreateImageCommand request, CancellationToken cancellationToken)
        {
            string? format = request.ImagePath[(request.ImagePath.LastIndexOf('.') + 1)..];
            Image image = new()
            {
                Format = format,
            };
            await _unitOfWork.Images.InsertAsync(image);
            await _unitOfWork.SaveChangesAsync();
            await _blobRepository.UploadImage(request.ImagePath, image.ToString());
            return image;

        }
    }
}
