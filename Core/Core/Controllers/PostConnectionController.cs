using Application.Commands.PostConnections;
using Application.Queries.PostConnections;
using Domain.Exceptions;
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPostConnections()
        {
            try
            {
                return Ok(await _mediator.Send(new GetPostConnectionsQuery()));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("postConnections")]
        [Authorize]
        public async Task<IActionResult> GetPostConnectionsOfAuthorizedUser()
        {
            try
            {
                return Ok(await _mediator.Send(new GetPostConnectionsByUserQuery(new JwtSecurityTokenHandler()
                    .ReadJwtToken(Request.Cookies["token"])
                    .Claims)));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CreatePostConnectionCommand post)
        {
            try
            {
                return Ok(await _mediator.Send(post));
            }
            catch (PostException postException)
            {
                return BadRequest(postException.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
    }
}
