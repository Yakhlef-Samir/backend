using MediatR;
using WeCount.Application.Common.Interfaces;
using WeCount.Application.Common.Interfaces.Repositories;
using WeCount.Application.Common.Mapping;
using WeCount.Application.DTOs;
using WeCount.Domain.Entities;
using WeCount.Domain.Entities.Transaction;
using WeCount.Domain.Exceptions;
using WeCount.Domain.ValueObjects;

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
        User? user =
            await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID {request.UserId} not found");

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

        Transaction createdTransaction = await _transactionRepository.CreateAsync(transaction);

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
