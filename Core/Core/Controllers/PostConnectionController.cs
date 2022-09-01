using Application.Commands.PostConnections;
using Application.Queries.PostConnections;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

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
        [Authorize]
        public async Task<IActionResult> GetPostConnectionsOfAuthorizedUser()
        {
            return Ok(await _mediator.Send(new GetPostConnectionsByUserQuery(Request.Cookies["token"])));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CreatePostConnectionCommand post)
        {
            return Ok(await _mediator.Send(post));
        }
    }
}
