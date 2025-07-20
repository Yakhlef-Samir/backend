using WeCount.Domain.Entities.Transaction;

namespace WeCount.Application.Common.Interfaces.Repositories;

public interface ITransactionCategoryRepository
{
    Task<TransactionCategory?> GetByNameAsync(string name);
    Task<IEnumerable<TransactionCategory>> GetAllAsync();
    Task<TransactionCategory> CreateAsync(TransactionCategory category);
    Task<bool> UpdateAsync(TransactionCategory category);
    Task<bool> DeleteAsync(string name);
}

