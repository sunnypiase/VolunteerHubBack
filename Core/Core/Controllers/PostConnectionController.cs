using Application.Commands.PostConnections;
using Application.Queries.PostConnections;
using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostConnectionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostConnectionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPostConnections()
        {
            return Ok(await _mediator.Send(new GetPostConnectionsQuery()));
        }

        [HttpGet("currentUser")]
        [Authorize(Roles = "Volunteer,Needful")]
        public async Task<IActionResult> GetPostConnectionsOfAuthorizedUser()
        {
            return Ok(await _mediator.Send(new GetPostConnectionsByUserQuery(Request.Cookies["token"])));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Volunteer,Needful,Admin")]
        public async Task<IActionResult> GetPostConnectionOfAuthorizedUser(int id)
        {
            var result = await _mediator.Send(new GetPostConnectionByIdQuery(Request.Cookies["token"], id));

            if (result != null)
                return Ok(result);

            return Forbid();
        }

        [HttpPost]
        [Authorize(Roles = "Volunteer,Needful")]
        public async Task<IActionResult> Post([FromBody] CreatePostConnectionRequest post)
        {
            return Ok(await _mediator.Send(new CreatePostConnectionCommand(post.Title, post.Message, post.VolunteerPostId,
                post.NeedfulPostId, Request.Cookies["token"])));
        }
    }
}
