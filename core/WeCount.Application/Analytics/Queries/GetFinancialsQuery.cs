using MediatR;
using WeCount.Application.DTOs.Analytics;

namespace WeCount.Application.Analytics.Queries
{
    public record GetFinancialsQuery(int Months, Guid? CoupleId = null) : IRequest<FinancialsDto>;
}
