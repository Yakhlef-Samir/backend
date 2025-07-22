using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeCount.Application.Auth.Commands;
using WeCount.Application.Auth.Queries;
using WeCount.Application.DTOs;

namespace WeCount.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        AuthResponseDto? result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Me), new { }, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        AuthResponseDto? result = await _mediator.Send(command);
        if (result is null)
            return Unauthorized();
        return Ok(result);
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        // Get user ID from claims
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            return Unauthorized();

        UserProfileDto user = await _mediator.Send(new GetCurrentUserQuery(parsedUserId));
        if (user is null)
            return NotFound();
        return Ok(user);
    }
}
