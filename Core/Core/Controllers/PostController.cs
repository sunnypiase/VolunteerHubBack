using Application.Commands.Posts;
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
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public PostController(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetPostsQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _mediator.Send(new GetPostByIdQuery(id)));
        }

        [HttpPost]
        public async Task<Post> Post([FromBody] Post post)
        {
            CreatePostCommand? model = new CreatePostCommand()
            {
                Title = post.Title,
                Description = post.Description,
                Image = post.Image,
                Tags = post.Tags,
                User = post.User,
                UserId = post.UserId,
            };
            return await _mediator.Send(model);
        }
    }
}
