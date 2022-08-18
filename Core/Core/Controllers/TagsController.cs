using Application.Tags.Queries;
using Application.UnitOfWorks;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public TagsController(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            try
            {
                //return Ok(await _unitOfWork.Tags.Get());
                return Ok(await _mediator.Send(new GetTagsQuery()));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTag(int id)
        {
            try
            {
                Tag? result = await _unitOfWork.Tags.GetById(id);
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

        [HttpPost]
        public async Task<IActionResult> Create(Tag tag)//Maybe i should use DTO somewhere here...
        {
            try
            {
                if (tag == null)
                {
                    return BadRequest();
                }

                bool result = await _unitOfWork.Tags.Insert(tag);
                await _unitOfWork.SaveChanges();

                return result ? Ok("Tag successfully added") : Conflict("Tag not added");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new tag record");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Tag tag)
        {
            try
            {
                bool result = await _unitOfWork.Tags.Update(tag);
                await _unitOfWork.SaveChanges();


                return result ? Ok("Tag successfully updated") : NotFound($"Tag with id = {tag.TagId} not found");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new tag record");
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool result = await _unitOfWork.Tags.Delete(id);
                await _unitOfWork.SaveChanges();

                return result ? Ok($"Tag with id = {id} deleted") : NotFound($"Tag with id = {id} not found");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting tag record");
            }
        }
    }
}
