using Application.Commands.Users;
using Domain.Exceptions;
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
                if (ModelState.IsValid)
                {
                    return Ok(await _mediator.Send(loginUserCommand));
                }
                return BadRequest("User is not valid");
            }
            catch (WrongUserEmailOrPasswordException wrongDataException)
            {
                return BadRequest(wrongDataException.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
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
                    return Ok("Successfully registered!");
                }
                return BadRequest("User is not valid");
            }
            catch (EmailTakenByOtherUserException emailException)
            {
                return BadRequest(emailException.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
    }
}
