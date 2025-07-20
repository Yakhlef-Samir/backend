using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeCount.Domain.Entities.Transaction;

namespace WeCount.Application.Common.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(Guid Id);
        Task<IEnumerable<Transaction>> GetByCoupleIdAsync(Guid coupleId, int page = 1, int pageSize = 20);
        Task<IEnumerable<Transaction>> GetAllAsync(int page = 1, int pageSize = 20);
        Task<Transaction> CreateAsync(Transaction transaction);
        Task<bool> UpdateAsync(Transaction transaction);
        Task<bool> DeleteAsync(Guid Id);
    }
}

