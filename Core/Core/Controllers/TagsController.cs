using Domain.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    //Controlles work but i think i need to do cleaner code in methods here (especially in Create and Update methods)
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly UnitOfWork _unit;
        public TagsController(UnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            try
            {
                return Ok(await _unit.TagRepository.Get());
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
                var result = await _unit.TagRepository.GetById(id);
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
        public async Task<IActionResult> Create(Tag tag)//Maybe i should use DTO somewhere here...(at least to add validation attrs)
        {
            try
            {
                if (tag == null)
                {
                    return BadRequest();
                }

                var createdTag = await _unit.TagRepository.Insert(tag);
                await _unit.SaveChanges();

                return CreatedAtAction(nameof(GetTag), new { id = createdTag.TagId }, createdTag);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new tag record");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Tag tag)//Should i return entity here? Do i need id as parameter?
        {
            try
            {
                if (id != tag.TagId)
                {
                    return BadRequest("Tag id mismatch");
                }

                var tagToUpdate = await _unit.TagRepository.GetById(id);

                if (tagToUpdate == null)
                {
                    return NotFound($"Tag with id = {id} not found");
                }

                tagToUpdate.Name = tag.Name;
                tagToUpdate.Posts = tag.Posts;

                await _unit.TagRepository.Update(tagToUpdate);
                await _unit.SaveChanges();
                return Ok(tagToUpdate);
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
                var tagToDelete = await _unit.TagRepository.GetById(id);

                if (tagToDelete == null)
                {
                    return NotFound($"Tag with id = {id} not found");
                }

                await _unit.TagRepository.Delete(tagToDelete);
                await _unit.SaveChanges();
                return Ok($"Tag with id = {id} deleted");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting tag record");
            }
        }
    }
}
