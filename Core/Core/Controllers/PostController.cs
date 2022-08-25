using Application.Commands.Posts;
using Application.Posts.Queries;
using Application.Queries.Posts;
using Application.Queries.Tags;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
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

        [HttpGet("tags={ids}")]
        public async Task<IActionResult> Get([FromRoute] string ids)
        {
            var tags = await GetTagsByIdsAsync(ids);
            return Ok(await _mediator.Send(new GetPostsByTagsQuery(tags)));
        }

        private async Task<IEnumerable<Tag>> GetTagsByIdsAsync(string idsString)
        {
            List<int> idsInt = idsString.Split(",").Select(x => Int32.Parse(x)).ToList();
            List<Tag> tags = new();
            foreach (int id in idsInt)
            {
                tags.Add(await _mediator.Send(new GetTagByIdQuery(id)));
            }
            return tags;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePostCommand post)
        {
            return Ok(await _mediator.Send(post));
        }
    }
}
