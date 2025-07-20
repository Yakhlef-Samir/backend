using MediatR;
using WeCount.Application.DTOs.Analytics;

namespace WeCount.Application.Analytics.Queries
{
    public record GetStatsQuery(Guid? CoupleId = null) : IRequest<StatsDto>;
}
