using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeCount.Application.Debts.Commands;
using WeCount.Application.Debts.Queries;

namespace WeCount.API.Controllers;

[ApiController]
[Route("api/debts")]
public class DebtController : ControllerBase
{
    private readonly IMediator _mediator;

    public DebtController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var debts = await _mediator.Send(new GetAllDebtsQuery());
        return Ok(debts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var debt = await _mediator.Send(new GetDebtByIdQuery(id));
        if (debt is null)
            return NotFound();
        return Ok(debt);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDebtCommand command)
    {
        var debt = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = debt.Id }, debt);
    }

    [HttpPatch("{id}/pay")]
    public async Task<IActionResult> Pay(Guid id, [FromBody] PayDebtCommand command)
    {
        if (id != command.Id)
            return BadRequest("Debt ID in URL must match debt ID in body");
            
        var result = await _mediator.Send(command);
        if (result is null)
            return NotFound();
        return Ok(result);
    }
}
