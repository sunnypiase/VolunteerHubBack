using Application.Services;
using Application.UnitOfWorks;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Users
{
    public record DeleteUserByIdCommand(int userId): IRequest;

    public class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserByIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
        {
            var userToDelete = (await _unitOfWork.Users.Get(user => user.UserId == request.userId)).FirstOrDefault();

            if (userToDelete == null)
            {
                throw new Exception();// TODO: Exeption that user don`t exist
            }

            await _unitOfWork.Users.Delete(userToDelete.UserId);
            await _unitOfWork.SaveChanges();

            return true;
        }
    }
}
