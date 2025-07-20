using MediatR;
using WeCount.Application.DTOs;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Debts.Queries;

public class GetAllDebtsQueryHandler : IRequestHandler<GetAllDebtsQuery, IEnumerable<DebtDto>>
{
    private readonly IDebtRepository _debtRepository;

    public GetAllDebtsQueryHandler(IDebtRepository debtRepository)
    {
        _debtRepository = debtRepository;
    }

    public async Task<IEnumerable<DebtDto>> Handle(GetAllDebtsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Debt> debts;
        
        if (request.CoupleId.HasValue)
        {
            debts = await _debtRepository.GetByCoupleIdAsync(request.CoupleId.Value);
        }
        else
        {
            debts = await _debtRepository.GetAllAsync();
        }

        return debts.Select(d => new DebtDto(
            d.Id,
            d.Title,
            d.Amount,
            d.PaidAmount,
            d.InterestRate,
            d.StartDate,
            d.EndDate,
            d.Type,
            d.Status,
            d.CoupleId
        ));
    }
}
