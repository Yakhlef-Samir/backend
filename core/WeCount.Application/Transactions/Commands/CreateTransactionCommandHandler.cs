using MediatR;
using WeCount.Application.Common.Interfaces;
using WeCount.Application.Common.Mapping;
using WeCount.Application.DTOs;
using WeCount.Domain.Entities.Transaction;
using WeCount.Domain.ValueObjects;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Transactions.Commands;

public class CreateTransactionCommandHandler
    : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapperService _mapper;

    public CreateTransactionCommandHandler(
        ITransactionRepository transactionRepository,
        IUserRepository userRepository,
        IMapperService mapper
    )
    {
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<TransactionDto> Handle(
        CreateTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = request.Description,
            Amount = request.Amount,
            Date = request.Date,
            Category = new Category(request.Category, request.Category),
            UserId = request.UserId,
            UserName = $"{user.Name.FirstName} {user.Name.LastName}",
            CoupleId = user.CoupleId,
            IsShared = request.IsShared && user.CoupleId != Guid.Empty,
        };

        var createdTransaction = await _transactionRepository.CreateAsync(transaction);

        return new TransactionDto(
            createdTransaction.Id,
            createdTransaction.Description,
            createdTransaction.Amount,
            createdTransaction.Date,
            createdTransaction.Category.ToString(),
            createdTransaction.UserId,
            createdTransaction.UserName,
            createdTransaction.CoupleId,
            createdTransaction.IsShared
        );
    }
}
