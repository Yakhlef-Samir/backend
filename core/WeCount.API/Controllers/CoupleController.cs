using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeCount.Application.Couple.Commands;
using WeCount.Application.Couple.Queries;

namespace WeCount.API.Controllers;

[ApiController]
[Route("api/couple")]
[Authorize]
public class CoupleController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoupleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview()
    {
        // Récupérer l'ID du couple depuis les claims de l'utilisateur (à implémenter)
        // Pour l'instant, on utilise un ID fixe pour les tests
        var coupleId = Guid.Parse("00000000-0000-0000-0000-000000000001"); // À remplacer par la récupération depuis les claims

        var overview = await _mediator.Send(new GetCoupleOverviewQuery(coupleId));
        return Ok(overview);
    }

    [HttpGet("score")]
    public async Task<IActionResult> GetScore()
    {
        // Récupérer l'ID du couple depuis les claims de l'utilisateur (à implémenter)
        // Pour l'instant, on utilise un ID fixe pour les tests
        var coupleId = Guid.Parse("00000000-0000-0000-0000-000000000001"); // À remplacer par la récupération depuis les claims
        
        var score = await _mediator.Send(new GetCoupleScoreQuery(coupleId));
        
        // Sauvegarder automatiquement le score dans l'historique
        await _mediator.Send(new SaveCoupleScoreCommand(
            coupleId,
            score.Score,
            score.BudgetScore,
            score.GoalsScore,
            score.DebtScore,
            score.SavingsScore,
            score.TransactionsScore,
            score.Message
        ));
        
        return Ok(score);
    }

    [HttpGet("score/history")]
    public async Task<IActionResult> GetScoreHistory([FromQuery] int? months = null)
    {
        // Récupérer l'ID du couple depuis les claims de l'utilisateur (à implémenter)
        // Pour l'instant, on utilise un ID fixe pour les tests
        var coupleId = Guid.Parse("00000000-0000-0000-0000-000000000001"); // À remplacer par la récupération depuis les claims

        var history = await _mediator.Send(new GetCoupleScoreHistoryQuery(coupleId, months));
        return Ok(history);
    }
}
