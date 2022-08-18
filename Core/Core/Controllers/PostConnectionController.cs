using Application.Commands.PostConnections;
using Application.Posts.Queries;
using Application.Queries.Posts;
using Application.UnitOfWorks;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostConnectionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public PostConnectionController(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<PostConnection> Post([FromBody] CreatePostConnectionCommand post)
        {

            return await _mediator.Send(post);
        }
    }
}
