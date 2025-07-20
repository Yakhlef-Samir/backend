using MediatR;
using WeCount.Application.DTOs;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Goals.Queries;

public class GetGoalByIdQueryHandler : IRequestHandler<GetGoalByIdQuery, GoalDto>
{
    private readonly IGoalRepository _goalRepository;

    public GetGoalByIdQueryHandler(IGoalRepository goalRepository)
    {
        _goalRepository = goalRepository;
    }

    public async Task<GoalDto> Handle(GetGoalByIdQuery request, CancellationToken cancellationToken)
    {
        var goal = await _goalRepository.GetByIdAsync(request.Id);
        if (goal is null)
        {
            return null!;
        }

        return new GoalDto(
            goal.Id,
            goal.Name,
            goal.TargetAmount,
            goal.SavedAmount,
            goal.Deadline,
            goal.Icon,
            goal.CoupleId,
            goal.IsCompleted,
            goal.Priority
        );
    }
}
