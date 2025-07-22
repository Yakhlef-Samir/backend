using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeCount.Application.Budgets.Commands;
using WeCount.Application.Budgets.Queries;
using WeCount.Application.DTOs.Budget;

namespace WeCount.API.Controllers;

[ApiController]
[Route("api/budget")]
public class BudgetController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _mediator.Send(new GetBudgetCategoriesQuery());
        return Ok(categories);
    }

    [HttpPut("categories/{category}")]
    public async Task<IActionResult> UpdateCategory(
        string category,
        [FromBody] UpdateBudgetCategoryCommand command
    )
    {
        if (category != command.Category)
            return BadRequest("Category in URL must match category in body");

        BudgetDto? result = await _mediator.Send(command);
        if (result is null)
            return NotFound();
        return Ok(result);
    }
}
