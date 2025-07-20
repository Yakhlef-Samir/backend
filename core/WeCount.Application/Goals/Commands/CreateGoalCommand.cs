using MediatR;
using WeCount.Application.DTOs;

namespace WeCount.Application.Goals.Commands;

public record CreateGoalCommand(
    string Name,
    decimal TargetAmount,
    decimal SavedAmount,
    DateTime Deadline,
    string Icon,
    Guid CoupleId
) : IRequest<GoalDto>;
