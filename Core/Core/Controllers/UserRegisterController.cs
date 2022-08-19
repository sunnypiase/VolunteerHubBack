using Application.Commands.Users;
using Application.Queries.Users;
using Application.UnitOfWorks;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegisterController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserRegisterController(IMediator mediator)
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
                return BadRequest("Model is not valid");
            }
            catch (Exception)// TODO: Change here to some custom exceptions
            {
                return BadRequest();
            }
        }
        //----------------------------------------
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
        //user put
        //[HttpPut]
        //[Authorize(Roles ="Volunteer","Needful")]

        //delete user
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                
                bool result = await _mediator.Send(new DeleteUserByIdCommand(id));
                await _unitOfWork.SaveChanges();

                return result ? Ok($"User with id = {id} deleted") : NotFound($"user with id = {id} not found");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting user record");
            }
        }

    }
}
