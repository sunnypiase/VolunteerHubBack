using Application.Common.Exceptions;
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
    public record UpdateTagCommand : IRequest<Tag>
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        //public ICollection<Post> Posts { get; set; }
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
            var tag = _unitOfWork.Tags.GetById(request.TagId).Result;

            if(tag == null)
            {
                throw new NotFoundException(nameof(Tag), request.TagId);
            }
            
            tag.Name = request.Name;
            //tag.Posts = request.Posts;

            await _unitOfWork.Tags.Update(tag);
            await _unitOfWork.SaveChanges();
            return tag;
        }
    }
}
