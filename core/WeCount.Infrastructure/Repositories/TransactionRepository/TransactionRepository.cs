using MongoDB.Driver;
using WeCount.Domain.Entities.Transaction;
using WeCount.Infrastructure.Common;
using WeCount.Application.Common.Interfaces.Repositories;
using WeCount.Application.Common.Interfaces;

namespace WeCount.Infrastructure.Repositories.TransactionRepository
{
    public class TransactionRepository(IMongoDbContext context) : ITransactionRepository
    {
        private readonly IMongoCollection<Transaction> _transactions = context.Transactions;

        public Task<Transaction?> GetByIdAsync(Guid id) => _transactions.GetByIdAsync(id)!;

        public async Task<IEnumerable<Transaction>> GetByCoupleIdAsync(
            Guid coupleId,
            int page = 1,
            int pageSize = 20
        ) =>
            await _transactions
                .Find(t => t.CoupleId == coupleId)
                .Paginate(page, pageSize)
                .ToListAsync();

        public async Task<IEnumerable<Transaction>> GetAllAsync(int page = 1, int pageSize = 20) =>
            await _transactions.Find(_ => true).Paginate(page, pageSize).ToListAsync();

        public Task<Transaction> CreateAsync(Transaction transaction) =>
            _transactions.CreateAsync(transaction);

        public Task<bool> UpdateAsync(Transaction transaction) =>
            _transactions.UpdateAsync(transaction);

        public Task<bool> DeleteAsync(Guid id) => _transactions.DeleteAsync(id);
    }
}
