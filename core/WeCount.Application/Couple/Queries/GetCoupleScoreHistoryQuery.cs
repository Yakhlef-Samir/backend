using MediatR;
using WeCount.Application.DTOs.Couple;

namespace WeCount.Application.Couple.Queries
{
    public record GetCoupleScoreHistoryQuery(Guid CoupleId, int? Months = null) : IRequest<CoupleScoreHistoryDto>;
}
