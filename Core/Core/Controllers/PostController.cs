﻿using Application.Commands.Posts;
using Application.Queries.Posts;
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
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _mediator.Send(new GetPostByIdQuery(id)));
        }

        [HttpGet("by-tags")]
        public async Task<IActionResult> GetByTags([FromQuery] int[] tagIds)
        {
            return Ok(await _mediator.Send(new GetPostsByTagsQuery(tagIds)));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePostCommand post)
        {
            return Ok(await _mediator.Send(post));
        }
    }
}
