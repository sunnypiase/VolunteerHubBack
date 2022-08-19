using Application.Commands.Tags;
using Application.Queries.Tags;
using Application.Tags.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _mediator.Send(new GetTagsQuery()));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id) 
        {
            try
            {
                return Ok(await _mediator.Send(new GetTagByIdQuery(id)));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTagCommand tag) => Ok(await _mediator.Send(tag));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTagCommand tag)
        {
            try
            {
                return Ok(await _mediator.Send(tag));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return Ok(await _mediator.Send(new DeleteTagCommand(id)));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
