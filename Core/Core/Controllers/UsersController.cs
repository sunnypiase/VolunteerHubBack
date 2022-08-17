using Application.Commands.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginUserCommand loginUserCommand)
        {
            try
            {
                return Ok(await _mediator.Send(loginUserCommand));
            }
            catch (Exception)// TODO: Change here to some custom exceptions
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand registerUserCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(registerUserCommand);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception)// TODO: Change here to some custom exceptions
            {
                return BadRequest();
            }
        }
    }
}
