using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeCount.Application.Analytics.Queries;
using WeCount.Application.DTOs.Analytics;

namespace WeCount.API.Controllers;

[ApiController]
[Route("api/analytics")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnalyticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        // Récupérer l'ID du couple depuis les claims de l'utilisateur (à implémenter)
        // Pour l'instant, on passe null pour récupérer toutes les statistiques
        StatsDto stats = await _mediator.Send(new GetStatsQuery());
        return Ok(stats);
    }

    [HttpGet("financials")]
    public async Task<IActionResult> GetFinancials([FromQuery] int months = 6)
    {
        // Récupérer l'ID du couple depuis les claims de l'utilisateur (à implémenter)
        // Pour l'instant, on passe null pour récupérer toutes les données financières
        FinancialsDto financials = await _mediator.Send(new GetFinancialsQuery(months));
        return Ok(financials);
    }
}
