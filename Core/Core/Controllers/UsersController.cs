using Application.Commands.Posts;
using Application.Commands.Users;
using Application.Queries.Users;
using Domain.Exceptions;
using Domain.Models;
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
        public async Task<IActionResult> Login(LoginUserCommand loginUserCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = await _mediator.Send(loginUserCommand);
                    HttpContext.Response.Cookies.Append("token", token, new CookieOptions
                    {
                        MaxAge = TimeSpan.FromDays(1)
                    });
                    return Ok("Successfully logged in");
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

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Response.Cookies.Delete("token");
            return Ok("Successfully logged out");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                return Ok(await _mediator.Send(new GetUsersQuery()));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {

                User? result = await _mediator.Send(new GetUserByIdQuery(id));
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
        [HttpGet("{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                User? result = await _mediator.Send(new GetUserByEmailQuery(email));
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch(UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
    }
}
