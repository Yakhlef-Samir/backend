using MediatR;
using WeCount.Application.DTOs;

namespace WeCount.Application.Goals.Queries;

public record GetGoalByIdQuery(Guid Id) : IRequest<GoalDto>;
