using MediatR;
using WeCount.Application.DTOs;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Debts.Queries;

public class GetDebtByIdQueryHandler : IRequestHandler<GetDebtByIdQuery, DebtDto>
{
    private readonly IDebtRepository _debtRepository;

    public GetDebtByIdQueryHandler(IDebtRepository debtRepository)
    {
        _debtRepository = debtRepository;
    }

    public async Task<DebtDto> Handle(GetDebtByIdQuery request, CancellationToken cancellationToken)
    {
        var debt = await _debtRepository.GetByIdAsync(request.Id);
        if (debt == null)
        {
            return null;
        }

        return new DebtDto(
            debt.Id,
            debt.Title,
            debt.Amount,
            debt.PaidAmount,
            debt.InterestRate,
            debt.StartDate,
            debt.EndDate,
            debt.Type,
            debt.Status,
            debt.CoupleId
        );
    }
}
