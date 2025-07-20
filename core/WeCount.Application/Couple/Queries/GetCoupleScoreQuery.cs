using MediatR;
using WeCount.Application.DTOs.Couple;

namespace WeCount.Application.Couple.Queries
{
    public record GetCoupleScoreQuery(Guid CoupleId) : IRequest<CoupleScoreDto>;
}
