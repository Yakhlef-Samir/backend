using MediatR;
using WeCount.Application.DTOs;
using WeCount.Domain.Enums;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Debts.Commands;

public class PayDebtCommandHandler : IRequestHandler<PayDebtCommand, DebtDto>
{
    private readonly IDebtRepository _debtRepository;

    public PayDebtCommandHandler(IDebtRepository debtRepository)
    {
        _debtRepository = debtRepository;
    }

    public async Task<DebtDto> Handle(PayDebtCommand request, CancellationToken cancellationToken)
    {
        // Get debt by id
        var debt = await _debtRepository.GetByIdAsync(request.Id);
        if (debt == null)
        {
            return null;
        }

        // Update paid amount
        debt.PaidAmount += request.Amount;

        // Update status if fully paid
        if (debt.PaidAmount >= debt.Amount)
        {
            debt.Status = PaymentStatus.Completed;
        }

        // Save changes
        await _debtRepository.UpdateAsync(debt);

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
