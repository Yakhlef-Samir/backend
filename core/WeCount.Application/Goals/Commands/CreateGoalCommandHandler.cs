using MediatR;
using WeCount.Application.DTOs;
using WeCount.Domain.Entities;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Goals.Commands;

public class CreateGoalCommandHandler : IRequestHandler<CreateGoalCommand, GoalDto>
{
    private readonly IGoalRepository _goalRepository;
    private readonly ICoupleRepository _coupleRepository;

    public CreateGoalCommandHandler(
        IGoalRepository goalRepository,
        ICoupleRepository coupleRepository
    )
    {
        _goalRepository = goalRepository;
        _coupleRepository = coupleRepository;
    }

    public async Task<GoalDto> Handle(
        CreateGoalCommand request,
        CancellationToken cancellationToken
    )
    {
        // Verify couple exists
        var couple = await _coupleRepository.GetByIdAsync(request.CoupleId);
        if (couple is null)
        {
            throw new Exception("Couple not found");
        }

        // Create new goal
        var goal = new Goal
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            TargetAmount = request.TargetAmount,
            SavedAmount = request.SavedAmount,
            Deadline = request.Deadline,
            Icon = request.Icon,
            CoupleId = request.CoupleId,
        };

        var createdGoal = await _goalRepository.CreateAsync(goal);

        return new GoalDto(
            createdGoal.Id,
            createdGoal.Name,
            createdGoal.TargetAmount,
            createdGoal.SavedAmount,
            createdGoal.Deadline,
            createdGoal.Icon,
            createdGoal.CoupleId,
            createdGoal.IsCompleted,
            createdGoal.Priority
        );
    }
}
