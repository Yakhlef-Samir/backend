using MediatR;
using WeCount.Application.DTOs;

namespace WeCount.Application.Goals.Queries;

public record GetAllGoalsQuery(Guid? CoupleId = null) : IRequest<IEnumerable<GoalDto>>;
