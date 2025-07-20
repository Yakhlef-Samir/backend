using MediatR;
using WeCount.Application.Common.Interfaces.Repositories;
using WeCount.Application.DTOs;
using WeCount.Domain.Entities;
using WeCount.Domain.Enums;

namespace WeCount.Application.Debts.Commands;

public class CreateDebtCommandHandler : IRequestHandler<CreateDebtCommand, DebtDto>
{
    private readonly IDebtRepository _debtRepository;
    private readonly ICoupleRepository _coupleRepository;

    public CreateDebtCommandHandler(
        IDebtRepository debtRepository,
        ICoupleRepository coupleRepository
    )
    {
        _debtRepository = debtRepository;
        _coupleRepository = coupleRepository;
    }

    public async Task<DebtDto> Handle(
        CreateDebtCommand request,
        CancellationToken cancellationToken
    )
    {
        // Verify couple exists
        var couple = await _coupleRepository.GetByIdAsync(request.CoupleId);
        if (couple == null)
        {
            throw new Exception("Couple not found");
        }

        // Create new debt
        var debt = new Debt
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Amount = request.Amount,
            PaidAmount = 0, // Initially no payment
            InterestRate = request.InterestRate,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Type = request.Type,
            Status = PaymentStatus.Active, // Initially active
            CoupleId = request.CoupleId,
        };

        var createdDebt = await _debtRepository.CreateAsync(debt);

        return new DebtDto(
            createdDebt.Id,
            createdDebt.Title,
            createdDebt.Amount,
            createdDebt.PaidAmount,
            createdDebt.InterestRate,
            createdDebt.StartDate,
            createdDebt.EndDate,
            createdDebt.Type,
            createdDebt.Status,
            createdDebt.CoupleId
        );
    }
}
