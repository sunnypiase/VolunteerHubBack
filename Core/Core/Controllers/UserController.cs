using Application.Tags.Queries;
using Application.UnitOfWorks;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public UserController(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                //return Ok(await _unitOfWork.Tags.Get());
                return Ok(await _mediator.Send(new GetUsersQuery()));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                Tag? result = await _unitOfWork.Users.GetById(id);
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
        [HttpPut]
        public async Task<IActionResult> UpdateUser(User user)
        {
            try
            {
                bool result = await _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChanges();


                return result ? Ok("User successfully updated") : NotFound($"User with id = {user.UserId} not found");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new user record");
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                bool result = await _unitOfWork.Users.Delete(id);
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
