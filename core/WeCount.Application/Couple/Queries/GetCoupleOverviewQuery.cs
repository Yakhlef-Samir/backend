using MediatR;
using WeCount.Application.DTOs.Couple;

namespace WeCount.Application.Couple.Queries
{
    public record GetCoupleOverviewQuery(Guid CoupleId) : IRequest<CoupleOverviewDto>;
}
