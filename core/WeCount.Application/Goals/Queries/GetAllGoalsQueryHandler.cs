using MediatR;
using WeCount.Application.DTOs;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Goals.Queries;

public class GetAllGoalsQueryHandler : IRequestHandler<GetAllGoalsQuery, IEnumerable<GoalDto>>
{
    private readonly IGoalRepository _goalRepository;

    public GetAllGoalsQueryHandler(IGoalRepository goalRepository)
    {
        _goalRepository = goalRepository;
    }

    public async Task<IEnumerable<GoalDto>> Handle(
        GetAllGoalsQuery request,
        CancellationToken cancellationToken
    )
    {
        IEnumerable<Domain.Entities.Goal> goals;

        if (request.CoupleId.HasValue)
        {
            goals = await _goalRepository.GetByCoupleIdAsync(request.CoupleId.Value);
        }
        else
        {
            goals = await _goalRepository.GetAllAsync();
        }

        return goals.Select(g => new GoalDto(
            g.Id,
            g.Name,
            g.TargetAmount,
            g.SavedAmount,
            g.Deadline,
            g.Icon,
            g.CoupleId,
            g.IsCompleted,
            g.Priority
        ));
    }
}
