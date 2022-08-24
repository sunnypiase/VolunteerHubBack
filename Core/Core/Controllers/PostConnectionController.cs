﻿using Application.Commands.PostConnections;
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

        [HttpGet]
        [Authorize(Roles = "Admin")] // TODO: I would propose to extract roles to constants since the "Admin" literal appears multiple times
        public async Task<IActionResult> GetPostConnections()
        {
            return Ok(await _mediator.Send(new GetPostConnectionsQuery()));
        }

        [HttpGet("postConnections")]
        [Authorize]
        public async Task<IActionResult> GetPostConnectionsOfAuthorizedUser()
        {
            return Ok(await _mediator.Send(new GetPostConnectionsByUserQuery(new JwtSecurityTokenHandler()
                .ReadJwtToken(Request.Cookies["token"])
                .Claims)));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CreatePostConnectionCommand post)
        {
            return Ok(await _mediator.Send(post));
        }
    }
}
