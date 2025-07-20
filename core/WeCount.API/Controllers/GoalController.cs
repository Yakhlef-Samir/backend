using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeCount.Application.Goals.Commands;
using WeCount.Application.Goals.Queries;

namespace WeCount.API.Controllers;

[ApiController]
[Route("api/goals")]
public class GoalController : ControllerBase
{
    private readonly IMediator _mediator;

    public GoalController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var goals = await _mediator.Send(new GetAllGoalsQuery());
        return Ok(goals);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var goal = await _mediator.Send(new GetGoalByIdQuery(id));
        if (goal is null)
            return NotFound();
        return Ok(goal);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGoalCommand command)
    {
        var goal = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = goal.Id }, goal);
    }
}
