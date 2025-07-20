using MongoDB.Driver;
using WeCount.Domain.Entities;
using WeCount.Infrastructure.Common; // pagination extensions
using WeCount.Infrastructure.Interfaces;

namespace WeCount.Infrastructure.Repositories.DebtRepository
{
    public class DebtRepository(IMongoDbContext context) : IDebtRepository
    {
        private readonly IMongoCollection<Debt> _debts = context.Debts;

        public Task<Debt?> GetByIdAsync(Guid id) => _debts.GetByIdAsync(id)!;

        public async Task<IEnumerable<Debt>> GetByCoupleIdAsync(
            Guid coupleId,
            int page = 1,
            int pageSize = 20
        ) => await _debts.Find(_ => true).Paginate(page, pageSize).ToListAsync();

        public async Task<IEnumerable<Debt>> GetAllAsync(int page = 1, int pageSize = 20) =>
            await _debts.Find(_ => true).Paginate(page, pageSize).ToListAsync();

        public Task<Debt> CreateAsync(Debt debt) => _debts.CreateAsync(debt);

        public Task<bool> UpdateAsync(Debt debt) => _debts.UpdateAsync(debt);

        public Task<bool> DeleteAsync(Guid id) => _debts.DeleteAsync(id);
    }
}
