using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeCount.Application.Transactions.Commands;
using WeCount.Application.Transactions.Queries;

namespace WeCount.API.Controllers;

[ApiController]
[Route("api/transactions")]
public class TransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var transactions = await _mediator.Send(new GetAllTransactionsQuery());
        return Ok(transactions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var transaction = await _mediator.Send(new GetTransactionByIdQuery(id));
        if (transaction is null)
            return NotFound();
        return Ok(transaction);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _mediator.Send(new GetTransactionCategoriesQuery());
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTransactionCommand command)
    {
        var transaction = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
    }
}
